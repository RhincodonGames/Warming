using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRing : MonoBehaviour
{
    public SelectionManager selectionManager;
    public int segments = 60;
    public float yOffset = 0.01f;

    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.loop = true;
        line.useWorldSpace = true;

        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        DrawCircle();
    }

    void DrawCircle()
    {
        Vector3 center = transform.position;
        float angleStep = 360f / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angle = angleStep * i;

            float radius = selectionManager.interactionDistance;

            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(center.x + x, center.y + yOffset, center.z + z));
        }
    }

    private void Update()
    {
        DrawCircle();
    }
}
