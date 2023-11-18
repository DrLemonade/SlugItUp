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
            slugColor.a = 255;
        else
            slugColor.a = 100;

            GetComponent<SpriteRenderer>().color = slugColor;

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        if (slug.getSize() == 1)
            gameObject.transform.localScale *= 0.15f;
        else if (slug.getSize() == 2)
            gameObject.transform.localScale *= 0.2f;
        else if (slug.getSize() == 3)
            gameObject.transform.localScale *= 1.25f;
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
        
        if (collision.gameObject.CompareTag("Breeding"))
        {
            if (!collision.gameObject.GetComponent<BreedingController>().isFull())
            {
                collision.gameObject.GetComponentInParent<BreedingController>().insertSlug(slug);
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Trash"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Submission"))
        {
            collision.gameObject.GetComponentInParent<SubmissionTable>().insertSlug(slug);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Feeder"))
        {
            collision.gameObject.GetComponentInParent<FeederController>().insertSlug(slug);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Dryer"))
        {
            collision.gameObject.GetComponentInParent<DryerController>().insertSlug(slug);
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