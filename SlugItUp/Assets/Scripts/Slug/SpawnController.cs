using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public float period;
    public GameObject slugPreset;
    public PlayerController player;

    private float lastTime;

    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > period)
        {
            GameObject slug = Instantiate(slugPreset);
            slug.transform.position = new Vector2(Random.Range(-11, 11), Random.Range(-5, 5));
            slug.GetComponent<SlugController>().player = player;
            lastTime = Time.time;
        }
    }
}
