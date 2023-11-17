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
    private GameObject targetBreeding = null; // The breeding pool that the player targets to pick a slug from
    private GameObject targetDryer = null;
    private GameObject targetFeeder = null;

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
        if(heldSlug == null)
        {
            if (Input.GetKeyDown("e") && targetBreeding != null && targetBreeding.GetComponentInParent<BreedingController>() != null) // Pick up slug from breeding pool
            {
                heldSlug = targetBreeding.GetComponentInParent<BreedingController>().getNewSlug();
                targetBreeding.GetComponentInParent<BreedingController>().setNewSlug();
                targetBreeding.GetComponentInParent<SpriteRenderer>().color = Color.black;
            }

            if (Input.GetKeyDown("e") && targetFeeder != null && targetFeeder.GetComponentInParent<FeederController>() != null) // Pick up slug from feeder pool
            {
                heldSlug = targetFeeder.GetComponentInParent<FeederController>().getSlug();
                targetBreeding.GetComponentInParent<FeederController>().empty();
                targetBreeding.GetComponentInParent<SpriteRenderer>().color = Color.red;
            }

            if (Input.GetKeyDown("e") && targetDryer != null && targetDryer.GetComponentInParent<DryerController>() != null) // Pick up slug from dryer pool
            {
                heldSlug = targetBreeding.GetComponentInParent<DryerController>().getSlug();
                targetBreeding.GetComponentInParent<DryerController>().empty();
                targetBreeding.GetComponentInParent<SpriteRenderer>().color = Color.yellow;
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
        if (collision.gameObject.CompareTag("Breeding"))
        {
            targetBreeding = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Feeder"))
        {
            targetFeeder = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Dryer"))
        {
            targetDryer = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetBreeding)
        {
            targetBreeding = null;
        }

        if (collision.gameObject.CompareTag("Feeder"))
        {
            targetFeeder = null;
        }

        if (collision.gameObject.CompareTag("Dryer"))
        {
            targetDryer = null;
        }
    }

    public void HoldSlug(Slug slug)
    {
        heldSlug = slug;
        slugSpriteObj.SetActive(true);
        slugSpriteObj.GetComponent<SpriteRenderer>().color = Slug.getColorFromType(slug.getType());
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
