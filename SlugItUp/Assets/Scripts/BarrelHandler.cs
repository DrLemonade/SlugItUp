using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelHandler : MonoBehaviour
{
    public GameObject newestBarrel;
    public GameObject player;

    void Start() 
    {
        newestBarrel = Instantiate(newestBarrel);
        newestBarrel.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        newestBarrel.GetComponentInParent<SubmissionTable>().player = player.GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (newestBarrel.GetComponentInParent<SubmissionTable>().isInsideSubmissionZone()) {
            newestBarrel = Instantiate(newestBarrel);
            newestBarrel.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            newestBarrel.GetComponentInParent<SubmissionTable>().player = player.GetComponentInParent<PlayerController>();
        }
    }
}
