using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingController : MonoBehaviour
{
    public float timer;
    public float initXVelocity;
    public float initYVelocity;
    public GameObject slug;
    public GameObject player;

    private float lastTime;
    private Slug heldSlug1;
    private Slug heldSlug2;
    private Slug newSlug;

    // Start is called before the first frame update
    void Start()
    {
        heldSlug1 = null;
        heldSlug2 = null;
        newSlug = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(heldSlug2 != null) // Takes (timer) seconds to finish breeding
        {
            if (Time.time - lastTime >= timer)
            {
                newSlug = new Slug(Slug.getMixedType(heldSlug1.getType(), heldSlug2.getType()), 1, false);
                newSlug.addScoreAddition();
                heldSlug1 = null;
                heldSlug2 = null;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
    
    public void insertSlug(Slug slug) // Inserts slug when collides with Breeder
    {
        if (heldSlug1 == null)
            heldSlug1 = slug;
        else if (heldSlug2 == null)
        {
            heldSlug2 = slug;
            lastTime = Time.time;
        }
    }

    public Slug getNewSlug() { return newSlug; } // Returns new slug

    public void setNewSlug() { newSlug = null; } // Gets Breeding ready for next two slugs
}
