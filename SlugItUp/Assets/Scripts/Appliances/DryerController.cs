using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerController : MonoBehaviour
{

    public float timer;

    private float startTime;
    private Slug slug;

    public void insertSlug(Slug slug) // Inserts slug when collides with Breeder
    {
        this.slug = new Slug(slug.getType(), slug.getSize(), true);
        startTime = Time.time;
    }

    public Slug getSlug() {
        if (slug != null && (Time.time - startTime >= timer)) {
            return slug;
        } else {
            return null;
        }
    }

    public void empty() {
        if (slug != null && (Time.time - startTime >= timer)) {
            slug = null;
        }
    }

}
