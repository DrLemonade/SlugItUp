using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float dropDeltaTime;
    public float zOffsetFromY;
    public float speed;
    public Slug heldSlug;
    public GameObject slugPreset;
    public GameObject slugSpriteObj;
    public GameObject waterDrop;
    public TMP_Text timerTxt;
    public AudioClip timeSound;
    public AudioClip squishSound;

    // Refactor to different script
    public float timeLimit;
    public GameObject timer;
    public GameObject timesUpScreen;
    public GameObject scoreTxt;
    public Sprite slugSpriteL; 
    public Sprite slugSpriteR;
    public Sprite slugSpriteB;
    public Sprite slugSpriteF;
    private int score;
    private bool finishedGame;

    // *** Private instances variables ***

    private Rigidbody2D rb;
    private Animator animator;

    private GameObject targetSlug = null; // The slug that the player targets to pick up
    private GameObject targetAppliance = null;

    private int direction; // 0 = forward, 1 = right, 2 = backward, 3 = left
    private float timeSinceLastDrop;
    private bool hasPlayedTimeSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        heldSlug = null;
        hasPlayedTimeSound = false;
        finishedGame = false;
        direction = 0;
        score = 0;
        timeLimit += Time.time;
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the camera to have the same x and y values as player position

        if (Time.time > timeLimit && !finishedGame)
        {
            timesUpScreen.SetActive(true);
        }
        else
        {
            if (heldSlug != null && Input.GetKeyDown("e")) // Pick up slug from ground
            {
                GameObject newSlug = Instantiate(slugPreset);
                newSlug.transform.position = new Vector2(transform.position.x, transform.position.y);
                SlugController sc = newSlug.GetComponent<SlugController>();
                sc.setSlugType(heldSlug);
                sc.player = transform.GetComponent<PlayerController>();
                Rigidbody2D slugRB = newSlug.GetComponent<Rigidbody2D>();
                int velocityDampen = 5;
                slugRB.velocity = new Vector2((Input.mousePosition.x - Screen.width / 2) / velocityDampen, (Input.mousePosition.y - Screen.height / 2) / velocityDampen);
                heldSlug = null;
                slugSpriteObj.SetActive(false);
            }
            else if (heldSlug == null)
            {
                if (Input.GetKeyDown("e"))
                {
                    if (targetAppliance != null) // Pick up slug from appliance
                    {
                        Slug s = targetAppliance.GetComponentInParent<ApplianceController>().getSlug();
                        if (s != null)
                        {
                            HoldSlug(s);
                            gameObject.GetComponent<AudioSource>().PlayOneShot(squishSound);
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time > timeLimit)
        {
            if (Int32.Parse(scoreTxt.GetComponent<TMP_Text>().text) < score)
            {
                scoreTxt.GetComponent<TMP_Text>().text = (Int32.Parse(scoreTxt.GetComponent<TMP_Text>().text) + 1).ToString();
            }
            scoreTxt.transform.localScale = new Vector2(1 - 1 * .2f * Mathf.Sin(Time.time) * Mathf.Sin(Time.time), 1 - 1 * .2f * Mathf.Cos(Time.time) * Mathf.Cos(Time.time));

        }
        else
        {
            if ((int)(timeLimit - Time.time) % 60 < 10)
            {
                timerTxt.text = (int)(timeLimit - Time.time) / 60 + ":0" + (int)(timeLimit - Time.time) % 60;
            }
            else
            {
                timerTxt.text = (int)(timeLimit - Time.time) / 60 + ":" + (int)(timeLimit - Time.time) % 60;
            }

            if (timeLimit - Time.time < 10 && !hasPlayedTimeSound)
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(timeSound);
                hasPlayedTimeSound = true;
                if ((int)Time.time % 2 == 1)
                {
                    timer.GetComponent<TMP_Text>().color = Color.red;
                }
                else if((int)Time.time % 2 == 0)
                {
                    timer.GetComponent<TMP_Text>().color = Color.white;
                }
            }

            float moveHorizontal = Input.GetAxis("Horizontal") * speed;
            float moveVertical = Input.GetAxis("Vertical") * speed;
            rb.velocity = new Vector2(moveHorizontal, moveVertical);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y - zOffsetFromY);

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

            // Magic number 0.74363f is the original scale of timer text
            timer.transform.localScale = new Vector2(0.74363f - 0.74363f * .2f * Mathf.Sin(Time.time) * Mathf.Sin(Time.time), 0.74363f - 0.74363f * .2f * Mathf.Cos(Time.time) * Mathf.Cos(Time.time));

            if(heldSlug != null)
            {
                if (!heldSlug.getIsDry() && Time.time - timeSinceLastDrop > dropDeltaTime)
                {
                    GameObject drop = Instantiate(waterDrop, slugSpriteObj.transform);
                    drop.transform.localPosition = new Vector3(UnityEngine.Random.Range(-drop.transform.localScale.x * 2, drop.transform.localScale.x * 2), drop.transform.localScale.y, -0.001f);
                    timeSinceLastDrop = Time.time;
                }
            }
        }
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

        gameObject.GetComponent<AudioSource>().PlayOneShot(squishSound);
        
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
    }
}
