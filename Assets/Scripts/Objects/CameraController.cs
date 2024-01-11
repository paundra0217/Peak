using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera Camera;

    private CameraController _instance;
    public CameraController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogErrorFormat("Camera Controller is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        Camera = GetComponent<CinemachineVirtualCamera>();
    }

    public void AttachCamera(GameObject ObjectToAttach)
    {
        Transform transform = ObjectToAttach.transform;
        Camera.Follow = transform;
    }

    public void DetachCamera()
    {
        Camera.Follow = null;
    }
}
