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
    public Animator animator;

    private GameObject targetSlug = null; // The slug that the player targets to pick up
    private GameObject targetBreeding = null; // The breeding pool that the player targets to pick a slug from

    // Start is called before the first frame update
    void Start()
    {
        heldSlug = null;
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
        }
        if(heldSlug == null)
        {
            if (Input.GetKeyDown("e") && targetBreeding != null && targetBreeding.GetComponentInParent<BreedingController>() != null) // Pick up slug from breeding pool
            {
                heldSlug = targetBreeding.GetComponentInParent<BreedingController>().getNewSlug();
                targetBreeding.GetComponentInParent<BreedingController>().setNewSlug();
                targetBreeding.GetComponentInParent<SpriteRenderer>().color = Color.black;
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

        animator.SetFloat("XVelocity", moveHorizontal);
        animator.SetFloat("YVelocity", moveVertical);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Breeding"))
        {
            targetBreeding = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetBreeding)
        {
            targetBreeding = null;
        }
    }

    public void HoldSlug(Slug slug)
    {
        heldSlug = slug;
    }

    public void setTargetSlug(GameObject slug) { targetSlug = slug; }

    public GameObject getTargetSlug() { return targetSlug; }
}
