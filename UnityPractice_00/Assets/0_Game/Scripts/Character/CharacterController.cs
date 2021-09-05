using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public enum direction
    {
        right,
        left
    }

    [Header("player control basic")]
    public direction faceDir = direction.right;
    [SerializeField] GameObject[] weaponSlot;//0=sword、1=shield、2=armor、3=bow
    [SerializeField] int weaponState = 0;//0=melee、1=bow
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rig;
    [SerializeField] GameObject invincibleFx;
    [SerializeField] GameObject playerHitFx;
    [SerializeField] GameObject shieldHitFx;
    public GameObject enemyHitFx;
    [SerializeField] bool needLockZ = true;

    [Header("player state switch")]
    [SerializeField] bool isFalling = false;
    [SerializeField] bool isMoving = false;
    [SerializeField] bool isDefending = false;
    [SerializeField] bool isJumping = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isSpeaking = false;
    public bool isInvincible = false;
    public bool isDeath = false;
    [SerializeField] bool isHurting = false;
    public bool haveBow = false;
    public bool haveArmor = false;
    public bool haveShield = false;

    [Header("movement detial")]
    public Vector3 rightYDirection;
    public Vector3 leftYDirection;
    [SerializeField] float defaultSpeed = 5.0f;
    [SerializeField] float jumpHight = 8.0f;
    [SerializeField] float defaultAttackCD = 0.5f;
    [SerializeField] float defaultJumpResetTime = 5f;
    [SerializeField] float jumpResetTime = 0;
    Vector3 lockZ = new Vector3(0, 0, 0);

    [Header("battle detial")]
    public float healthPoint = 200f;
    public float meleeAttackPoint = 100f;
    public float arrowAttackPoint = 100f;
    [SerializeField] float attackCD = 1;
    [SerializeField] float getHurtkCD = 1;
    float hurtResetTime = 0;
    [SerializeField] GameObject meleePrafab;
    [SerializeField] Vector3 meleeAttackRightPos = new Vector3(0, 1, 1);
    [SerializeField] Vector3 meleeAttackLeftPos = new Vector3(0, 1, 1);
    [SerializeField] ParticleSystem arrowParticle;

    [Header("npc interact")]
    public GameObject NPCTarget = null;
    [SerializeField] GameObject ActiveFlowChart;
    public GameObject DeathFlowChart;

    [Header("Enemy Special")]
    public GameObject[] enmeySkills;

    void Awake()
    {
        InitManager();
    }

    void FixedUpdate()
    {
        DetectState();
    }

    void OnParticleCollision(GameObject other)
    {
        if (isInvincible) { return; }
        if (gameObject.tag == "Player")//player get hit special solution
        {
            if (other.gameObject.tag == "EnemyWeapon")
            {
                GetHurt();
            }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (isInvincible) { return; }
        if (gameObject.tag == "Player")//player get hit special solution
        {
            if (col.gameObject.tag == "EnemyWeapon")
            {
                GetHurt();
            }
        }
    }

    public void InitManager()
    {

        //init faceDirection
        rightYDirection = new Vector3(0, 90, 0);
        leftYDirection = new Vector3(0, 270, 0);

        //init characterAnimator
        animator = gameObject.GetComponent<Animator>();

        //init characterRigidbody
        rig = gameObject.GetComponent<Rigidbody>();
    }

    public void DetectState()
    {
        if (needLockZ)
        {
            lockZ = new Vector3(transform.position.x, transform.position.y, 0);
            transform.position = lockZ;
        }


        //Death or not
        if (healthPoint <= 0)
        {
            if (isDeath == false)
            {
                Death();
            }
            return;
        }

        if (isHurting)
        {
            if (hurtResetTime < getHurtkCD)
            {
                hurtResetTime += Time.deltaTime;
                invincibleFx.SetActive(true);
            }
            else
            {
                invincibleFx.SetActive(false);
                hurtResetTime = 0;
                isHurting = false;
            }
        }

        faceDir = CheckPlayerFaceDirection();

        //reset characterIsOnTheGorund
        int layerMask = 1 << LayerMask.NameToLayer("Environment");
        RaycastHit hit;
        Vector3 offectRayPos = new Vector3(0f, 0.5f, 0f);
        if (Physics.Raycast(transform.position + offectRayPos, transform.TransformDirection(Vector3.down), out hit, 0.6f, layerMask))
        {
            isFalling = false;
            animator.ResetTrigger("Jump");
            animator.SetBool("isFalling", false);
        }
        else
        {
            isFalling = true;
            animator.SetBool("isFalling", true);
        }

        //reset characterIsJumping
        if (isFalling == false && isJumping == true)
        {
            isJumping = false;
        }

        //reset animator.isDefinding when characterIsDefending==false
        if (isDefending == false)
        {
            animator.ResetTrigger("Defend");
            animator.SetBool("isDefending", false);
        }

        //reset characterIsAttacking
        if (isAttacking == true)
        {
            attackCD += Time.deltaTime;
            if (attackCD >= defaultAttackCD)
            {
                isAttacking = false;
                attackCD = 0;
            }
        }

        if (haveShield && weaponState == 0)
        {
            weaponSlot[1].SetActive(true);
        }
        else if (weaponSlot.Length != 0)
        {
            weaponSlot[1].SetActive(false);
        }

        if (haveArmor)
        {
            weaponSlot[2].SetActive(true);
        }
        else if (weaponSlot.Length != 0)
        {
            weaponSlot[2].SetActive(false);
        }
    }

    void GetHurt()
    {
        if (isInvincible) { return; }

        if (isHurting) { return; }

        if (isDefending)
        {
            Instantiate<GameObject>(shieldHitFx, transform.position + Vector3.up, Quaternion.identity);
            isHurting = true;
        }
        else if (haveShield && weaponState == 0)
        {
            Instantiate<GameObject>(playerHitFx, transform.position + Vector3.up, Quaternion.identity);
            haveShield = false;
            isHurting = true;
        }
        else if (haveArmor)
        {
            Instantiate<GameObject>(playerHitFx, transform.position + Vector3.up, Quaternion.identity);
            haveArmor = false;
            isHurting = true;
        }
        else
        {
            Instantiate<GameObject>(playerHitFx, transform.position + Vector3.up, Quaternion.identity);
            healthPoint -= 200;
        }

    }

    public direction CheckPlayerFaceDirection()
    {
        if (transform.eulerAngles.y < 100f && transform.eulerAngles.y > 80f)
        {
            return direction.right;
        }
        else
        {
            return direction.left;
        }
    }

    public void TurnCharacter(direction dir)
    {
        if (dir == direction.right)
        {
            transform.rotation = Quaternion.Euler(rightYDirection);
            faceDir = direction.right;
        }
        else
        {
            transform.rotation = Quaternion.Euler(leftYDirection);
            faceDir = direction.left;
        }
    }

    public void Move(float horizontalMove)
    {
        if (isAttacking || isSpeaking)
        {
            return;
        }

        if (isFalling == false)
        {
            animator.SetTrigger("Move");
        }

        float speed = defaultSpeed;
        if (isDefending == true)
        {
            speed /= 2f;
        }

        horizontalMove *= speed;//effect by speed
        horizontalMove *= Time.deltaTime;//make unit/sec

        transform.Translate(0, 0, horizontalMove);
        animator.SetBool("isMoving", true);
        isMoving = true;
    }

    public void Idle()
    {
        isMoving = false;
        animator.SetBool("isMoving", false);
    }

    public void Jump()
    {
        if (isJumping == false && isFalling == false && isDefending == false && isSpeaking == false)
        {
            animator.SetBool("isMoving", false);
            animator.SetTrigger("Jump");

            Vector3 v3 = new Vector3(0, jumpHight, 0);
            rig.AddForce(v3, ForceMode.Impulse);

            isJumping = true;
        }
    }

    public void Defend()
    {
        if (isFalling == false && isAttacking == false && isSpeaking == false)
        {
            animator.SetBool("isDefending", true);
            animator.SetTrigger("Defend");

            isDefending = true;
        }
    }

    public void Undefend()
    {
        if (isFalling == false)
        {
            isDefending = false;
        }
    }

    public void SwitchWeapon()
    {
        //shield and armor switch in detect function
        if (isAttacking == true || isFalling == true) { return; }

        if (weaponState == 0 && haveBow == true)
        {
            weaponSlot[0].SetActive(false);
            weaponSlot[3].SetActive(true);
            weaponState = 1;
        }
        else if (weaponState == 1)
        {
            weaponSlot[3].SetActive(false);
            weaponSlot[0].SetActive(true);
            weaponState = 0;
        }
    }

    public void Attack()
    {
        if (isDefending == false && isAttacking == false && isFalling == false && isMoving == false && isSpeaking == false)
        {
            if (weaponState == 0)
            {
                MeleeAttack();
            }
            else if (weaponState == 1)
            {
                BowAttack();
            }
        }
    }

    public void MeleeAttack()
    {
        isAttacking = true;
        animator.SetTrigger("AttackMelee");

        if (gameObject.tag == "Enemy")//enemy dont spawn damage trigger
        {
            return;
        }

        Vector3 playerV3 = transform.position;
        GameObject obj;

        if (faceDir == direction.right)
        {
            obj = Instantiate<GameObject>(meleePrafab, playerV3 + meleeAttackRightPos, Quaternion.identity);
        }
        else
        {
            obj = Instantiate<GameObject>(meleePrafab, playerV3 + meleeAttackLeftPos, Quaternion.identity);
        }
        obj.GetComponent<DamageObject>().charCtrl = this; //tell meleePrafab who shoot it

    }

    public void BowAttack()
    {
        isAttacking = true;
        animator.SetTrigger("AttackBow");
        arrowParticle.Play();
    }

    public void Speak()
    {
        NPCController npcCtrl = NPCTarget.GetComponent<NPCController>();
        if (npcCtrl == null) { return; }

        if (NPCTarget != null || isSpeaking == false)
        {
            isSpeaking = true;
            isInvincible = true;
            NPCTarget.GetComponent<NPCController>().flowChart.SetActive(true);
        }
    }

    public void EndSpeak()
    {
        NPCTarget.GetComponent<NPCController>().flowChart.SetActive(false);
        isSpeaking = false;
        isInvincible = false;
    }

    void Death()
    {
        isDeath = true;
        animator.SetBool("isDead", true);
        animator.SetTrigger("Death");

        rig.useGravity = false;
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        Collider[] cols = GetComponentsInChildren<Collider>();

        foreach (var item in cols)
        {
            Destroy(item);
        }

        if (DeathFlowChart != null)
        {
            DeathFlowChart.SetActive(true);
        }
        if (GetComponent<InputController>())
        {
            Invoke("ReloadTheLevel", 2);
        }
        else
        {
            Invoke("DisableEnemyPrefab", 2);
        }
    }

    void ReloadTheLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void DisableEnemyPrefab()
    {
        this.gameObject.SetActive(false);
    }
}