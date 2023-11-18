using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionTable : ApplianceController
{
    public float FRICTION_CONSTANT;
    public float Z_CONSTANT;
    public static Slug requiredSlug;
    public Sprite emptySprite;
    public Sprite fullSprite;
    public PlayerController player;
    public Rigidbody2D rb;

    private bool locked;
    private Slug heldSlug;

    void Start() 
    {
        requiredSlug = ListGenerator.getRandomSlug();
        locked = false;

        Debug.Log("Wanted slug: " + Slug.getSlugFullName(requiredSlug));
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y - Z_CONSTANT);
        // Friction
        if (rb.velocity.x > 0) // Friction x pos and right velocity
        {
            if (rb.velocity.x < FRICTION_CONSTANT)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x - FRICTION_CONSTANT, rb.velocity.y);
        }
        if (rb.velocity.y > 0) // Friction y pos and backward velocity
        {
            if (rb.velocity.y < FRICTION_CONSTANT)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - FRICTION_CONSTANT);
        }
        if (rb.velocity.x < 0) // Friction x neg and left velocity
        {
            if (rb.velocity.x > FRICTION_CONSTANT)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x + FRICTION_CONSTANT, rb.velocity.y);
        }
        if (rb.velocity.y < 0) // Friction y neg and forward velocity
        {
            if (rb.velocity.y > FRICTION_CONSTANT)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + FRICTION_CONSTANT);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("submissionZone") && heldSlug != null)
        {
            player.addScore(heldSlug.getScoreAddition());
            locked = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("submissionZone") && heldSlug != null)
        {
            player.addScore(-heldSlug.getScoreAddition());
            locked = false;
        }
    }

    public override bool insertSlug(Slug slug) 
    {
        bool hasSameType = (requiredSlug.getType() == slug.getType());
        bool hasSameSize = (requiredSlug.getSize() == slug.getSize());
        bool hasSameWetness = (requiredSlug.getIsDry() == slug.getIsDry());

        heldSlug = slug;

        gameObject.GetComponent<SpriteRenderer>().sprite = fullSprite;

        if (hasSameType && hasSameSize && hasSameWetness) 
        {
            Debug.Log("This was correct!");
            Debug.Log("You gave me: " + Slug.getSlugFullName(slug) + "!");
        } 
        else 
        {
            Debug.Log("This was wrong!");
            Debug.Log("You gave me: " + Slug.getSlugFullName(slug));
            Debug.Log("But I wanted: " + Slug.getSlugFullName(requiredSlug));
        }

        return true;
    }

    public override Slug getSlug() 
    {
        if (!locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;
            if (heldSlug != null)
            {
                Slug temp = heldSlug;
                heldSlug = null;
                return temp;
            }
        }
        return null;
    }

}
