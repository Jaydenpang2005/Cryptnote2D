using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FollowPlayer(Transform transform)
    {
        cinemachineVirtualCamera.Follow = transform;
    }
}
