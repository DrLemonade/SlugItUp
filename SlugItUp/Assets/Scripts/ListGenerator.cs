using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ListGenerator
{
    
    public static Slug[] generateSlugList() 
    {
        return generateSlugList(10);
    }

    public static Slug[] generateSlugList(int listSize) 
    {
        Slug[] list = new Slug[listSize];

        // For each index in list create a slug where it is either red, blue, or green, is a size between 1-3 and has a random isDry value
        for (int i = 0; i < listSize; i++) 
        {
            list[i] = getRandomSlug();
        }

        return list;
    }

    public static Slug getRandomSlug() {
        int type = (int) Math.Pow(2, UnityEngine.Random.Range(0, 3));
        int size = UnityEngine.Random.Range(1, 4);
        bool isDry = (UnityEngine.Random.Range(0, 2) == 0);
        return new Slug(type, size, isDry);
    }

}
