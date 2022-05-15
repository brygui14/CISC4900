using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRigidBody : MonoBehaviour
{
    const int STARTMENUINDEX = 0;
    const int GAMEPLAYINDEX = 1;
    const int OPTIONSMENUINDEX = 2;
    const int GAMEOVERINDEX = 5;


    GameObject canvas;
    public Animator anim;
    public AudioClip audioSource;

    Renderer rend;
    Material material;
    private int playerScaleDivide = 8;
    private Vector2 screenHalfWorldUnits;
    private float acceleration = 75f;
    private float speedMultiplier = 2;
    private float maxSpeed;
    private float maxSpeedMultiplier = 2.5f;
    private float speed = 7;
    private float laserDistance;
    private float damage;

    private bool isColliding;

    private bool isPaused = false;

    Vector2 move;
    Rigidbody2D body;

    RaycastHit2D[] rays = new RaycastHit2D[40];


    public GameObject lineOrigin, Laser, Arrow, earth;

    int heal = 100;
    bool isDestroyed = false;
    

    void Awake(){
        
        anim = gameObject.GetComponent<Animator>();
        float playerHalfWidth = transform.localScale.x / playerScaleDivide;
        screenHalfWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        body = GetComponent<Rigidbody2D>();
        canvas = GameObject.FindGameObjectWithTag("Pause_Screen");

        LineRenderer tmp = Laser.GetComponent<LineRenderer>();
        tmp.enabled = false;
        
        
    }

    void Start(){
        Arrow = GameObject.Find("Arrow");
        GetComponent<AudioSource> ().playOnAwake = false;
        GetComponent<AudioSource> ().clip = audioSource;
    }

     void FixedUpdate() { 
         if (isDestroyed != true){
            projectRayCast();
            movePlayer();
         }
         BoundaryCheck();
       
    }

    
    void Update()
    {
        if (isDestroyed != true){
            // Debug.Log("Health: " + heal);
            checkHealth();
            getInputs();
            pauseScreen();
        }

        
    }

    void getInputs(){
        move = new Vector2(Input.GetAxisRaw("Horizontal"),  Input.GetAxisRaw("Vertical"));
        
        if (Input.GetKey("space") & !isColliding){
            maxSpeed = speed * maxSpeedMultiplier;
            // Debug.Log(maxSpeed);
        }
        else if (Input.GetButtonDown("Pause")){
            isPaused = true;
            Time.timeScale = 0;
        }
        else if (Input.GetButtonDown("Resume")){
            isPaused = false;
            Time.timeScale = 1; 
        }
        else{
            maxSpeed = speed;
            // Debug.Log(maxSpeed);
        }

    }

    void pauseScreen(){
        if (isPaused){
            canvas.SetActive(true);
            if (Input.GetButtonDown("Quit")){
                Time.timeScale = 1;
                SceneManager.LoadSceneAsync(STARTMENUINDEX, LoadSceneMode.Single);
            }
        }
        else {
            canvas.SetActive(false);
            
             
        }
    }

    void activateShield(){
        anim.Play("shield");
    }

    void movePlayer(){

         if(body.velocity.magnitude > maxSpeed)
         {
                body.velocity = body.velocity.normalized * maxSpeed;
                // Debug.Log("Speed to Great: " + body.velocity);
         }

        
        if (isColliding){
            body.AddForce(move * acceleration * speedMultiplier * Time.fixedDeltaTime, ForceMode2D.Impulse);
            // Debug.Log("Colliding Force: " + move * acceleration * speedMultiplier * Time.fixedDeltaTime);
        }
        else{
            body.AddForce(move * acceleration * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    void projectRayCast(){
        int index = 0;
        int distIndex = -1;

        float shortestDist = 25;
   

        for (int i = 70; i < 110; ++i){
            Vector3 point = new Vector3(Mathf.Cos(Mathf.Deg2Rad * i), Mathf.Sin(Mathf.Deg2Rad * i), 0);
            
            rays[index] = Physics2D.Raycast(transform.position, point, laserDistance);
            

            if (rays[index].collider != null && rays[index].collider.GetComponent<FallingBlock>().isDestroyed != true){
                float dist = Vector2.Distance(transform.position, rays[index].point);
                if (dist < shortestDist){
                    // Debug.DrawLine(transform.position, rays[index].point, Color.yellow);
                    shortestDist = dist;
                    distIndex = index;
                    
                }
                else{
                    Debug.DrawLine(transform.position, rays[index].point, Color.red);
                }
            }
            // else{
            //     Debug.DrawLine(transform.position, transform.position + point * 100, Color.green);
            // }
            index++;

        }

        if (shortestDist < 2.5){
            activateShield();
            if (shortestDist <= 1){
                isColliding = true;
            }
            else{
                isColliding = false;
            }
        }
        // Debug.DrawLine(transform.position, rays[distIndex].point, Color.yellow);
        // Debug.Log("Raycast Point: " + rays[distIndex].point);
        setAngle(distIndex, 4, 2);
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


            deactivateLaser();
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

            activateLaser(rays[index].point);
            passHitData(index);



        }
    }

    int findAngle(Vector2 orgin, Vector2 point){
        float opp = point.x - orgin.x;
        float adj = point.y - orgin.y;

        float angle = 180 - (Mathf.Rad2Deg * Mathf.Atan(opp/adj));
        return (int)angle;
    }

    void BoundaryCheck(){
        // Debug.Log(screenHalfWorldUnits);
        if (transform.position.x < -screenHalfWorldUnits.x)
        {
            transform.position = new Vector2(screenHalfWorldUnits.x, transform.position.y);
        }
        else if (transform.position.x > screenHalfWorldUnits.x)
        {
            transform.position = new Vector2(-screenHalfWorldUnits.x, transform.position.y);
        }
        else if (transform.position.y < -screenHalfWorldUnits.y - transform.localScale.y){
            transform.position = new Vector2(transform.position.x, -screenHalfWorldUnits.y - transform.localScale.y);
            Arrow.SetActive(true);
        }
        else if (transform.position.y < -screenHalfWorldUnits.y){
            Arrow.SetActive(true);
        }
        else if(transform.position.y > -screenHalfWorldUnits.y - transform.localScale.y){
            Arrow.SetActive(false);
        }
        if (transform.position.y > screenHalfWorldUnits.y - transform.localScale.y){
            transform.position = new Vector2(transform.position.x, screenHalfWorldUnits.y - transform.localScale.y);
        }

    }


    void activateLaser(Vector2 point){

        LineRenderer temp = Laser.GetComponent<LineRenderer>();
        // Debug.Log("Laser Point: " + point);
        temp.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
        temp.SetPosition(1, new Vector3(point.x, point.y, 0));

        temp.enabled = true;
    }

    void passHitData(int index){
        RaycastHit2D hit = rays[index];
        if (hit.collider != null && hit.collider.tag == "Asteroids"){
            hit.transform.SendMessage("LaserHit", damage);
        }
    }

    void deactivateLaser(){
        LineRenderer temp = Laser.GetComponent<LineRenderer>();
        temp.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Asteroids"){
            heal -= 5;
        }
    }

    public void increaseHealth(int value){
        if (heal + value <= 95){
            heal += value;
        }
    }

    public void decreaseHealth(int value){
        GetComponent<AudioSource>().Play(0);
        earth.SendMessage("Tremble");
        heal -= value;
    }

    public void checkHealth(){
        if (heal < 0){

            earth.SendMessage("playAnim");
            isDestroyed = true;
            anim.Play("SpaceShipExplosion");
            GameObject.Find("Laser").gameObject.SetActive(false);
        }
    }

    public void destroy(){
        SceneManager.LoadSceneAsync(GAMEOVERINDEX, LoadSceneMode.Single);
        Destroy(gameObject);
    }

    public int gethealth(){
        return heal;
    }

    public void setLaserDistance(float dist){
        laserDistance = dist;
    }

    public void setDamage(float dam){
        damage = dam;
    }

}