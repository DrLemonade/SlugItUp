using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float timeLimit;
    public float initVelocityMN;
    public float Z_CONSTANT;
    public Rigidbody2D rb;
    public Slug heldSlug;
    public GameObject slugPreset;
    public GameObject slugSpriteObj;
    public TMP_Text timerTxt;
    public TMP_Text scoreTxt;
    public Animator animator;

    public Sprite slugSpriteL;
    public Sprite slugSpriteR;
    public Sprite slugSpriteB;
    public Sprite slugSpriteF;

    private int direction; // 0 = forward, 1 = right, 2 = backward, 3 = left
    private int score;
    private GameObject targetSlug = null; // The slug that the player targets to pick up
    private GameObject targetAppliance = null;

    // Start is called before the first frame update
    void Start()
    {
        heldSlug = null;
        direction = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the camera to have the same x and y values as player position
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);

        if(heldSlug != null && Input.GetKeyDown("e")) // Pick up slug from ground
        {
            GameObject newSlug = Instantiate(slugPreset);
            newSlug.transform.position = new Vector2(transform.position.x, transform.position.y);
            SlugController sc = newSlug.GetComponent<SlugController>();
            sc.setSlugType(heldSlug);
            sc.player = transform.GetComponent<PlayerController>();
            Rigidbody2D slugRB = newSlug.GetComponent<Rigidbody2D>();
            int velocityDampen = 25;
            slugRB.velocity = new Vector2((Input.mousePosition.x - Screen.width / 2) / velocityDampen, (Input.mousePosition.y - Screen.height / 2) / velocityDampen);
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
        if ((int)(timeLimit - Time.time) % 60 < 10)
        {
            timerTxt.text = (int)(timeLimit - Time.time) / 60 + ":0" + (int)(timeLimit - Time.time) % 60;
        }
        else
        {
            timerTxt.text = (int)(timeLimit - Time.time) / 60 + ":" + (int)(timeLimit - Time.time) % 60;
        }
        float moveHorizontal = Input.GetAxis("Horizontal")*speed;
        float moveVertical = Input.GetAxis("Vertical")*speed;
        rb.velocity = new Vector2(moveHorizontal, moveVertical);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y - Z_CONSTANT);

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

    private void OnTriggerEnter2D(Collider2D collision) // NOTE: Make a tag that is just "Appliance"
    {
        bool isBreeder = collision.gameObject.CompareTag("Breeding");
        bool isFeeder = collision.gameObject.CompareTag("Feeder");
        bool isDryer = collision.gameObject.CompareTag("Dryer");
        bool isSubbmission = collision.gameObject.CompareTag("Submission");

        if (isBreeder || isFeeder || isDryer || isSubbmission)
        {
            targetAppliance = collision.gameObject;
        }

        if (isSubbmission)
            collision.gameObject.GetComponentInParent<SubmissionTable>().label.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetAppliance)
        {
            targetAppliance = null;
        }

        if (collision.gameObject.CompareTag("Submission"))
            collision.gameObject.GetComponentInParent<SubmissionTable>().label.SetActive(false);
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

    public void addScore(int s)
    {
        score += s;
        scoreTxt.text = score.ToString();
    }
}
