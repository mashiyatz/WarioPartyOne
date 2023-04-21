using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public PlayerManager pm;
    private bool inRange = false;
    private GameObject target;
    private GameManagerScript gmScript;
    private float timeOfPress;

    private float reloadTime = 2f;
    private float activationTime = 1f;

    private void Start()
    {
        gmScript = FindObjectOfType<GameManagerScript>();
    }

    IEnumerator Reload()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        pm.slider.gameObject.SetActive(true);

        float timer = 0;
        while (timer <= reloadTime)
        {
            timer += Time.deltaTime;
            pm.slider.value = timer/reloadTime;
            yield return null;
        }
        pm.slider.gameObject.SetActive(false);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    private void Update()
    {

        if (transform.parent.CompareTag("Paparazzi"))
        {
            if (Input.GetKeyDown(pm.actionKey))
            {

                if (pm.resources >= 1 && gameObject.GetComponent<Collider2D>().enabled)
                {
                    gmScript.FlashCamera();
                    if (inRange)
                    {
                        if (target.CompareTag("Celebrity") && pm.resources >= 1)
                        {
                            pm.UpdateScore();
                            pm.UpdateResource(-1);
                            StartCoroutine(Reload());
                        }
                    }
                    else
                    {
                        pm.UpdateResource(-1);
                        StartCoroutine(Reload());
                    }
                }
            }
        }
        else if (transform.parent.CompareTag("Celebrity"))
        {
            if (Input.GetKeyDown(pm.actionKey) && inRange)
            {
                timeOfPress = Time.time;
                pm.slider.value = 0;
                pm.slider.gameObject.SetActive(true);
            }
            else if (Input.GetKey(pm.actionKey) && inRange)
            {
                pm.slider.value = (Time.time - timeOfPress) / activationTime;
                if (pm.slider.value >= 1)
                {
                    pm.UpdateScore();
                    // Destroy(target);
                    target.GetComponent<PowerUpSelfDestruct>().StartSelfDestruct();
                }
            }
            else
            {
                timeOfPress = Time.time;
                pm.slider.value = 0;
                pm.slider.gameObject.SetActive(false);
            }

        }
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
            target = collision.gameObject;
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
            target = null;
        }
    }
}
