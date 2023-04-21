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
}
