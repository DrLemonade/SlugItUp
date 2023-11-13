using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public Rigidbody2D rb;
    public Slug heldSlug;

    // Start is called before the first frame update
    void Start()
    {
        heldSlug = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal")*speed;
        float moveVertical = Input.GetAxis("Vertical")*speed;
        rb.velocity = new Vector2(moveHorizontal, moveVertical);
    }

    public void HoldSlug(Slug slug)
    {
        heldSlug = slug;
    }
}
