using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImproveTrackingImage : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = Object.FindFirstObjectByType<ARTrackedImageManager>();

        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnPrefabs.Add(prefab.name, newPrefab);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnPrefabs[name];

        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach (GameObject go in spawnPrefabs.Values)
        {
            if (go.name != name)
            {
                go.SetActive(false);
            }
        }
    }
}
