using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject cutscene;
    GameObject catchObj;


    private void Start()
    {
        Invoke("PlayCutscene", 2f);
        Invoke("DisableCutscene", 30f);
    }

    private void Update()
    {
        if (Input.GetKey("p"))
        {
            DisableCutscene();
        }
    }

    void PlayCutscene()
    {
        catchObj = Instantiate<GameObject>(cutscene, transform);
    }

    void DisableCutscene()
    {
        catchObj.SetActive(false);
    }


    public void ReloadLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
