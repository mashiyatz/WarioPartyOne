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

    private float reloadTime = 2.5f;
    private bool isReloading = false;

    private float activationTime = 3f;

    private void Start()
    {
        gmScript = FindObjectOfType<GameManagerScript>();
    }

    IEnumerator Reload()
    {
        pm.slider.gameObject.SetActive(true);
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

    private void Update()
    {

        if (transform.parent.CompareTag("Paparazzi"))
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

        }
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
