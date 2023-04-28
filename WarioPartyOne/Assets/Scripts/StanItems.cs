using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StanItems : MonoBehaviour
{
    public bool isHoldingItem;
    private bool isUsingTelephoto;
    public GameObject normalCameraRange;
    public GameObject newCameraRange;
    public GameObject normalCameraRangeColliderObject;
    public GameObject newCameraRangeColliderObject;

    private RawImage batteryOutline;

    private void Awake()
    {
        batteryOutline = GameObject.Find("BatteryOutline").GetComponent<RawImage>();
    }

    void Start()
    {
        isUsingTelephoto = false;
        isHoldingItem = false;
    }

    public bool CheckIfUsingTelephoto()
    {
        return isUsingTelephoto;
    }

    void ActivateNewCamera()
    {
        isUsingTelephoto = true;
        GetComponent<PlayerManager>().UpdateResource(3);
        normalCameraRange.SetActive(false);
        normalCameraRangeColliderObject.SetActive(false);
        newCameraRange.SetActive(true);
        newCameraRangeColliderObject.SetActive(true);
        batteryOutline.color = Color.blue;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && isHoldingItem)
        {
            ActivateNewCamera();
            isHoldingItem = false;
        }

        if (isUsingTelephoto && GetComponent<PlayerManager>().resources == 0)
        {
            isUsingTelephoto = false;
            normalCameraRange.SetActive(true);
            normalCameraRangeColliderObject.SetActive(true);
            newCameraRange.SetActive(false);
            newCameraRangeColliderObject.SetActive(false);
            batteryOutline.color = Color.white;
        }
    }
}
