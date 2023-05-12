using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;

public class Monke_Y : NetworkBehaviour
{
    public float punchForce = 10f;
    public float punchRadius = 0.5f;
    public LayerMask punchLayerMask;

    [ServerRpc]
    void PunchServerRpc()
    {
        Debug.Log("PunchServerRpc");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, punchRadius, punchLayerMask);
        foreach (Collider2D hitCollider in hitColliders)
        {
            NetworkObject hitNetworkObject = hitCollider.GetComponent<NetworkObject>();
            if (hitNetworkObject != null)
            {
                Vector2 direction = hitNetworkObject.transform.position - transform.position;
                if (hitNetworkObject != null && hitNetworkObject != this.NetworkObject)
                {
                    hitNetworkObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * punchForce, ForceMode2D.Impulse);
                }
            }
        }
    }
    [ClientRpc]
    void PunchClientRpc()
    {
        Debug.Log("PunchClientRpc");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, punchRadius, punchLayerMask);
        foreach (Collider2D hitCollider in hitColliders)
        {
            NetworkObject hitNetworkObject = hitCollider.GetComponent<NetworkObject>();
            if (hitNetworkObject != null)
            {
                Vector2 direction = hitNetworkObject.transform.position - transform.position;
                if (hitNetworkObject != null && hitNetworkObject != this.NetworkObject)
                {
                    hitNetworkObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * punchForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(IsClient){PunchServerRpc();}
            if(IsServer){PunchClientRpc();}
        }
    }
}
