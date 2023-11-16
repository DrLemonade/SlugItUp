using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionTable : MonoBehaviour
{
    
    public static Slug[] submission;

    public void start() {
        submission = ListGenerator.generateSlugList(10);
    }

}
