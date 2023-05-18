using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private float dash_modifier = 5f;
    [SerializeField] private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    void Start()
    {
        if(!IsOwner) return;
        CameraFollow.Instance.FollowPlayer(transform);
    }
    private void Update()
    {
        if(!IsOwner) return;
        Vector3 moveDir = new Vector3(0, 0, 0);
        if(Input.GetKey(KeyCode.W)) moveDir.y = +1f;
        if(Input.GetKey(KeyCode.S)) moveDir.y = -1f;
        if(Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if(Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        float moveSpeed = 5f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}