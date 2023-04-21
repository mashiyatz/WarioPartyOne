using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryScript : MonoBehaviour
{
    // for some reason, setting the trigger code in on the action button side makes it such that the entire view
    // cone is treated as having the Paparazzi tag, even though it should just be the body.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Paparazzi"))
        {
            if (collision.GetComponent<PlayerManager>().resources < 3) collision.GetComponent<PlayerManager>().UpdateResource(2);
            StartCoroutine(PlayAudioThenDestroy());
        }
    }

    IEnumerator PlayAudioThenDestroy()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
