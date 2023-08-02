using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class RaycastUsingRay : MonoBehaviour
{
    public Transform rayContent;
    [SerializeField]
    private Camera camera;

    public Transform screenContent;

    List<ARRaycastHit> m_RaycastHits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        var screenPosition = touch.position;
        var ray = camera.ScreenPointToRay(screenPosition);

        var manager = GetComponent<ARRaycastManager>();

        if (manager.Raycast(ray, m_RaycastHits, TrackableType.PlaneWithinPolygon))
        {
            var pose = m_RaycastHits[0].pose;
            rayContent.transform.SetPositionAndRotation(pose.position, pose.rotation);
        }

        if (manager.Raycast(screenPosition, m_RaycastHits, TrackableType.PlaneWithinPolygon))
        {
            var pose = m_RaycastHits[0].pose;
            screenContent.transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
    }
}