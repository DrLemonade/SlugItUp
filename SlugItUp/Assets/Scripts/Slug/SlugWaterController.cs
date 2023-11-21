using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugWaterController : MonoBehaviour
{
    public float existTime;
    public float speed;

    private float timeSinceLastDrop;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastDrop = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy self if existTime has passed
        if (Time.time - timeSinceLastDrop > existTime) 
        {
            Destroy(gameObject);
            return;
        }
        
        // Make drop slower over time
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed * ((Time.time - timeSinceLastDrop) - existTime));

        // Make drop more transparent over time
        Color color = gameObject.GetComponent<SpriteRenderer>().color;
        color.a = 1 - (Time.time - timeSinceLastDrop) / existTime;
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
