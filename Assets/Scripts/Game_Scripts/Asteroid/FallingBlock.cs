using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{

    Vector2 move;
    public float speed = 1000f;
    Rigidbody2D body;
    float screenHalfWorldUnits;
    SpriteRenderer sr;

    void Awake()
    {
        float playerHalfWidth = transform.localScale.x;
        float aspectRatio = Camera.main.aspect;
        float orthogrpahicSize = Camera.main.orthographicSize;

        screenHalfWorldUnits = aspectRatio * orthogrpahicSize + playerHalfWidth;
        body = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>(); 
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
    void BoundaryCheck(float boundaryUnit){
        // print(boundaryUnit);
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
