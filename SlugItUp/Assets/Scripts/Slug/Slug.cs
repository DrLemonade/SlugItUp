using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug
{
    // Constants
    public static readonly int RED = 0b001;
    public static readonly int GREEN = 0b010;
    public static readonly int BLUE = 0b100;

    // Instance variables
    private int type;
    private int size;
    private bool isDry;
    private int scoreAddition;

    // Creates a new slug
    public Slug(int type, int size, bool isDry, int scoreAddition)
    {
        this.type = type;
        this.size = size;
        this.isDry = isDry;
        this.scoreAddition = scoreAddition;
    }

    // Returns this instances type variable value
    public int getType() 
    {
        return type;
    }

    // Returns this instances size variable value
    public int getSize() 
    {
        return size;
    }

    // Returns this instances isDry variable value
    public bool getIsDry() 
    {
        return isDry;
    }

    // Returns the color mixing result of two colors
    public static int getMixedType(int type1, int type2) 
    {
        return (type1 | type2);
    }

    // Returns the string value of the name of a color type
    public static string getTypeName(int type) 
    {
        string name = "none";

        switch (type)
        {
            case 0:
                name = "white";
                break;
            case 1:
                name = "red";
                break;
            case 2:
                name = "green";
                break;
            case 3:
                name = "yellow";
                break;
            case 4:
                name = "blue";
                break;
            case 5:
                name = "magenta";
                break;
            case 6:
                name = "cyan";
                break;
            case 7:
                name = "black";
                break;
        }

        return name;
    }

    public static Color getColorFromType(int type) 
    {
        Color color = Color.white;

        switch (type)
        {
            case 0:
                color = Color.white;
                break;
            case 1:
                color = Color.red;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.yellow;
                break;
            case 4:
                color = Color.blue;
                break;
            case 5:
                color = Color.magenta;
                break;
            case 6:
                color = Color.cyan;
                break;
            case 7:
                color = Color.black;
                break;
        }

        return color;
    }

    // Return the string value of the name of the provided slug
    public static string getSlugName(Slug slug)
    {
        return getTypeName(slug.getType()) + " slug";
    }

    public static string getSlugFullName(Slug slug)
    {
        return getTypeName(slug.getType()) + " " + slug.getSize() + " length " + (slug.getIsDry() ? "dry" : "wet") + " slug";
    }

    public string toString()
    {
        return "(" + type + ", " + size + ", " + (isDry ? 0 : 1) + ")";
    }

    public void addScoreAddition()
    {
        scoreAddition++;
    }

    public int getScoreAddition()
    {
        return scoreAddition;
    }
}
