using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;

    private void Update()
    {
        transform.position = new Vector3(
            cinemachineCamera.VirtualCameraGameObject.transform.position.x,
            cinemachineCamera.VirtualCameraGameObject.transform.position.y,
            gameObject.transform.position.z
            );
    }
}