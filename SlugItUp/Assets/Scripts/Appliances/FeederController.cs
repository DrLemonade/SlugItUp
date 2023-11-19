using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeederController : ApplianceController
{

    private Slug heldSlug;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    void Update() 
    {
        if (producedSlug == null && heldSlug != null && isTimeFinished()) 
        {
            producedSlug = new Slug(heldSlug.getType(), (heldSlug.getSize() < 3 ? heldSlug.getSize() + 1 : 3), heldSlug.getIsDry(), heldSlug.getScoreAddition());
            producedSlug.addScoreAddition();
            heldSlug = null;

            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
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
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;

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
