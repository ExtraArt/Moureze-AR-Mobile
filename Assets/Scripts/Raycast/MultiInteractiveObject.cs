using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class MultiInteractiveObject : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager m_RaycastManager;
    private ARTrackedImageManager m_TrackedImageManager;
    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private PlacementObject[] placedObjects;
    [SerializeField]
    private GameObject[] arObjectsToPlace;
    [SerializeField]

    private Vector3 scaleFactor = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector2 touchPosition = default;
    private Vector3 savePanoramaStartPosition;

    private GameObject theHitObject;
    private bool zoomInPanoramaActive;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();

        foreach (GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            arObjects.Add(arObject.name, newARObject);
        }
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

    private void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += On;
    }
}
