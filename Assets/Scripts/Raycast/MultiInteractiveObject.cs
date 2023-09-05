using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private GameObject[] arObjectsToPlace;
    [SerializeField]
    private Text imageTrackedText;
    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.2f, 0.2f, 0.2f);

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

    private void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }
    private void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }

    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArges)
    {
        foreach (ARTrackedImage trackedImage in eventArges.added)
        {
            UpdateARImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArges.updated)
        {
            UpdateARImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArges.removed)
        {
            arObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        imageTrackedText.text = trackedImage.referenceImage.name;
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);
        Debug.Log($"trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        if (arObjectsToPlace != null)
        {
            GameObject goARObject = arObjects[name];
            goARObject.SetActive(true);
            goARObject.transform.position = newPosition;
            goARObject.transform.localScale = scaleFactor;
            foreach (GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if (go.name != name)
                {
                    go.SetActive(false);
                }
            }

        }
    }
}