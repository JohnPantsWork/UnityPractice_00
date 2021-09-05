using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public CharacterController charCtrl;
    public Animator animator;

    [Header("Skills")]
    [SerializeField] float defaultAIRandomBehaviorCD = 1f;
    float aiRandomBehaviorCD = 1;
    [SerializeField] float aiRandomTime = 0;
    [SerializeField] float moveTime = 3f;
    public int randomActionIndex;
    [SerializeField] int[] skillWeight;
    int totalSkillWeight = 0;
    int skillIndexValue = 0;
    int skillIndex = 0;

    [Header("Moving")]
    public bool aiIsMoving = false;

    [Header("Follow")]
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 offsetV3;
    [SerializeField] float followSpeed = 1f;

    [Header("CatchTarget")]
    public GameObject player;
    float currentDistance = 100;
    [SerializeField] float detectRange = 8;

    [Header("#$%^&*")]
    bool isDead = false;
    [SerializeField] GameObject bossDeadParticle;
    [SerializeField] Transform parent;

    void Awake()
    {
        parent = GameObject.Find("Pools").transform;
        animator = GetComponent<Animator>();
        charCtrl = gameObject.GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (var item in skillWeight)
        {
            totalSkillWeight += item;
        }
        aiRandomBehaviorCD = defaultAIRandomBehaviorCD;
    }

    // Update is called once per frame
    void Update()
    {
        BossControl();
    }

    void BossControl()
    {

        if (charCtrl.healthPoint <= 0f)
        {
            if (isDead == false)
            {
                Dead();
                isDead = true;
            }
            return;
        }

        //deside what to do
        aiRandomTime += Time.deltaTime;
        if (aiRandomTime >= aiRandomBehaviorCD)
        {
            aiIsMoving = false;

            aiRandomBehaviorCD = defaultAIRandomBehaviorCD;

            randomActionIndex = Random.Range(0, totalSkillWeight);

            foreach (var item in skillWeight)
            {
                skillIndexValue += item;
                if (randomActionIndex < skillIndexValue)
                {
                    skillIndexValue = 0;
                    break;
                }
                skillIndex++;
            }

            switch (skillIndex)
            {
                case 0:
                    aiIsMoving = true;
                    break;
                case 1:
                    aiRandomBehaviorCD = moveTime;
                    animator.SetTrigger("AttackLeft");
                    break;
                case 2:
                    aiRandomBehaviorCD = moveTime;
                    animator.SetTrigger("AttackRight");
                    break;
                case 3:
                    animator.SetTrigger("AttackBow");
                    charCtrl.enmeySkills[0].GetComponent<ParticleSystem>().Play();
                    break;
                default:
                    break;
            }
            aiRandomTime = 0f;
            skillIndex = 0;
        }

        if (aiIsMoving)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        float lerpTime = followSpeed * Time.deltaTime;
        this.transform.position = Vector3.Lerp(this.transform.position, offsetV3 + targetTransform.position, lerpTime);
        this.transform.position = Vector3.Lerp(this.transform.position, offsetV3 + targetTransform.position, lerpTime);
    }

    void Dead()
    {
        player.GetComponent<CharacterController>().isInvincible = true;
        charCtrl.DeathFlowChart.SetActive(true);
        Instantiate<GameObject>(bossDeadParticle, parent);
        Invoke("Destroy", 2f);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
