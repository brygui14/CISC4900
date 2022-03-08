using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    private bool Collided = false;
    private float acceleration;
    private float speedMultiplier;

    Vector2 ogPosition;

    Rigidbody2D body;

    int deltaDistance = 3;

    // Start is called before the first frame update
    void Start()
    {
        acceleration = Random.Range(30, 125);
        speedMultiplier = Random.Range(.1f, 2);

        ogPosition = transform.position;
        body = GetComponent<Rigidbody2D>();
        Debug.Log("Start");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (transform.position.y + deltaDistance < ogPosition.y && !Collided ){
            body.AddForce(new Vector2(0, 1) * acceleration * speedMultiplier * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collided = true;
    }
}
