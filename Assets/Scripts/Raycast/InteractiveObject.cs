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
    [SerializeField]
    private Vector3 defaultScalePanorama;
    [SerializeField]
    private Vector3 zoomScalePanorama = new Vector3(3, 3, 3);
    [SerializeField]
    private ParticleSystem blowVFX;

    private Vector2 touchPosition = default;
    private Vector3 savePanoramaStartPosition;

    [SerializeField]
    private GameObject objectToFind;
    private GameObject instanceOfObjectToFind;
    private GameObject theHitObject;
    private GameObject storeTheHitedObject;

    private bool isInPanoramaView = false;
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
                    savePanoramaStartPosition = hitObject.transform.position;

                    if (placementObject != null)
                    {
                        theHitObject = hitObject.collider.gameObject;
                        if (hitObject.collider.tag == "Panorama")
                        {
                            storeTheHitedObject = hitObject.collider.gameObject;
                            defaultScalePanorama = hitObject.transform.localScale;
                            EnterInPanoramaView(theHitObject);
                        }
                        if (hitObject.collider.tag == "ObjectToFind")
                        {
                            SuccessfullyFindObject();
                        }
                        //ChangeSelectedObject(placementObject);
                    }
                    if (isInPanoramaView == true)
                    {
                        FixPanoramaRotation(theHitObject);
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
                //ToggleZoomInPanoramaView(theHitObject);
            }
            else
            {
                current.IsSelected = true;
                //selected.transform.localScale += new Vector3(2, 2, 2);
                //ToggleZoomInPanoramaView(theHitObject);
            }
        }
    }
    void EnterInPanoramaView(GameObject selectedPanorama)
    {
        if (isInPanoramaView == false)
        {
            selectedPanorama.transform.position = arCamera.transform.position;
            selectedPanorama.transform.localScale = zoomScalePanorama;
            FixPanoramaRotation(selectedPanorama);

            isInPanoramaView = true;
        }
        InstanciateObjectToFindInPanorama();
        //HideAllInactivePanorama(selectedPanorama);

    }

    public void LeavePanormama()
    {
        storeTheHitedObject.transform.position = arCamera.transform.position + savePanoramaStartPosition;
        storeTheHitedObject.transform.localScale = defaultScalePanorama;
        isInPanoramaView = false;
        instanceOfObjectToFind.GetComponent<DestroyObjectToFind>().DestroyObject();
        //UnhideEveryPanoramas();
    }
    private void InstanciateObjectToFindInPanorama()
    {
        //Vector3 randomizePosition = new Vector3(Random.Range(0, 2), 0, Random.Range(0,2));
        //Instantiate(objectToFind, new Vector3(arCamera.transform.position.x + randomizePosition.x, arCamera.transform.position.y, arCamera.transform.position.z + randomizePosition.z), Quaternion.identity);
        instanceOfObjectToFind = Instantiate(objectToFind, new Vector3(arCamera.transform.position.x + 1, arCamera.transform.position.y, arCamera.transform.position.z + 3), Quaternion.identity);
        instanceOfObjectToFind.tag = "ObjectToFind";
    }
    private void SuccessfullyFindObject()
    {
        if (objectToFind != null)
        {
            instanceOfObjectToFind.GetComponent<DestroyObjectToFind>().DestroyObject();
            Instantiate(blowVFX, instanceOfObjectToFind.transform.position, Quaternion.identity);
        }
    }

    private void HideAllInactivePanorama(GameObject gameObjectHitedByRaycast)
    {
        GameObject[] collectionOfPanorama = GameObject.FindGameObjectsWithTag("Panorama");

        if (isInPanoramaView == true)
        {
            foreach (var panorama in collectionOfPanorama)
            {
                panorama.SetActive(false);
                if (gameObjectHitedByRaycast.name == panorama.name)
                {
                    gameObjectHitedByRaycast.SetActive(true);
                }
            }
        }
    }
    private void UnhideEveryPanoramas()
    {
        GameObject[] collectionOfPanoramas = GameObject.FindGameObjectsWithTag("Panorama");
        foreach (var panoramaToShow in collectionOfPanoramas)
        {
            panoramaToShow.SetActive(true);
        }
    }
    private void FixPanoramaRotation(GameObject panoramaWithWrongRotation)
    {
        //panoramaWithWrongRotation.transform.LookAt(arCamera.transform.position);
        Vector3 eulerRotation = new Vector3(transform.eulerAngles.x, arCamera.transform.eulerAngles.y, transform.eulerAngles.z);
        panoramaWithWrongRotation.transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
