using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ApplianceController : MonoBehaviour
{

    // *** Public variables ***

    // How long it takes this appliance to do its work.
    public float applianceTimer;

    // *** Privates protected variables ***

    // The time when this appliance began doing its thing.
    private protected float startTime;

    // The slug that is produces when this appliance finishes its thing.
    private protected Slug producedSlug;
    
    // *** abstract methods ***

    // Inserts a slug into the current appliance. Returns true if the 
    // insert was successful, otherwise false.
    public abstract bool insertSlug(Slug slug);

    // Returns the slug that was produces if producesSlug isn't null, 
    // otherwise returns a slug that is in the appliance but hasn't 
    // finished processing, otherwise returns null if there are no
    // slugs at all in this appliance.
    public abstract Slug getSlug();

    // *** Instance methods ***

    // Returns true if this appliance has started its timer and the
    // timer is done, otherwise flase.
    private protected bool isTimeFinished() {
        return (startTime != 0) && (Time.time - startTime >= applianceTimer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slug"))
        {
            bool success = insertSlug(collision.GetComponentInParent<SlugController>().getSlug());
            if (success)
                Destroy(collision.gameObject);
        }
    }

}
