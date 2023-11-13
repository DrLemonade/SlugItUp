using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : MonoBehaviour
{
    public float frictionConstant;
    public Rigidbody2D rb;
    public PlayerController player;
    public GameObject slug;

    private bool collectable;
    private Slug slugType;

    // Start is called before the first frame update
    void Start()
    {
        collectable = false;
        slugType = new Slug("red", "red", 1, true);
    }

    // Update is called once per frame
    void Update()
    {
        // Friction
        if(rb.velocity.x > 0) // Friction x pos
        {
            if (rb.velocity.y < frictionConstant)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x - frictionConstant, rb.velocity.y);
        }
        if (rb.velocity.y > 0) // Friction y pos
        {
            if (rb.velocity.y < frictionConstant)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - frictionConstant);
        }
        if (rb.velocity.x < 0) // Friction x neg
        {
            if (rb.velocity.y < frictionConstant)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x + frictionConstant, rb.velocity.y);
        }
        if (rb.velocity.y < 0) // Friction y neg
        {
            if (rb.velocity.y < frictionConstant)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + frictionConstant);
        }

        if (Input.GetKeyDown("e") && collectable && player.heldSlug == null)
        {
            Debug.Log("Collection");
            player.HoldSlug(slugType);
            Destroy(slug);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collector"))
            collectable = true;
        if (collision.gameObject.CompareTag("Breeding"))
        {
            gameObject.GetComponent<BreedingController>().insertSlug(slugType);
            Destroy(slug);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collector"))
            collectable = false;
    }

    public void setSlugType(Slug s)
    {
        slugType = s;
    }
}

public class Slug
{
    private string color1;
    private string color2;
    private int size;
    private bool wet;

    public Slug(string c1, string c2, int s, bool w)
    {
        color1 = c1;
        color2 = c2;
        size = s;
        wet = w;
    }

    public string getColor1() { return color1; }

    public string getColor2() { return color2; }
}
