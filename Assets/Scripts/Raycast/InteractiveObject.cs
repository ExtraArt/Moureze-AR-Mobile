using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private PlacementObject[] placedObjects;

    private Vector2 touchPosition = default;
    private Vector3 savePanoramaStartPosition;

    private GameObject theHitObject;
    private bool zoomInPanoramaActive;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    private void Start()
    {

    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {
                    PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
                    if (placementObject != null)
                    {
                        theHitObject = hitObject.collider.gameObject;
                        ChangeSelectedObject(placementObject);
                        /*
                        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
                        {
                            //ToggleZoomInPanoramaView(theHitObject);
                        }
                        */
                    }
                }
            }

        }
    }

    void ChangeSelectedObject(PlacementObject selected)
    {
        foreach(PlacementObject current in placedObjects)
        {
            
            if (selected != current)
            {
                current.IsSelected = false;
            }
            else
            {
                current.IsSelected = true;
                //selected.transform.localScale += new Vector3(2, 2, 2);
                ToggleZoomInPanoramaView(theHitObject);
            }
        }
    }
    void ToggleZoomInPanoramaView(GameObject selectedPanorama)
    {
        if (zoomInPanoramaActive == false)
        {
            selectedPanorama.transform.position = arCamera.transform.position;
            selectedPanorama.transform.localScale += new Vector3(2, 2, 2);
            zoomInPanoramaActive = !zoomInPanoramaActive;
        }
        else
        {
            selectedPanorama.transform.position = savePanoramaStartPosition;
            selectedPanorama.transform.localScale -= new Vector3(2, 2, 2);
            zoomInPanoramaActive = !zoomInPanoramaActive;
        }
    }
}
