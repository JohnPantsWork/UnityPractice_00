using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField] GameObject boss;

    bool lockPortal = true;

    private void Update()
    {
        if (boss.TryGetComponent<CharacterController>(out CharacterController charCtrl))
        {
            if (charCtrl.isDeath == true)
            {
                lockPortal = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("coll");
        if (lockPortal == false && other.gameObject.tag == "Player")
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
