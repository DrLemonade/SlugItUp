using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionTable : MonoBehaviour
{
    public static int submissionCounter;
    public static Slug[] submission;

    private int score;

    void Start() {
        submissionCounter = 0;
        submission = ListGenerator.generateSlugList(10);

        score = 0;

        foreach (Slug s in submission) {
            Debug.Log("One of the wanted slugs: " + s.toString());
        }
    }

    public void insertSlug(Slug slug) {
        Slug requiredSlug = submission[submissionCounter];

        bool hasSameType = (requiredSlug.getType() == slug.getType());
        bool hasSameSize = (requiredSlug.getSize() == slug.getSize());
        bool hasSameWetness = (requiredSlug.getIsDry() == slug.getIsDry());

        if (hasSameType && hasSameSize && hasSameWetness) {
            submissionCounter++;

            score += slug.getScoreAddition();

            Debug.Log("This was correct!");
            Debug.Log("You gave me: " + slug.toString() + "!");
        } else {
            Debug.Log("This was wrong!");
            Debug.Log("You gave me: " + slug.toString());
            Debug.Log("But I wanted: " + requiredSlug.toString());
            // The person who submitted this slug is stupid and should die
        }
    }

}
