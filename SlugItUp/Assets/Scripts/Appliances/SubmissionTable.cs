using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionTable : ApplianceController
{
    public float FRICTION_CONSTANT;
    public float Z_CONSTANT;
    public static Slug requiredSlug;
    public PlayerController player;

    public Sprite emptySprite;
    public Sprite fullSprite;
    
    // Label variables
    public GameObject label;
    public GameObject slugBackground;
    public GameObject bigSlug;
    public GameObject medSlug;
    public GameObject smlSlug;
    public GameObject water;

    private bool locked;
    private bool correct;
    private Slug heldSlug;

    private Rigidbody2D rb;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();

        requiredSlug = ListGenerator.getRandomSlug();

        locked = false;

        correct = false;

        smlSlug.SetActive(requiredSlug.getSize() == 1);
        medSlug.SetActive(requiredSlug.getSize() == 2);
        bigSlug.SetActive(requiredSlug.getSize() == 3);

        Color slugColor = Slug.getColorFromType(requiredSlug.getType());

        GameObject slugToUse = (requiredSlug.getSize() == 1 ? smlSlug : (requiredSlug.getSize() == 2 ? medSlug : (requiredSlug.getSize() == 3 ? bigSlug : null)));
        slugToUse.GetComponent<SpriteRenderer>().color = slugColor;
        
        slugBackground.GetComponent<SpriteRenderer>().color = new Vector4(slugColor.r, slugColor.g, slugColor.b, 0.8f);

        water.SetActive(!requiredSlug.getIsDry());

        label.SetActive(false);

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
        if (collision.CompareTag("submissionZone") && heldSlug != null && !locked)
        {
            if (correct)
                player.addScore(heldSlug.getScoreAddition());
            else
                player.addScore(-1);
            locked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("submissionZone") && heldSlug != null && locked)
        {
            if (correct)
                player.addScore(heldSlug.getScoreAddition());
            else
                player.addScore(1);
            locked = false;
        }
    }

    public override bool insertSlug(Slug slug) 
    {
        if (heldSlug == null) 
        {
            bool hasSameType = (requiredSlug.getType() == slug.getType());
            bool hasSameSize = (requiredSlug.getSize() == slug.getSize());
            bool hasSameWetness = (requiredSlug.getIsDry() == slug.getIsDry());

            heldSlug = slug;

            gameObject.GetComponent<SpriteRenderer>().sprite = fullSprite;

            if (hasSameType && hasSameSize && hasSameWetness) 
            {
                correct = true;
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

        return false;
    }

    public override Slug getSlug() 
    {
        if (!locked)
        {
            correct = false;

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

    public bool isInsideSubmissionZone() {
        return locked;
    }

}
