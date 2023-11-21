using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public float period;
    public GameObject slugPreset;
    public GameObject spawnSquare;

    private float lastTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnSquare.GetComponentInParent<SpriteRenderer>().color = new Vector4(0, 0, 0, 0);

        GameObject slug = Instantiate(slugPreset, transform);
        slug.transform.position = new Vector2(Random.Range(spawnSquare.transform.position.x - spawnSquare.transform.localScale.x / 2, spawnSquare.transform.position.x + spawnSquare.transform.localScale.x / 2), Random.Range(spawnSquare.transform.position.y - spawnSquare.transform.localScale.y / 2, spawnSquare.transform.position.y + spawnSquare.transform.localScale.y / 2));
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastTime > period)
        {
            GameObject slug = Instantiate(slugPreset, transform);
            slug.transform.position = new Vector2(Random.Range(spawnSquare.transform.position.x - spawnSquare.transform.localScale.x / 2, spawnSquare.transform.position.x + spawnSquare.transform.localScale.x / 2), Random.Range(spawnSquare.transform.position.y - spawnSquare.transform.localScale.y / 2, spawnSquare.transform.position.y + spawnSquare.transform.localScale.y / 2));
            lastTime = Time.time;
        }
    }
}
