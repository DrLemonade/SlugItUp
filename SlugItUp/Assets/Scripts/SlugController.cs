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
        slugType = new Slug(Slug.RED, 1, true);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.heldSlug == null)
        {
            if (Input.GetKeyDown("e") && collectable)
            {
                player.HoldSlug(slugType);
                Destroy(slug);
            }
        }
    }

    private void FixedUpdate()
    {
        // Friction
        if (rb.velocity.x > 0) // Friction x pos
        {
            if (rb.velocity.x < frictionConstant)
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
            if (rb.velocity.x > frictionConstant)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x + frictionConstant, rb.velocity.y);
        }
        if (rb.velocity.y < 0) // Friction y neg
        {
            if (rb.velocity.y > frictionConstant)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + frictionConstant);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collector") && player.getTargetSlug() == null)
        {
            collectable = true;
            player.setTargetSlug(slug);
        }
        
        if (collision.gameObject.CompareTag("Breeding"))
        {
            collision.gameObject.GetComponentInParent<BreedingController>().insertSlug(slugType);
            Destroy(slug);
        }
        if (collision.gameObject.CompareTag("Trash"))
        {
            Destroy(slug);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collector") && player.getTargetSlug() == slug)
        {
            collectable = false;
            player.setTargetSlug(null);
        }
    }

    public void setSlugType(Slug s)
    {
        slugType = s;
    }
}