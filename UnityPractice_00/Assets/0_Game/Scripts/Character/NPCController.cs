using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NPCController : MonoBehaviour
{
    enum NPCType
    {
        Bow,
        armor,
        shield,
        npc
    }

    [SerializeField] NPCType npcType;
    [SerializeField] GameObject particleObj;
    [SerializeField] Transform parent;
    public GameObject flowChart;
    public ItemSpawner spawner;

    public GameObject player;
    float currentDistance = 100;
    [SerializeField] float detectRange = 8;
    CharacterController playerCtrl;

    void Start()
    {
        parent = GameObject.Find("Pools").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        playerCtrl = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (npcType == NPCType.npc)
        {
            SetAsNPC();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharacterController charCtrl = other.GetComponent<CharacterController>();
            if (npcType == NPCType.Bow)
            {
                charCtrl.haveBow = true;
                DestroyNPC();
            }
            else if (npcType == NPCType.armor)
            {
                charCtrl.haveArmor = true;
                DestroyNPC();
            }
            else if (npcType == NPCType.shield)
            {
                charCtrl.haveShield = true;
                DestroyNPC();
            }
            else if (npcType == NPCType.npc)
            {
                charCtrl.NPCTarget = this.gameObject;
            }
        }
    }
    void DestroyNPC()
    {
        if (spawner != null)
        {
            spawner.GetComponent<ItemSpawner>().isSpawn = false;
        }
        Instantiate<GameObject>(particleObj, transform.position, Quaternion.identity, parent);
        GameObject.Destroy(this.gameObject);
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

    void SetAsNPC()
    {

        if (PlayerInDetectrange())
        {
            playerCtrl.NPCTarget = gameObject;
        }
        else if (playerCtrl.NPCTarget == gameObject)
        {
            playerCtrl.NPCTarget = null;
        }
    }

    public void ResetSpeakAndFungus()
    {
        playerCtrl.EndSpeak();
    }
}
