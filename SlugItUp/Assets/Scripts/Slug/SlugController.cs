using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlugController : MonoBehaviour
{
    public float frictionConstant;
    public Rigidbody2D rb;
    public PlayerController player;
    public SpriteRenderer spriteRenderer;
    public Sprite slugSpriteF;
    public Sprite slugSpriteR;
    public Sprite slugSpriteB;
    public Sprite slugSpriteL;

    private bool collectable;
    private Slug slug;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = slugSpriteF;

        collectable = false;

        if (slug == null)
            slug = new Slug((int) Math.Pow(2, UnityEngine.Random.Range(0, 3)), 1, false);

        Color slugColor = Slug.getColorFromType(slug.getType());

        if (slug.getIsDry())
            slugColor.a = 1f;
        else
            slugColor.a = 200f / 255f;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        sprite.color = slugColor;

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localScale *= 0.05f + (slug.getSize() * 0.05f);

        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                sprite.sprite = slugSpriteF;
                break;
            case 1:
                sprite.sprite = slugSpriteR;
                break;
            case 2:
                sprite.sprite = slugSpriteB;
                break;
            case 3:
                sprite.sprite = slugSpriteL;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player.heldSlug == null)
        {
            if (Input.GetKeyDown("e") && collectable)
            {
                player.HoldSlug(slug);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // Friction
        if (rb.velocity.x > 0) // Friction x pos and right velocity
        {
            spriteRenderer.sprite = slugSpriteR;
            if (rb.velocity.x < frictionConstant)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x - frictionConstant, rb.velocity.y);
        }
        if (rb.velocity.y > 0) // Friction y pos and backward velocity
        {
            spriteRenderer.sprite = slugSpriteB;
            if (rb.velocity.y < frictionConstant)
                rb.velocity = new Vector2(rb.velocity.x, 0);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - frictionConstant);
        }
        if (rb.velocity.x < 0) // Friction x neg and left velocity
        {
            spriteRenderer.sprite = slugSpriteL;
            if (rb.velocity.x > frictionConstant)
                rb.velocity = new Vector2(0, rb.velocity.y);
            else
                rb.velocity = new Vector2(rb.velocity.x + frictionConstant, rb.velocity.y);
        }
        if (rb.velocity.y < 0) // Friction y neg and forward velocity
        {
            spriteRenderer.sprite = slugSpriteL;
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
            player.setTargetSlug(gameObject);
        }

        bool isBreeder = collision.gameObject.CompareTag("Breeding");
        bool isTrash = collision.gameObject.CompareTag("Trash");
        bool isBarrel = collision.gameObject.CompareTag("Submission");
        bool isFeeder = collision.gameObject.CompareTag("Feeder");
        bool isDryer = collision.gameObject.CompareTag("Dryer");
        
        if (isBreeder || isTrash || isBarrel || isFeeder || isDryer)
        {
            bool success = collision.gameObject.GetComponentInParent<ApplianceController>().insertSlug(slug);
            if (success)
                Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collector") && player.getTargetSlug() == gameObject)
        {
            collectable = false;
            player.setTargetSlug(null);
        }
    }

    public void setSlugType(Slug s)
    {   
        slug = s;
    }
}