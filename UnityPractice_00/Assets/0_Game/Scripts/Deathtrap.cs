using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathtrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        other.gameObject.GetComponent<CharacterController>().healthPoint=0;
    }
}
