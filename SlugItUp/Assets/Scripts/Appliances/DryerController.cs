using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerController : ApplianceController
{

    private Slug heldSlug;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    void Update() 
    {
        if (producedSlug == null && heldSlug != null && isTimeFinished()) 
        {
            producedSlug = new Slug(heldSlug.getType(), heldSlug.getSize(), true);
            producedSlug.addScoreAddition();
            heldSlug = null;

            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public override bool insertSlug(Slug slug) // Inserts slug when collides with Dryer
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
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;

            startTime = 0;

            Slug temp = producedSlug;
            producedSlug = null;
            return temp;
        } 
        else 
        {
            if (heldSlug != null)
            {
                startTime = 0;

                Slug temp = heldSlug;
                heldSlug = null;
                return temp;
            }
        }

        return null;
    }

}
