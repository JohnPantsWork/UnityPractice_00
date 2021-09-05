using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public enum atkType
    {
        melee,//prefab trigger
        bow//particle system
    }

    float damageAmount = 0;
    [SerializeField] atkType attackType = atkType.melee;
    public CharacterController charCtrl;//will fill by the Instantiater

    float autoDelTime = 0.1f;
    float calculateTime = 0;
    void Update()
    {
        if (attackType == atkType.melee)//if melee attack not hit anyone,still destroy after 0.1f;
        {
            calculateTime += Time.deltaTime;
            if (calculateTime > autoDelTime)
            {
                DestroyMeleePrefab();
            }
        }
    }

    void OnTriggerEnter(Collider other)//melee attack
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.name != "EvilSkeleton")
        {
            if (attackType == atkType.melee)
            {
                CharacterController otherCharCtrl = other.gameObject.GetComponent<CharacterController>();
                damageAmount = charCtrl.meleeAttackPoint;
                otherCharCtrl.healthPoint -= damageAmount;
                DestroyMeleePrefab();
                Instantiate<GameObject>(otherCharCtrl.enemyHitFx, other.transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }

    void OnParticleCollision(GameObject other)//bow attack
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (attackType == atkType.bow)
            {
                CharacterController otherCharCtrl = other.gameObject.GetComponent<CharacterController>();
                damageAmount = charCtrl.arrowAttackPoint;
                otherCharCtrl.healthPoint -= damageAmount;
                Instantiate<GameObject>(otherCharCtrl.enemyHitFx, other.transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }

    void DestroyMeleePrefab()
    {
        GameObject.Destroy(this.gameObject);
    }
}
