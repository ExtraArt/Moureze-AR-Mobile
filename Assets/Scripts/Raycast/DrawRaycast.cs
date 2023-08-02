using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRaycast : MonoBehaviour
{
    private Ray forwardRay;

    void Update()
    {
        forwardRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(forwardRay.origin, forwardRay.direction * 10, Color.magenta);
        OnDrawGizmos();
    }

    private void OnDrawGizmos()
    {
        RaycastHit hitInfo;
        Gizmos.color = Color.magenta;

        if (Physics.Raycast(forwardRay, out hitInfo))
        {
            Gizmos.DrawSphere(hitInfo.point, 0.1f);
        }
    }
}
