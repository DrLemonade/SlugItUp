using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float timeLimit;
    public float initVelocityMN;
    public Rigidbody2D rb;
    public Slug heldSlug;
    public GameObject slugPreset;
    public GameObject slugSpriteObj;
    public Animator animator;

    public Sprite slugSpriteL;
    public Sprite slugSpriteR;
    public Sprite slugSpriteB;
    public Sprite slugSpriteF;

    private int direction; // 0 = forward, 1 = right, 2 = backward, 3 = left
    private GameObject targetSlug = null; // The slug that the player targets to pick up
    private GameObject targetAppliance = null;

    // Start is called before the first frame update
    void Start()
    {
        heldSlug = null;
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(heldSlug != null && Input.GetKeyDown("e")) // Pick up slug from ground
        {
            GameObject newSlug = Instantiate(slugPreset);
            newSlug.transform.position = new Vector2(transform.position.x, transform.position.y);
            SlugController sc = newSlug.GetComponent<SlugController>();
            sc.setSlugType(heldSlug);
            sc.player = transform.GetComponent<PlayerController>();
            Rigidbody2D slugRB = newSlug.GetComponent<Rigidbody2D>();
            slugRB.velocity = new Vector2((Input.mousePosition.x - 585) / 50 - newSlug.transform.position.x, (Input.mousePosition.y - 250) / 45 - newSlug.transform.position.y);
            heldSlug = null;
            slugSpriteObj.SetActive(false);
        } 
        else if(heldSlug == null)
        {
            if (Input.GetKeyDown("e")) 
            {
                if (targetAppliance != null) // Pick up slug from appliance
                {
                    Slug s = targetAppliance.GetComponentInParent<ApplianceController>().getSlug();
                    if (s != null) {
                        HoldSlug(s);
                    }
                }
            }
        }

        if (Time.time > timeLimit)
        {
            SceneController sceneController = new SceneController();
            sceneController.sceneEvent(2);
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal")*speed;
        float moveVertical = Input.GetAxis("Vertical")*speed;
        rb.velocity = new Vector2(moveHorizontal, moveVertical);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        if (moveHorizontal > 0)
        {
            direction = 1;
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteR;
        }
        else if (moveHorizontal < 0)
        {
            direction = 3;
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteL;
        }
        else if (moveVertical > 0)
        {
            direction = 2;
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteB;
        }
        else if (moveVertical < 0)
        {
            direction = 0;
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteF;
        }

        animator.SetFloat("XVelocity", moveHorizontal);
        animator.SetFloat("YVelocity", moveVertical);
    }

    private void OnTriggerEnter2D(Collider2D collision) // NOTE: This could be shortened with inheritance
    {
        bool isBreeder = collision.gameObject.CompareTag("Breeding");
        bool isFeeder = collision.gameObject.CompareTag("Feeder");
        bool isDryer = collision.gameObject.CompareTag("Dryer");

        if (isBreeder || isFeeder || isDryer)
        {
            targetAppliance = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetAppliance)
        {
            targetAppliance = null;
        }
    }

    public void HoldSlug(Slug slug)
    {
        slugSpriteObj.SetActive(true);
        
        Color slugColor = Slug.getColorFromType(slug.getType());

        if (slug.getIsDry())
            slugColor.a = 1f;
        else
            slugColor.a = 200f / 255f;

        slugSpriteObj.GetComponent<SpriteRenderer>().color = slugColor;

        slugSpriteObj.transform.localScale = new Vector3(1, 1, 1);
        slugSpriteObj.transform.localScale *= 0.325f + (slug.getSize() * 0.2f);

        heldSlug = slug;

        switch (direction)
        {
            case 0:
                slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteF;
                break;
            case 1:
                slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteR;
                break;
            case 2:
                slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteB;
                break;
            case 3:
                slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteL;
                break;
        }
    }

    public void setTargetSlug(GameObject slug) { targetSlug = slug; }

    public GameObject getTargetSlug() { return targetSlug; }
}
