using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingController : ApplianceController
{

    // Instance variables
    public float SCALE_CONSTANT;
    public Slug heldSlug1;
    public Slug heldSlug2;
    public Sprite fullSprite;
    public Sprite emptySprite;

    private float orgScale;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        orgScale = transform.localScale.x;

        heldSlug1 = null;
        heldSlug2 = null;
        producedSlug = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (producedSlug == null && isTimeFinished())
        {
            producedSlug = new Slug(Slug.getMixedType(heldSlug1.getType(), heldSlug2.getType()), 1, false, heldSlug1.getScoreAddition());
            producedSlug.addScoreAddition();
            transform.localScale = new Vector2(0.1564078f, 0.1564078f);
            heldSlug1 = null;
            heldSlug2 = null;
        }

        if (producedSlug == null && !isTimeFinished() && heldSlug2 != null)
        {
            transform.localScale = new Vector2(orgScale - orgScale * SCALE_CONSTANT * Mathf.Sin(Time.time) * Mathf.Sin(Time.time), orgScale - orgScale * SCALE_CONSTANT * Mathf.Cos(Time.time) * Mathf.Cos(Time.time));
        }
    }
    
    public override bool insertSlug(Slug slug) // Inserts slug when collides with Breeder
    {
        if (producedSlug == null)
        {
            if (heldSlug1 == null) 
            {
                heldSlug1 = slug;
                return true;
            }
            else if (heldSlug2 == null)
            {
                heldSlug2 = slug;
                startTime = Time.time;
                gameObject.GetComponent<SpriteRenderer>().sprite = fullSprite;
                return true;
            }
        }

        return false;
    }

    public override Slug getSlug() // For the record, this way of styling the if-statements makes me wanna puke.
    {
        if (producedSlug != null) 
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;

            startTime = 0;

            Slug temp = producedSlug;
            producedSlug = null;
            return temp;
        }
        else
        {
            if (heldSlug2 != null) 
            {
                startTime = 0;

                Slug temp = heldSlug2;
                heldSlug2 = null;
                return temp;
            }
            else
            {
                if (heldSlug1 != null)
                {
                    startTime = 0;

                    Slug temp = heldSlug1;
                    heldSlug1 = null;
                    return temp;
                }
                else
                {
                    return null;
                }
            }
        }
    }

}
