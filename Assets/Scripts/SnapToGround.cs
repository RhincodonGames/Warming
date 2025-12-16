using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SnapToGround : MonoBehaviour
{
    public float raycastHeight = 50f;
    public string groundTag = "Ground";

    void Start()
    {
        Snap();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
            Snap();
    }
#endif

    public void Snap()
    {
        Vector3 origin = transform.position + Vector3.up * raycastHeight;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                transform.position = hit.point;
            }
        }
    }
}
