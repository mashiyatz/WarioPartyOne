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
        if (Input.GetKeyDown(pm.actionKey) && inRange)
        {
            pm.UpdateScore();

            if (transform.parent.CompareTag("Paparazzi"))
            {
                StartCoroutine(Reload());
            }

            if (transform.parent.CompareTag("Celebrity"))
            {
                Destroy(target);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Celebrity"))
        {
            inRange = true;
        }

        if (transform.parent.CompareTag("Celebrity") && collision.gameObject.CompareTag("Goal"))
        {
            inRange = true;
            target = collision.gameObject;
        }

/*        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Funds"))
        {
            inRange = true;
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Celebrity"))
        {
            inRange = false;
        }

        if (transform.parent.CompareTag("Celebrity") && collision.gameObject.CompareTag("Goal"))
        {
            inRange = false;
            target = null;
        }

/*        if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Funds"))
        {
            inRange = false;
        }*/
    }


    /*    private void OnTriggerStay2D(Collider2D collision)
        {
            if (transform.parent.CompareTag("Paparazzi") && collision.gameObject.CompareTag("Celebrity"))
            {
                if (Input.GetKeyDown(pm.actionKey))
                {
                    pm.UpdateScore();
                    StartCoroutine(Reload());
                }
            }

            if (transform.parent.CompareTag("Celebrity") && collision.gameObject.CompareTag("Goal"))
            {
                if (Input.GetKeyDown(pm.actionKey))
                {
                    pm.UpdateScore();
                    Destroy(collision.gameObject);
                }
            }
        }*/

}
