using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;

public class Monke_Y : NetworkBehaviour
{
    public float punchForce = 10f;
    public LayerMask punchMask;
    public Collider2D handCollider;

    void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Collider[] hitColliders = Physics.OverlapSphere(handCollider.transform.position, 0.5f, punchMask);
            int i = 0;
            while (i < hitColliders.Length) {
                Rigidbody rb = hitColliders[i].GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.AddForce(transform.forward * punchForce);
                }
                i++;
            }
        }
    }
}
