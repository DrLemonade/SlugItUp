using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) 
        {
            child.position = new Vector3(child.position.x, child.position.y, child.position.y + 0.5f);
        }
    }
}
