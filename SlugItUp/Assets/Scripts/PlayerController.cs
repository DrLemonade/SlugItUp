using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Sprite slugSpriteL; 
    public Sprite slugSpriteR;
    public Sprite slugSpriteB;
    public Sprite slugSpriteF;

    public float zOffsetFromY;
    public float speed;
    public Slug heldSlug;
    public GameObject slugPreset;
    public GameObject slugSpriteObj;
    public TMP_Text timerTxt;
    public AudioClip timeSound;
    public AudioClip squishSound;

    // Refactor to different script
    public float timeLimit;
    public GameObject timer;
    public GameObject timesUpScreen;
    public GameObject scoreTxt;
    private int score;
    private bool finishedGame;

    // *** Private instances variables ***

    private Rigidbody2D rb;
    private Animator animator;

    private GameObject targetAppliance = null;

    private int direction; // 0 = forward, 1 = right, 2 = backward, 3 = left
    private bool hasPlayedTimeSound;

    public GameObject[] inRangeSlugs;
    private CircleCollider2D collectorCollider;

    // Start is called before the first frame update
    void Start()
    {
        inRangeSlugs = new GameObject[0];
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collectorCollider = GetComponentInChildren<CircleCollider2D>();

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
                sc.setSlug(heldSlug);
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
                    else
                    {
                        // Find which slug is the closest to the player
                        GameObject closestObject = null;
                        for (int i = 0; i < inRangeSlugs.Length; i++)
                        {
                            if (closestObject == null)
                            {
                                closestObject = inRangeSlugs[i];
                                continue;
                            }

                            float distance1 = Vector2.Distance (transform.position, inRangeSlugs[i].transform.position);
                            float distance2 = Vector2.Distance (transform.position, closestObject.transform.position);
                            if (distance1 < distance2)
                                closestObject = inRangeSlugs[i];
                        }

                        // Hold and delete slug gameobject
                        if (closestObject != null)
                        {
                            HoldSlug(closestObject.GetComponent<SlugController>().getSlug());
                            Destroy(closestObject);
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
        
        // Add slug to inRangeSlugs if it is not already inside the inRangeSlugs array
        if (collision.gameObject.CompareTag("Slug"))
        {
            if (!isSlugInRange(collision.gameObject))
            {
                GameObject[] temp = new GameObject[inRangeSlugs.Length + 1];
                for (int i = 0; i < inRangeSlugs.Length; i++)
                    temp[i] = inRangeSlugs[i];
                temp[inRangeSlugs.Length] = collision.gameObject;
                inRangeSlugs = temp;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == targetAppliance)
        {
            targetAppliance = null;
        }

        // Remove slugs from inRangeSlugs array if it is inside the inRangeSlugs array
        if (collision.gameObject.CompareTag("Slug") && !collectorCollider.IsTouching(collision))
        {
            if (isSlugInRange(collision.gameObject)) 
            {
                GameObject[] temp = new GameObject[inRangeSlugs.Length - 1];
                for (int i = 0, j = 0; i < inRangeSlugs.Length; i++)
                {
                    if (collision.gameObject != inRangeSlugs[i])
                    {
                        temp[j] = inRangeSlugs[i];
                        j++;
                    }
                }
                inRangeSlugs = temp;
            }
        }
    }

    private bool isSlugInRange(GameObject slug)
    {
        foreach (var obj in inRangeSlugs)
            if (obj == slug)
                return true;

        return false;
    }

    public void HoldSlug(Slug slug)
    {
        heldSlug = slug;
        slugSpriteObj.SetActive(true);

        // Decide which slugs sprite to use
        if (direction == 0)
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteF;
        else if (direction == 1)
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteR;
        else if (direction == 2)
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteB;
        else if (direction == 3)
            slugSpriteObj.GetComponent<SpriteRenderer>().sprite = slugSpriteL;

        // Set scale
        slugSpriteObj.transform.localScale = new Vector3(1, 1, 1);
        slugSpriteObj.transform.localScale *= 0.325f + (slug.getSize() * 0.2f);

        // Set color
        Color slugColor = Slug.getColorFromType(slug.getType());
        if (slug.getIsDry())
            slugColor.a = 1f;
        else
            slugColor.a = 0.7f;
        slugSpriteObj.GetComponent<SpriteRenderer>().color = slugColor;

        // Do dripping and play sound
        gameObject.GetComponent<AudioSource>().PlayOneShot(squishSound);
        slugSpriteObj.GetComponent<Drippy>().setDoDripping(!slug.getIsDry());
    }

    public void addScore(int s)
    {
        score += s;
    }
}
