using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelHandler : MonoBehaviour
{
    public GameObject barrelPrefab;
    public GameObject player;
    public GameObject displayCircle;

    private GameObject newestBarrel;

    void Start() 
    {
        displayCircle.GetComponentInParent<SpriteRenderer>().color = new Vector4(0, 0, 0, 0);
        
        newestBarrel = Instantiate(barrelPrefab, transform);
        newestBarrel.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        newestBarrel.GetComponentInParent<SubmissionTable>().player = player.GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (newestBarrel.GetComponentInParent<SubmissionTable>().isInsideSubmissionZone()) {
            newestBarrel = Instantiate(barrelPrefab, transform);
            newestBarrel.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            newestBarrel.GetComponentInParent<SubmissionTable>().player = player.GetComponentInParent<PlayerController>();
        }
    }
}
