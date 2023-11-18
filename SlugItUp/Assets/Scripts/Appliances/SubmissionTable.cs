using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionTable : ApplianceController
{

    public static Slug requiredSlug;

    private int score;

    void Start() 
    {
        requiredSlug = ListGenerator.getRandomSlug();

        score = 0;

        Debug.Log("Wanted slug: " + Slug.getSlugFullName(requiredSlug));
    }

    public override bool insertSlug(Slug slug) 
    {
        bool hasSameType = (requiredSlug.getType() == slug.getType());
        bool hasSameSize = (requiredSlug.getSize() == slug.getSize());
        bool hasSameWetness = (requiredSlug.getIsDry() == slug.getIsDry());

        if (hasSameType && hasSameSize && hasSameWetness) 
        {
            requiredSlug = ListGenerator.getRandomSlug();

            score += slug.getScoreAddition();

            Debug.Log("This was correct!");
            Debug.Log("You gave me: " + Slug.getSlugFullName(slug) + "!");
            Debug.Log("Now I want: " + Slug.getSlugFullName(requiredSlug) + "!");
        } 
        else 
        {
            Debug.Log("This was wrong!");
            Debug.Log("You gave me: " + Slug.getSlugFullName(slug));
            Debug.Log("But I wanted: " + Slug.getSlugFullName(requiredSlug));
        }

        return true;
    }

    public override Slug getSlug() 
    {
        return null;
    }

}
