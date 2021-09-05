using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneOver : MonoBehaviour
{
    public PlayableDirector director;
    [SerializeField] GameObject beginningFlowChart;

    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
    private void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            gameObject.SetActive(false);
        }
    }
}
