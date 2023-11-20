using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlugController : MonoBehaviour
{

    // TODO: Make sure that classes that use slugSprites use these variables instead of having their own.
    public static Sprite slugSpriteF;
    public static Sprite slugSpriteR;
    public static Sprite slugSpriteB;
    public static Sprite slugSpriteL;

    public float FRICTION_CONSTANT; // TODO: Refactor all uppercased variables to follow normal variable style conventions. Only readonly variables follow this convention
    public PlayerController player; // TODO: Completely remove this variable from this class

    public float DROP_DELTA; // TODO: Create a script that create water drops and remove this
    public GameObject waterDrop; // TODO: Same as line above
    private float lastTime = 0; // TODO: Same as line above

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Slug slug;

    private bool collectable;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        collectable = false;

        if (slug == null) // TODO: Find a more elegant way to do this
            slug = new Slug((int) Math.Pow(2, UnityEngine.Random.Range(0, 3)), 1, false, 1);

        spriteRenderer.color = Slug.getColorFromType(slug.getType());

        float slugSize = 0.05f + (slug.getSize() * 0.05f);
        gameObject.transform.localScale = new Vector3(slugSize, slugSize, slugSize);

        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                spriteRenderer.sprite = slugSpriteF;
                break;
            case 1:
                spriteRenderer.sprite = slugSpriteR;
                break;
            case 2:
                spriteRenderer.sprite = slugSpriteB;
                break;
            case 3:
                spriteRenderer.sprite = slugSpriteL;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player.heldSlug == null)
        {
            if (Input.GetKeyDown("e") && collectable) // TODO: Not entirely sure if this is just me, but is the collectable variable REQUIRED? There must be a simpler way
            {
                player.HoldSlug(slug);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        // Apply friction and change sprite if moving
        if (rb.velocity.magnitude > 0) // TODO: Find what other class uses friction and adopt this method into that one as well
        {
            // Set the z position of this slug based on its y position
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

            // Set which slug sprite to use based on the fastest velocity component
            bool xNotZero = Math.Abs(rb.velocity.x) > 0.1;
            bool yNotZero = Math.Abs(rb.velocity.y) > 0.1;

            bool useXVelocity = (xNotZero && !yNotZero) || ((xNotZero && yNotZero) && Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y));
            bool useYVelocity = (!xNotZero && yNotZero) || ((xNotZero && yNotZero) && Mathf.Abs(rb.velocity.x) < Mathf.Abs(rb.velocity.y));

            if (useXVelocity)
                if (rb.velocity.x > 0)
                    spriteRenderer.sprite = slugSpriteR;
                else
                    spriteRenderer.sprite = slugSpriteL;
            else if (useYVelocity)
                if (rb.velocity.y > 0)
                    spriteRenderer.sprite = slugSpriteB;
                else
                    spriteRenderer.sprite = slugSpriteF;

            // Clamp velocity to max of 50
            int maxMagnitude = 50;
            if (rb.velocity.magnitude > maxMagnitude)
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxMagnitude);

            // Apply friction
            rb.velocity *= FRICTION_CONSTANT;
        }

        // Wet Particals if wet
        if (!slug.getIsDry() && Time.time - lastTime > DROP_DELTA) // TODO: Create a seperate script that handles the creation of water drops. This snipbit of code is used 3 times in 3 different classes!
        {
            GameObject drop = Instantiate(waterDrop, this.transform);
            drop.transform.localPosition = new Vector3(UnityEngine.Random.Range(-drop.transform.localScale.x * 2, drop.transform.localScale.x * 2), drop.transform.localScale.y, -0.001f);
            lastTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: This should be moved to the PlayerController class
        if (collision.gameObject.CompareTag("Collector") && player.getTargetSlug() == null)
        {
            collectable = true;
            player.setTargetSlug(gameObject);
        }

        // TODO: This should be coded into the ApplianceController class and instead use put a Slug tag onto slugs
        bool isBreeder = collision.gameObject.CompareTag("Breeding");
        bool isTrash = collision.gameObject.CompareTag("Trash");
        bool isBarrel = collision.gameObject.CompareTag("Submission");
        bool isFeeder = collision.gameObject.CompareTag("Feeder");
        bool isDryer = collision.gameObject.CompareTag("Dryer");

        if (isBreeder || isBarrel || isFeeder || isDryer || isTrash)
        {
            bool success = collision.gameObject.GetComponentInParent<ApplianceController>().insertSlug(slug);
            if (success)
                Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) // TODO: This should be moved to the PlayerController class
    {
        if (collision.gameObject.CompareTag("Collector") && player.getTargetSlug() == gameObject)
        {
            collectable = false;
            player.setTargetSlug(null);
        }
    }

    public void setSlugType(Slug s) // TODO: Rename to a better name
    {   
        slug = s;
    }
}