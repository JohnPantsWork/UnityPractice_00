using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] GameObject boss;
    CharacterController bossCharCtrl;
    [SerializeField] GameObject slider;
    float originalHP;
    float currentHP;

    private void Awake()
    {
        bossCharCtrl = boss.GetComponent<CharacterController>();
        originalHP = bossCharCtrl.healthPoint;
    }

    private void Update()
    {
        currentHP = bossCharCtrl.healthPoint;
        GameObject.Find("Slider03_BasicType").GetComponent<Slider>().value = currentHP / originalHP;
    }
}
