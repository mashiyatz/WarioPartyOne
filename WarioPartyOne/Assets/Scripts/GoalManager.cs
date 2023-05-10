using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public GameObject goalPrefab;
    public bool canCollectGoal;
    public Zones zones;
    private Coroutine co;
    private GameObject goalObject;

    void Start()
    {
        canCollectGoal = true;
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            zones.DeactivateZone();
            GenerateNewGoal();
        }
    }

    void GenerateNewGoal()
    {
        if (co != null) StopCoroutine(co);
        Vector3 pos = zones.ActivateZone();
        goalObject = Instantiate(goalPrefab, pos, Quaternion.identity, transform);
        co = StartCoroutine(DeactivateGoal(goalObject));
    }

    IEnumerator DeactivateGoal(GameObject goalObject)
    {
        yield return new WaitForSeconds(16.0f);
        if (goalObject != null)
        {
            zones.DeactivateZone();
            Destroy(goalObject);
        }
    }

    public void StartSelfDestruct()
    {
        canCollectGoal = false;
        if (co != null) StopCoroutine(co);
        StartCoroutine(GoalSelfDestruct());
    }

    IEnumerator GoalSelfDestruct()
    {
        zones.DeactivateZone();
        goalObject.GetComponent<SpriteRenderer>().enabled = false;
        goalObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2.0f);
        Destroy(transform.GetChild(0).gameObject);
        canCollectGoal = true;
    }
}
