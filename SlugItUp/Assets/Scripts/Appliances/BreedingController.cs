using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingController : ApplianceController
{

    // Instance variables
    public float scaleAmount;
    public Sprite fullSprite;
    public Sprite emptySprite;
    public GameObject displayColor1;
    public GameObject displayColor2;
    public GameObject displayColor3;

    private Slug heldSlug1;
    private Slug heldSlug2;
    private float orgScale;
    
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        orgScale = transform.localScale.x;

        heldSlug1 = null;
        heldSlug2 = null;
        producedSlug = null;
    }

    void Update()
    {
        if (producedSlug == null && isTimeFinished())
        {
            heldSlug1 = null;
            heldSlug2 = null;

            producedSlug = new Slug(Slug.getMixedType(heldSlug1.getType(), heldSlug2.getType()), 1, false, heldSlug1.getScoreAddition());
            producedSlug.addScoreAddition();
            
            transform.localScale = new Vector3(orgScale, orgScale, transform.localScale.z);
        
            gameObject.GetComponent<SpriteRenderer>().sprite = fullSprite;

            displayColor1.SetActive(false);
            displayColor2.SetActive(false);
            displayColor3.SetActive(true);
            displayColor3.GetComponentInParent<SpriteRenderer>().color = Slug.getColorFromType(producedSlug.getType());
        }

        if (producedSlug == null && !isTimeFinished() && heldSlug2 != null)
        {
            transform.localScale = new Vector3(orgScale - orgScale * scaleAmount * Mathf.Sin(Time.time) * Mathf.Sin(Time.time), orgScale - orgScale * scaleAmount * Mathf.Cos(Time.time) * Mathf.Cos(Time.time), transform.localScale.z);
        }
    }
    
    public override bool insertSlug(Slug slug) // Inserts slug when collides with Breeder
    {
        if (producedSlug == null)
        {
            if (heldSlug1 == null) 
            {
                heldSlug1 = slug;
                displayColor1.SetActive(true);

                displayColor1.GetComponentInParent<SpriteRenderer>().color = Slug.getColorFromType(heldSlug1.getType());

                return true;
            }
            else if (heldSlug2 == null)
            {
                heldSlug2 = slug;
                displayColor2.SetActive(true);

                displayColor2.GetComponentInParent<SpriteRenderer>().color = Slug.getColorFromType(heldSlug2.getType());

                startTime = Time.time;
                return true;
            }
        }

        return false;
    }

    public override Slug getSlug()
    {
        if (producedSlug != null) 
        {
            startTime = 0;

            gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;

            displayColor1.SetActive(false);
            displayColor2.SetActive(false);
            displayColor3.SetActive(false);

            Slug temp = producedSlug;
            producedSlug = null;
            return temp;
        }
        else
        {
            if (heldSlug2 != null) 
            {
                startTime = 0;

                gameObject.GetComponent<SpriteRenderer>().sprite = emptySprite;

                displayColor2.SetActive(false);

                transform.localScale = new Vector3(orgScale, orgScale, transform.localScale.z);

                Slug temp = heldSlug2;
                heldSlug2 = null;
                return temp;
            }
            else
            {
                if (heldSlug1 != null)
                {
                    startTime = 0;
                    
                    displayColor1.SetActive(false);

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
