using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class UIBlockRaycast : MonoBehaviour
{
    private ARRaycastManager arRayCastManager;

    [SerializeField]
    private GameObject placedPrefab;
    [SerializeField]
    private GameObject uiPanel;
    [SerializeField]
    private Button toggleButton;
    [SerializeField]
    private Camera arCam;

    [SerializeField]
    private ParticleSystem vfxBlow;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject theHitObjectOfUIScript;
    private void Awake()
    {
        arRayCastManager = GetComponent<ARRaycastManager>();
    }

    public void Toggle()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        var toggleButtonText = toggleButton.gameObject.GetComponent<TextMeshProUGUI>();
        toggleButtonText.text = uiPanel.activeSelf ? "UI OFF " : " UI ON";
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var touchPosition = touch.position;
                bool isOverUI = touchPosition.IsPointingOverUIObject();

                if (isOverUI == true)
                {
                    Debug.Log("Not allow to click on the UI");
                }
                if (isOverUI == false && arRayCastManager.Raycast(touchPosition, hits))
                {
                    var hitPose = hits[0].pose;
                    Instantiate(vfxBlow, hitPose.position, hitPose.rotation);
                }
            }
            /*
            Ray ray = arCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                theHitObjectOfUIScript = hit.collider.gameObject;
                if (theHitObjectOfUIScript.tag == "UI")
                {
                    isOverUI = true;
                }
            }

            */
        }
    }
}
