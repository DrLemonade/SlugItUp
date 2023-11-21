using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlugController : MonoBehaviour
{

    // *** Public instance variables ***
    public Sprite slugSpriteL;
    public Sprite slugSpriteR;
    public Sprite slugSpriteF;
    public Sprite slugSpriteB;
    public float friction = 0.85f;

    // *** Private instance variables ***
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Slug slug;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (slug == null)
            slug = new Slug((int) Math.Pow(2, UnityEngine.Random.Range(0, 3)), 1, false, 1);

        spriteRenderer.color = Slug.getColorFromType(slug.getType());

        float slugSize = 0.05f + (slug.getSize() * 0.05f);
        gameObject.transform.localScale = new Vector3(slugSize, slugSize, slugSize);

        int direction = UnityEngine.Random.Range(0, 4);
        if (direction == 0)
            spriteRenderer.sprite = slugSpriteF;
        else if (direction == 1)
            spriteRenderer.sprite = slugSpriteR;
        else if (direction == 2)
            spriteRenderer.sprite = slugSpriteB;
        else if (direction == 3)
            spriteRenderer.sprite = slugSpriteL;

        GetComponent<Drippy>().setDoDripping(!slug.getIsDry());
    }

    private void FixedUpdate()
    {
        // Apply friction and change sprite if moving
        if (rb.velocity.magnitude > 0)
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
            rb.velocity *= friction;
        }
    }

    public void setSlug(Slug s)
    {   
        slug = s;
    }

    public Slug getSlug()
    {
        return slug;
    }
}