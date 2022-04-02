using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{

    Vector2 move;
    public float speed = 1000f;
    Rigidbody2D body;
    float screenHalfWorldUnits;

    void Awake()
    {
        //Calculate Asteroid Size based on World Units
        float playerHalfWidth = transform.localScale.x;
        float aspectRatio = Camera.main.aspect;
        float orthogrpahicSize = Camera.main.orthographicSize;
        screenHalfWorldUnits = aspectRatio * orthogrpahicSize + playerHalfWidth;


        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        body.AddForce(move * speed * Time.deltaTime);
    }
    void Update()
    {
        move = Vector2.down;
        BoundaryCheck(screenHalfWorldUnits);

        


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
}
