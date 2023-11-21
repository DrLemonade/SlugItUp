using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drippy : MonoBehaviour
{

    public float dropDeltaTime = 0.25f;
    public GameObject waterDropPrefab;

    private float timeAtLastSpawn;
    private bool doDripping;

    // Start is called before the first frame update
    void Start()
    {
        timeAtLastSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (doDripping)
        {
            if (Time.time - timeAtLastSpawn > dropDeltaTime)
            {
                GameObject drop = Instantiate(waterDropPrefab, transform);
                drop.transform.localPosition = new Vector3(UnityEngine.Random.Range(-drop.transform.localScale.x * 2, drop.transform.localScale.x * 2), drop.transform.localScale.y, -0.001f);
                timeAtLastSpawn = Time.time;
            }
        }
    }

    public void setDoDripping(bool val)
    {
        doDripping = val;
    }

}
