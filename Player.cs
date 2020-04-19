using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float deathVel = 10f; //how fast should fly away on death
    public float deathSpin = 1000f; //how much torque to apply on death
    PlayerMovement movement;

    public Transform spritemask;
    public float fovShrinkRate = 0.99f;
    public bool shouldShrink = true;
    
    CapsuleCollider2D[] colliders;
    Rigidbody2D rb;

    int currentScene;

    bool activated = true;

    private void Awake()
    {
        colliders = GetComponents<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldShrink)
        {
            Vector2 spritemaskSize = spritemask.localScale;
            spritemaskSize.x -= fovShrinkRate * spritemaskSize.x * Time.deltaTime;
            spritemaskSize.y -= fovShrinkRate * spritemaskSize.y * Time.deltaTime;
            spritemask.localScale = spritemaskSize;
            if (spritemaskSize.x < 4f)
            {
                Die(true);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy")){
            Die(true);
        } else if (collision.transform.CompareTag("Exit"))
        {
            collision.gameObject.SetActive(false);
            Die(false); //just to get him to spin away, not actually dying
            StartCoroutine(LoadScene(currentScene + 1, 1f));
            //consider adding some kind of victory thing
        }
    }

    private void OnBecameInvisible()
    {
        //without this activated check, Unity tries to run this again after scene is reset for some reason
        //maybe camera is shut down before game object, which causes this to be run again?
        //Just checked, it's because onBecameInvisible() runs when the game is closed
        if (activated)
            StartCoroutine(LoadScene(currentScene, 1f));
    }

    private void Die(bool shouldGoLeft)
    {
        //disable colliders, spin away
        foreach (CapsuleCollider2D col in colliders)
        {
            col.enabled = false;
        }
        rb.velocity = new Vector2((shouldGoLeft)? -1f : 1f, 1f) * deathVel;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddTorque((shouldGoLeft) ? deathSpin : -deathSpin);
        movement.enabled = false;

        //onBecameInvisible() will handle reloading scene
    }

    IEnumerator LoadScene(int sceneIndex, float waitTime)
    {
        activated = false;
        //Debug.Log("Loading");
        yield return new WaitForSeconds(waitTime);

        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);


        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void OnApplicationQuit()
    {
        activated = false;
    }
}
