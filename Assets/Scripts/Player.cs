using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float mySpeed = 5f;
    float screenHalfWorldUnits;


    Camera camera;

    void Start() 
    {
        float playerHalfWidth = transform.localScale.x / 2f;
        camera = Camera.main;
        float aspectRatio = camera.aspect;
        float orthogrpahicSize = camera.orthographicSize;

        screenHalfWorldUnits = aspectRatio * orthogrpahicSize + playerHalfWidth;

    }

    void Update()
    {
        float ogSpeed = mySpeed;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        if (vertical == -1)
        {
            mySpeed = (float)(mySpeed * .4) + mySpeed;
        }

        Vector2 input = new Vector2(horizontal, vertical);
        Vector2 direction = input.normalized;
        Vector2 velocity = direction * mySpeed;

        transform.Translate(velocity * Time.deltaTime);

        BoundaryCheck(screenHalfWorldUnits);

        mySpeed = ogSpeed;
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
