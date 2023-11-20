using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeederController : ApplianceController
{
    public float SCALE_CONSTANT;
    public Sprite fullSprite;
    public Sprite emptySprite;

    private float orgScale;
    private Slug heldSlug;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        
        gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;

        orgScale = transform.localScale.x;
    }

    void Update() 
    {
        if (producedSlug == null && heldSlug != null && isTimeFinished()) 
        {
            producedSlug = new Slug(heldSlug.getType(), (heldSlug.getSize() < 3 ? heldSlug.getSize() + 1 : 3), heldSlug.getIsDry(), heldSlug.getScoreAddition());
            producedSlug.addScoreAddition();
            transform.localScale = new Vector2(0.1564078f, 0.1564078f);
            heldSlug = null;
            
            gameObject.GetComponent<SpriteRenderer>().sprite = fullSprite;
        }

        if (producedSlug == null && !isTimeFinished() && heldSlug != null)
        {
            transform.localScale = new Vector2(orgScale - orgScale * SCALE_CONSTANT * Mathf.Sin(Time.time) * Mathf.Sin(Time.time), orgScale - orgScale * SCALE_CONSTANT * Mathf.Cos(Time.time) * Mathf.Cos(Time.time));
        }
    }

    public override bool insertSlug(Slug slug) // Inserts slug when collides with Feeder
    {
        if (heldSlug == null && producedSlug == null) 
        {
            heldSlug = slug;
            startTime = Time.time;
            return true;
        }

        return false;
    }

    public override Slug getSlug() 
    {
        if (producedSlug != null && isTimeFinished()) 
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;

            startTime = 0;

            Slug temp = producedSlug;
            producedSlug = null;
            return temp;
        } 
        else 
        {
            if (heldSlug != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;
                transform.localScale = new Vector2(0.1564078f, 0.1564078f);
                startTime = 0;

                Slug temp = heldSlug;
                heldSlug = null;
                return temp;
            }
        }

        return null;
    }
    
}
