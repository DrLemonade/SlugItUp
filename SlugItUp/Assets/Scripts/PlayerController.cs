using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public Rigidbody2D rb;
    public Slug heldSlug;
    public GameObject slugPreset;

    // Start is called before the first frame update
    void Start()
    {
        heldSlug = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(heldSlug != null && Input.GetKeyDown("e"))
        {
            GameObject newSlug = Instantiate(slugPreset);
            SlugController sc = newSlug.GetComponent<SlugController>();
            sc.setSlugType(heldSlug);
            sc.player = transform.GetComponent<PlayerController>();
            Rigidbody2D slugRB = newSlug.GetComponent<Rigidbody2D>();
            slugRB.velocity = new Vector2((Input.mousePosition.x - 575) / 25 - newSlug.transform.position.x, (Input.mousePosition.y - 265) / 25 - newSlug.transform.position.y);
            heldSlug = null;
        }
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
