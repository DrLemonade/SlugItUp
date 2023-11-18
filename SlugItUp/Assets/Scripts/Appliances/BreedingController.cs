using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingController : ApplianceController
{

    // Instance variables
    public Slug heldSlug1;
    public Slug heldSlug2;

    // Start is called before the first frame update
    void Start()
    {
        heldSlug1 = null;
        heldSlug2 = null;
        producedSlug = null;

        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (producedSlug == null && isTimeFinished())
        {
            producedSlug = new Slug(Slug.getMixedType(heldSlug1.getType(), heldSlug2.getType()), 1, false);
            producedSlug.addScoreAddition();
            heldSlug1 = null;
            heldSlug2 = null;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
                return true;
            }
        }

        return false;
    }

    public override Slug getSlug() // For the record, this way of styling the if-statements makes me wanna puke.
    {
        if (producedSlug != null) 
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;

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
