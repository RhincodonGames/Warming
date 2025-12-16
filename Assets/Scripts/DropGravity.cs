using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropGravity : MonoBehaviour
{
    Rigidbody rb;
    bool settled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (settled)
            return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;   // freezes completely
            settled = true;
        }
    }
}
