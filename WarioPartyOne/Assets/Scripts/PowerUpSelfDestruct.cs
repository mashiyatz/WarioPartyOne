using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSelfDestruct : MonoBehaviour
{
    public void StartSelfDestruct()
    {
        StartCoroutine(PlayAudioThenDestroy());
    }

    IEnumerator PlayAudioThenDestroy()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Battery"))
        {
            if (collision.gameObject.CompareTag("Paparazzi"))
            {
                if (!collision.GetComponent<StanItems>().CheckIfUsingTelephoto())
                {
                    if (collision.GetComponent<PlayerManager>().resources < 3)
                    {
                        collision.GetComponent<PlayerManager>().UpdateResource(2);
                    }
                    StartCoroutine(PlayAudioThenDestroy());
                }
            }
        }

        if (gameObject.CompareTag("Speed"))
        {
            if (collision.gameObject.CompareTag("Paparazzi") || collision.gameObject.CompareTag("Celebrity")) {
                collision.GetComponent<PlayerManager>().StartSpeedUp();
                StartCoroutine(PlayAudioThenDestroy());
            }
        }

        if (gameObject.CompareTag("Disguise"))
        {
            if (collision.gameObject.CompareTag("Celebrity"))
            {
                collision.GetComponent<CelebItems>().isHoldingItem = true;
                StartCoroutine(PlayAudioThenDestroy());
            }
        }

        if (gameObject.CompareTag("CameraUpgrade"))
        {
            if (collision.gameObject.CompareTag("Paparazzi"))
            {
                collision.GetComponent<StanItems>().isHoldingItem = true;
                StartCoroutine(PlayAudioThenDestroy());
            }
        }
    }
}
