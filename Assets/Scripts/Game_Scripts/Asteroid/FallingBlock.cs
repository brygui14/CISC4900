using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{

    Vector2 move;
    public float speed = 1000f;
    private float health = 100;
    float value;
    Rigidbody2D body;
    GameObject score;
    float screenHalfWorldUnits;
    Animator anim;
    public AudioClip audioSource;

    public bool isDestroyed = false;

    void Awake()
    {
        //Calculate Asteroid Size based on World Units
        float playerHalfWidth = transform.localScale.x;
        float aspectRatio = Camera.main.aspect;
        float orthogrpahicSize = Camera.main.orthographicSize;
        screenHalfWorldUnits = aspectRatio * orthogrpahicSize + playerHalfWidth;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void Start(){
        score = GameObject.Find("Score");
        GetComponent<AudioSource> ().playOnAwake = false;
        GetComponent<AudioSource> ().clip = audioSource;
    }

    void FixedUpdate()
    {
        body.AddForce(move * speed * Time.deltaTime);
    }
    void Update()
    {
        move = Vector2.down;
        BoundaryCheck(screenHalfWorldUnits);

        if (health < 0 && isDestroyed != true){
            GetComponent<AudioSource>().Play(0);
            isDestroyed = true;
            GetComponent<CircleCollider2D>().isTrigger = true;
            anim.Play("Explosion");
        }

    }
    
    //If the asteroid's body goes outside the screenview teleport the asteroid to the other side for continoius scrolling effect
    void BoundaryCheck(float boundaryUnit){
        if (transform.position.x < -boundaryUnit)
        {
            transform.position = new Vector2(boundaryUnit, transform.position.y);
        }
        else if (transform.position.x > boundaryUnit)
        {
            transform.position = new Vector2(-boundaryUnit, transform.position.y);
        }
    }

    void LaserHit(){
        // Debug.Log("I was hit by laser");
        health -= 1;
        score.SendMessage("increaseScore", 1);
        // Debug.Log(health);
    }

    void destroy(){
        GameObject player = GameObject.Find("spaceship_4");
        player.SendMessage("increaseHealth", 5);

        Destroy(gameObject);
    }
}
