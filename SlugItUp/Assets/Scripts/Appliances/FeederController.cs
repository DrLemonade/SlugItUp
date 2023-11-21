using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeederController : ApplianceController
{
    public float scaleAmount;
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
            heldSlug = null;

            transform.localScale = new Vector3(orgScale, orgScale, transform.localScale.z);
            
            gameObject.GetComponent<SpriteRenderer>().sprite = fullSprite;
        }

        if (producedSlug == null && !isTimeFinished() && heldSlug != null)
        {
            transform.localScale = new Vector3(orgScale - orgScale * scaleAmount * Mathf.Sin(Time.time) * Mathf.Sin(Time.time), orgScale - orgScale * scaleAmount * Mathf.Cos(Time.time) * Mathf.Cos(Time.time), transform.localScale.z);
        }
    }

    public override bool insertSlug(Slug slug)
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
            startTime = 0;

            gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;

            Slug temp = producedSlug;
            producedSlug = null;
            return temp;
        } 
        else 
        {
            if (heldSlug != null)
            {
                startTime = 0;

                gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;
                transform.localScale = new Vector3(orgScale, orgScale, transform.localScale.z);

                Slug temp = heldSlug;
                heldSlug = null;
                return temp;
            }
        }

        return null;
    }
    
}
