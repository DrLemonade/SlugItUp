using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerController : ApplianceController
{
    public float SCALE_CONSTANT;
    public GameObject slugSprite;

    private float orgScale;
    private Slug heldSlug;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        orgScale = transform.localScale.x;
    }

    void Update() 
    {
        if (producedSlug == null && heldSlug != null && isTimeFinished()) 
        {
            producedSlug = new Slug(heldSlug.getType(), heldSlug.getSize(), true, heldSlug.getScoreAddition());
            producedSlug.addScoreAddition();
            transform.localScale = new Vector2(0.1564078f, 0.1564078f);
            heldSlug = null;
        }

        if (producedSlug == null && !isTimeFinished() && heldSlug != null)
        {
            transform.localScale = new Vector2(orgScale - orgScale * SCALE_CONSTANT * Mathf.Sin(Time.time) * Mathf.Sin(Time.time), orgScale - orgScale * SCALE_CONSTANT * Mathf.Cos(Time.time) * Mathf.Cos(Time.time));
        }
    }

    public override bool insertSlug(Slug slug) // Inserts slug when collides with Dryer
    {
        if (heldSlug == null && producedSlug == null) 
        {
            slugSprite.SetActive(true);
            slugSprite.GetComponent<SpriteRenderer>().color = Slug.getColorFromType(slug.getType());
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

            slugSprite.SetActive(false);

            Slug temp = producedSlug;
            producedSlug = null;
            return temp;
        } 
        else 
        {
            if (heldSlug != null)
            {
                slugSprite.SetActive(false);
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
