using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;

public class Monke_Y : NetworkBehaviour
{
    public float punchForce = 10f;
    public float punchRadius = 1.5f;
    public LayerMask punchLayerMask;
    public Transform handTransform;

//RPCs
    [ServerRpc]
    void PunchServerRpc()
    {
        Punch();
    }
    [ClientRpc]
    void PunchClientRpc()
    {
        Punch();
    }

//Functions
    void Punch()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(handTransform.position, punchRadius, punchLayerMask);
        foreach (Collider2D hitCollider in hitColliders)
        {
            NetworkObject hitNetworkObject = hitCollider.GetComponent<NetworkObject>();
            if (hitNetworkObject != null)
            {
                Vector2 direction = hitNetworkObject.transform.position - handTransform.position;
                if (hitNetworkObject != null && hitNetworkObject != this.NetworkObject)
                {
                    hitNetworkObject.GetComponent<CharStatController>().LoseHealth(20);
                    hitNetworkObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * punchForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(IsClient){if(IsOwner){PunchServerRpc();}}
            if(IsServer){if(IsOwner){PunchClientRpc();}}
        }
    }
}
