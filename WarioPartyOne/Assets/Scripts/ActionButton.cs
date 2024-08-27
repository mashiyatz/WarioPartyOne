using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionButton : MonoBehaviour
{
    public PlayerManager pm;
    private bool inRange = false;
    private GameObject target;
    private GameManagerScript gmScript;
    private float timeOfPress;

    // private float reloadTime;
    private bool isReloading = false;

    private float activationTime;

    private void Awake()
    {
        gmScript = FindObjectOfType<GameManagerScript>();
    }

    private void Start()
    {
        // reloadTime = ValueSettings.cameraReloadTime;
        activationTime = ValueSettings.zoneActivationTime;
    }

    IEnumerator Reload()
    {
        float reloadTime = ValueSettings.cameraReloadTime;

        pm.slider.gameObject.SetActive(true);
        if (pm.gameObject.GetComponent<StanItems>().isUsingTelephoto) {
            reloadTime = ValueSettings.telephotoReloadTime;
        } 
        isReloading = true;

        float timer = 0;
        while (timer <= reloadTime)
        {
            timer += Time.deltaTime;
            pm.slider.value = timer/reloadTime;
            yield return null;
        }
        pm.slider.gameObject.SetActive(false);
        isReloading = false;
    }

    public void PaparazziActionButton(InputAction.CallbackContext context)
    {
        if (gmScript.currentState != GameManagerScript.GameState.PLAY) return;
        if (PlayerInput.FindFirstPairedToDevice(context.control.device) != pm.GetComponent<PlayerInput>()) return;
        if (context.ReadValueAsButton() && !isReloading) 
        {
            if (pm.resources >= 1 && gameObject.GetComponent<Collider2D>().enabled)
            {
                if (inRange && target.GetComponent<CelebItems>().CheckIfVisible())
                {
                    gmScript.FlashCamera(true);
                    pm.UpdateScore();
                    pm.UpdateResource(-1);
                    StartCoroutine(Reload());
                }
                else
                {
                    gmScript.FlashCamera(false);
                    pm.UpdateResource(-1);
                    StartCoroutine(Reload());
                }

                if (pm.resources == 0)
                {
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    IEnumerator StartCountdown()
    {
        while (pm.slider.value < 1)
        {
            pm.slider.value = (Time.time - timeOfPress) / activationTime;

            if (!inRange || !gmScript.goalManager.canCollectGoal) ResetSlider();

            yield return null;
        }

        ResetSlider();
        pm.UpdateScore();
        gmScript.goalManager.StartSelfDestruct();
    }

    public void CelebrityActionButton(InputAction.CallbackContext context)
    {
        if (gmScript.currentState != GameManagerScript.GameState.PLAY) return;
        if (PlayerInput.FindFirstPairedToDevice(context.control.device) != pm.GetComponent<PlayerInput>()) return;
        if (context.started && inRange && gmScript.goalManager.canCollectGoal)
        {
            if (!pm.slider.gameObject.activeSelf)
            {
                timeOfPress = Time.time;
                pm.slider.value = 0;
                pm.slider.gameObject.SetActive(true);
            }

            // how to update
            /*            pm.slider.value = (Time.time - timeOfPress) / activationTime;
                        if (pm.slider.value >= 1)
                        {
                            pm.UpdateScore();
                            gmScript.goalManager.StartSelfDestruct();
                        }*/
            //
            StartCoroutine(nameof(StartCountdown));
        }
        else if (context.canceled)
        {
            ResetSlider();
            StopCoroutine(nameof(StartCountdown));
        }
    }

    private void ResetSlider()
    {
        timeOfPress = Time.time;
        if (pm.slider.gameObject.activeSelf)
        {
            pm.slider.value = 0;
            pm.slider.gameObject.SetActive(false);
        }
    }

    private void Update()
    {

        /*if (transform.parent.CompareTag("Paparazzi"))
        {
            if (Input.GetKeyDown(pm.actionKey) && !isReloading)
            {
                if (pm.resources >= 1 && gameObject.GetComponent<Collider2D>().enabled)
                {
                    if (inRange && target.GetComponent<CelebItems>().CheckIfVisible())
                    {
                        gmScript.FlashCamera(true);
                        pm.UpdateScore();
                        pm.UpdateResource(-1);
                        StartCoroutine(Reload());
                    }
                    else
                    {
                        gmScript.FlashCamera(false);
                        pm.UpdateResource(-1);
                        StartCoroutine(Reload());
                    }

                    if (pm.resources == 0)
                    {
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
        else if (transform.parent.CompareTag("Celebrity"))
        {
            if (Input.GetKey(pm.actionKey) && inRange && gmScript.goalManager.canCollectGoal)
            {
                if (!pm.slider.gameObject.activeSelf)
                {
                    timeOfPress = Time.time;
                    pm.slider.value = 0;
                    pm.slider.gameObject.SetActive(true);
                }

                pm.slider.value = (Time.time - timeOfPress) / activationTime;
                if (pm.slider.value >= 1)
                {
                    pm.UpdateScore();
                    gmScript.goalManager.StartSelfDestruct();
                }
            }
            else
            {
                timeOfPress = Time.time;
                pm.slider.value = 0;
                pm.slider.gameObject.SetActive(false);
            }

        }*/
    }

    public bool CheckIfInRange()
    {
        return inRange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Celebrity"))
        {
            inRange = true;
            target = collision.gameObject;
        }

        if (transform.parent.CompareTag("Celebrity") && collision.gameObject.CompareTag("Goal"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Celebrity"))
        {
            inRange = false;
            target = null;
        }

        if (transform.parent.CompareTag("Celebrity") && collision.gameObject.CompareTag("Goal"))
        {
            inRange = false;
        }
    }
}
