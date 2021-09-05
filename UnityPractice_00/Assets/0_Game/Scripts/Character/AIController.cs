using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public CharacterController charCtrl;

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
    public bool aiIsIdle = false;
    
    [Header("CheckTarget")]
    public GameObject player;
    float currentDistance = 100;
    [SerializeField] float detectRange = 8;

    void Awake()
    {
        charCtrl = gameObject.GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (var item in skillWeight)
        {
            totalSkillWeight += item;
        }
        aiRandomBehaviorCD = defaultAIRandomBehaviorCD;
    }

    void Update()
    {
        AIControl();
    }

    void AIControl()
    {
        if (charCtrl.healthPoint <= 0f)
        {
            return;
        }

        //deside what to do
        aiRandomTime += Time.deltaTime;
        if (aiRandomTime >= aiRandomBehaviorCD)
        {
            aiRandomBehaviorCD = defaultAIRandomBehaviorCD;

            charCtrl.Idle();
            aiIsMoving = false;
            aiIsIdle = false;

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
                    aiRandomBehaviorCD = moveTime;
                    aiIsMoving = true;
                    break;
                case 1:
                    charCtrl.TurnCharacter(CharacterController.direction.left);
                    break;
                case 2:
                    charCtrl.TurnCharacter(CharacterController.direction.right);
                    break;
                case 3:
                    aiIsIdle = true;
                    break;
                case 4:
                    charCtrl.Attack();
                    charCtrl.enmeySkills[0].GetComponent<ParticleSystem>().Play();
                    break;
                case 5:
                    charCtrl.Attack();
                    charCtrl.enmeySkills[1].GetComponent<ParticleSystem>().Play();
                    break;
                case 6:
                    charCtrl.Attack();
                    charCtrl.enmeySkills[2].GetComponent<ParticleSystem>().Play();
                    break;
                default:
                    break;
            }
            aiRandomTime = 0f;
            skillIndex = 0;
        }

        if (PlayerInDetectrange() && player.GetComponent<CharacterController>().isDeath == false)
        {
            FaceToPlayer();
        }

        if (aiIsIdle)
        {
            charCtrl.Idle();
        }
        else if (aiIsMoving)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Environment");

            if (charCtrl.faceDir == CharacterController.direction.left)
            {
                Vector3 offectRayPosLeft = new Vector3(-2f, 0.5f, 0f);
                RaycastHit hitLeft;
                if (Physics.Raycast(this.transform.position + offectRayPosLeft, transform.TransformDirection(Vector3.down), out hitLeft, 0.501f, layerMask))
                {
                    charCtrl.Move(1f);
                }
                else
                {
                    if (PlayerInDetectrange())
                    {
                        charCtrl.Move(0.1f);
                    }
                    else
                    {
                        charCtrl.TurnCharacter(CharacterController.direction.right);
                    }
                }
            }

            if (charCtrl.faceDir == CharacterController.direction.right)
            {
                Vector3 offectRayPosRight = new Vector3(2f, 0.5f, 0f);
                RaycastHit hitRight;
                if (Physics.Raycast(this.transform.position + offectRayPosRight, transform.TransformDirection(Vector3.down), out hitRight, 0.501f, layerMask))
                {
                    charCtrl.Move(1f);
                }
                else
                {
                    if (PlayerInDetectrange())
                    {
                        charCtrl.Move(0.1f);
                    }
                    else
                    {
                        charCtrl.TurnCharacter(CharacterController.direction.left);
                    }
                }
            }
        }
    }

    void FaceToPlayer()
    {
        if (transform.position.x - player.transform.position.x >= 0)
        {
            charCtrl.TurnCharacter(CharacterController.direction.left);
        }
        else if (transform.position.x - player.transform.position.x <= 0)
        {
            charCtrl.TurnCharacter(CharacterController.direction.right);
        }
    }

    bool PlayerInDetectrange()
    {
        currentDistance = Vector3.Distance(transform.position, player.transform.position);
        if (currentDistance < detectRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
