using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigidBody : MonoBehaviour
{

    public Animator anim;

    float error, output, screenHalfWorldUnits;
    public float speed = 1500f;
    public float maxSpeed;
    public float maxAngleSpeed = 20;
    private PID rotateController;
    Vector2 move;
    Rigidbody2D body;
    // Start is called before the first frame update

    RaycastHit2D[] rays = new RaycastHit2D[90];
    

    void Awake(){
        
        anim = gameObject.GetComponent<Animator>();

        float playerHalfWidth = transform.localScale.x / 8f;
        float aspectRatio = Camera.main.aspect;
        float orthogrpahicSize = Camera.main.orthographicSize;

        screenHalfWorldUnits = aspectRatio * orthogrpahicSize + playerHalfWidth;
        rotateController = gameObject.GetComponent<PID>();
        body = GetComponent<Rigidbody2D>();
        
    }

     void FixedUpdate() { 

        movePlayer();
        projectRayCast();
        BoundaryCheck(screenHalfWorldUnits);
               
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"),  Input.GetAxisRaw("Vertical"));
    }

    void activateShield(){
        anim.Play("Shield");
    }

    void movePlayer(){

         if(body.velocity.magnitude > maxSpeed)
         {
                body.velocity = body.velocity.normalized * maxSpeed;
         }
         
         body.AddForce(move * speed * Time.deltaTime);
    }

    void projectRayCast(){
        int index = 0;
        int distIndex = -1;

        float shortestDist = 100;
   

        for (int i = 45; i < 135; ++i){
            Vector3 point = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(Mathf.Deg2Rad * i), 0);
            rays[index] = Physics2D.Raycast(transform.position, point, 100);

            if (rays[index].collider != null){
                float dist = Vector3.Distance(transform.position, rays[index].point);
                if (dist < shortestDist){
                    // Debug.DrawLine(transform.position, rays[index].point, Color.yellow);
                    shortestDist = dist;
                    distIndex = index;
                    
                }
            //     else{
            //         Debug.DrawLine(transform.position, rays[index].point, Color.red);
            //     }
            // }
            // else{
            //     Debug.DrawLine(transform.position, transform.position + point * 100, Color.green);
            }
            index++;

        }

        if (shortestDist < 2.5){
            activateShield();
        }
        
        // Debug.DrawLine(transform.position, rays[distIndex + 1].point, Color.magenta);
        setAngle(distIndex, 4, 2);


        // print("Dist Index: " + distIndex);
        // print(rays[distIndex].point);
        // print(findAngle(body.position, rays[distIndex].point));
    }

    void setAngle(int index, int angleThreshold, int angleSmoothness){
        if (index == -1){
            int currentAngle = (int)transform.rotation.eulerAngles.z;
            if (currentAngle > 180 + angleThreshold){
                currentAngle -= angleSmoothness;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            else if (currentAngle < 180 - angleThreshold){
                currentAngle += angleSmoothness;
                transform.rotation = Quaternion.Euler(0,0, currentAngle);
            }
            else {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            // print("Current Angle: " + currentAngle + "\nAngle: " + angle);d
        }
        else{

            int angle = findAngle(body.position, rays[index].point);
            int currentAngle = (int)transform.rotation.eulerAngles.z;

            if (currentAngle > angle + angleThreshold){
                currentAngle -= angleSmoothness;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            else if (currentAngle < angle - angleThreshold){
                currentAngle += angleSmoothness;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            }
            else{
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    int findAngle(Vector2 orgin, Vector2 point){
        float opp = point.x - orgin.x;
        float adj = point.y - orgin.y;

        float angle = 180 - (Mathf.Rad2Deg * Mathf.Atan(opp/adj));
        return (int)angle;
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
