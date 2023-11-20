using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugWaterController : MonoBehaviour
{
    public float EXIST_TIME;
    public float speed;

    private float last_time;

    // Start is called before the first frame update
    void Start()
    {
        last_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - last_time > EXIST_TIME) 
        {
            Destroy(gameObject);
            return;
        }
        
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed * ((Time.time - last_time) - EXIST_TIME));
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        color.a = 1 - (Time.time - last_time) / EXIST_TIME;
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
