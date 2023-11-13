using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedingController : MonoBehaviour
{
    public float timer;
    public GameObject slug;

    private Slug heldSlug1;
    private Slug heldSlug2;
    private float lastTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(heldSlug2 != null)
        {
            if (Time.time - lastTime >= timer)
            {
                Slug newSlug = new Slug(heldSlug1.getColor1(), heldSlug2.getColor2(), 1, true);
                GameObject s = Instantiate(slug);
                s.GetComponent<SlugController>().setSlugType(newSlug);
                s.GetComponent<Rigidbody2D>().velocity = new Vector2();
            }
        }
    }
    
    public void insertSlug(Slug slug)
    {
        if (heldSlug1 == null)
            heldSlug1 = slug;
        else
        {
            heldSlug1 = slug;
            lastTime = Time.time;
        }
    }
}
