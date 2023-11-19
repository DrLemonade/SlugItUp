using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelHandler : MonoBehaviour
{
    public GameObject barrelPrefab;
    public GameObject player;

    private GameObject newestBarrel;

    void Start() 
    {
        newestBarrel = Instantiate(barrelPrefab);
        newestBarrel.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        newestBarrel.GetComponentInParent<SubmissionTable>().player = player.GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (newestBarrel.GetComponentInParent<SubmissionTable>().isInsideSubmissionZone()) {
            newestBarrel = Instantiate(barrelPrefab);
            newestBarrel.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            newestBarrel.GetComponentInParent<SubmissionTable>().player = player.GetComponentInParent<PlayerController>();
        }
    }
}
