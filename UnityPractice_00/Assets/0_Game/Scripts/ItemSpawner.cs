using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject spawnObject;
    [SerializeField] float spawnCDTimer = 5;
    [SerializeField] float timer = 0;
    [SerializeField] bool preSpawn = false;
    public bool isSpawn = false;
    // [SerializeField] bool isSpawn = false;
    GameObject spawnedObject;

    private void Start()
    {
        if (preSpawn)
        {
            Spawn();
            isSpawn = true;
        }
    }

    void Update()
    {
        if (isSpawn)
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > spawnCDTimer)
            {
                Spawn();
                timer = 0;
                isSpawn=true;
            }
        }
    }

    void Spawn()
    {
        spawnedObject = Instantiate<GameObject>(spawnObject, transform);
        spawnedObject.GetComponent<NPCController>().spawner = this;
    }
}
