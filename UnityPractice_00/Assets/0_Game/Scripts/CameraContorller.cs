using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContorller : MonoBehaviour
{
    [SerializeField] Transform cameraTargetTransform;
    [SerializeField] Vector3 offsetV3;
    [SerializeField] float followSpeed = 1f;
    //private Transform cameraTransform;

    void Start()
    {
        //cameraTransform=this.transfrom;
    }

    void Update()
    {
        //transform.LookAt(cameraTargetTransform);
        float lerpTime = followSpeed * Time.deltaTime;
        this.transform.position = Vector3.Lerp(this.transform.position, offsetV3 + cameraTargetTransform.position, lerpTime);
        // this.transform.position = offsetV3 + cameraTargetTransform.position;
    }
}
