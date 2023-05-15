using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StanItems : MonoBehaviour
{
    public bool isHoldingItem;
    public bool isUsingTelephoto;
/*    public GameObject normalCameraRange;
    public GameObject newCameraRange;
    public GameObject normalCameraRangeColliderObject;
    public GameObject newCameraRangeColliderObject;*/

    public SpriteRenderer normalCameraRange;
    public SpriteRenderer newCameraRange;
    public Collider2D normalCameraRangeColliderObject;
    public Collider2D newCameraRangeColliderObject;

    public ActionButton normalAction;
    public ActionButton newAction;

    private RawImage batteryOutline;
    public GameObject lowBatteryIndicator;

    private void Awake()
    {
        batteryOutline = GameObject.Find("BatteryOutline").GetComponent<RawImage>();
    }

    void Start()
    {
        isUsingTelephoto = false;
        isHoldingItem = false;

        normalCameraRange.enabled = true;
        normalCameraRangeColliderObject.enabled = true;

        newCameraRange.enabled = false;
        newCameraRangeColliderObject.enabled = false;

        normalAction.enabled = true;
        newAction.enabled = false;

    }

    public bool CheckIfUsingTelephoto()
    {
        return isUsingTelephoto;
    }

    void ActivateNewCamera()
    {
        isUsingTelephoto = true;
        GetComponent<PlayerManager>().UpdateResource(3);
        /*normalCameraRange.SetActive(false);
        normalCameraRangeColliderObject.SetActive(false);
        newCameraRange.SetActive(true);
        newCameraRangeColliderObject.SetActive(true);*/
        normalCameraRange.enabled = false;
        normalCameraRangeColliderObject.enabled = false;
        normalAction.enabled = false;
        newCameraRange.enabled = true;
        newCameraRangeColliderObject.enabled = true;
        newAction.enabled = true; 

        batteryOutline.color = Color.blue;
        lowBatteryIndicator.SetActive(false);
    }

    void Update()
    {
        if (!GetComponent<PlayerManager>().enabled) return;

        if (Input.GetKeyDown(KeyCode.O) && isHoldingItem)
        {
            ActivateNewCamera();
            isHoldingItem = false;
        }

        if (GetComponent<PlayerManager>().resources == 0)
        {
            lowBatteryIndicator.SetActive(true);
            normalCameraRange.enabled = false;
        } else if (GetComponent<PlayerManager>().resources > 0 && !isUsingTelephoto)
        {
            lowBatteryIndicator.SetActive(false);
            normalCameraRange.enabled = true;
        }

        if (isUsingTelephoto && GetComponent<PlayerManager>().resources == 0)
        {
            isUsingTelephoto = false;
            /*normalCameraRange.SetActive(true);
            normalCameraRangeColliderObject.SetActive(true);
            newCameraRange.SetActive(false);
            newCameraRangeColliderObject.SetActive(false);*/

            normalCameraRange.enabled = true;
            normalCameraRangeColliderObject.enabled = true;
            normalAction.enabled = true;
            newCameraRange.enabled = false;
            newCameraRangeColliderObject.enabled = false;
            newAction.enabled = false;

            batteryOutline.color = Color.white;
        }
    }
}
