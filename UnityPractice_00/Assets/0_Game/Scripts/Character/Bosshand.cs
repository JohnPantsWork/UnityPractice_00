using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosshand : MonoBehaviour
{
    [SerializeField] GameObject boss;
    CharacterController charCtrl;

    private void Awake()
    {
        charCtrl = boss.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    // private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "PlayerWeapon")
        {
            charCtrl.healthPoint -= 100;
        }
    }
    void OnParticleCollision(GameObject other)//bow attack
    {
        if (other.gameObject.tag == "PlayerWeapon")
        {
            charCtrl.healthPoint -= 100;
        }
    }
}
