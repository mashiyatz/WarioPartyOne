using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public PlayerManager pm;
    private bool inRange = false;
    private GameObject target;

    IEnumerator Reload()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(3.0f);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(pm.actionKey))
        {
            if (transform.parent.CompareTag("Paparazzi") && inRange)
            {
                if (target.CompareTag("Battery"))
                {
                    if (pm.resources < 3) pm.UpdateResource(2);
                    Destroy(target);
                }
                else if (target.CompareTag("Celebrity") && pm.resources >= 1)
                {
                    pm.UpdateScore();
                    pm.UpdateResource(-1);
                    StartCoroutine(Reload());
                }
            } else if (transform.parent.CompareTag("Paparazzi") && !inRange && pm.resources >= 1)
            {
                pm.UpdateResource(-1);
            }

            if (transform.parent.CompareTag("Celebrity") && inRange)
            {
                if (target.CompareTag("Goal"))
                {
                    pm.UpdateScore();
                    Destroy(target);
                }
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

        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Battery"))
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

        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Battery"))
        {
            inRange = false;
            target = null;
        }
    }
}
