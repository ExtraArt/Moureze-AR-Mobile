using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognition : MonoBehaviour
{
    private ARTrackedImageManager _arTrackedImageManager;
    private Camera arCam;
    private GameObject instanceOfPanorama;
    private void Awake()
    {
        _arTrackedImageManager = Object.FindFirstObjectByType<ARTrackedImageManager>();
    }

    void Start()
    {
        arCam = this.gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        
    }
    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            //do something
        }
    }
}
