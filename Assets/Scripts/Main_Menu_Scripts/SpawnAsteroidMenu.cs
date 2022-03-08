using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroidMenu : MonoBehaviour
{
    
    Transform tmp;
    GameObject spawnObject;
    public float timeToSpawn = 1f;
    float nextTime;
    Vector2 screenHalfWorldUnits;
   
    void Start()
    {
        screenHalfWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        nextTime = Time.time + timeToSpawn;
    }

    void Update(){
        if (Input.GetMouseButtonDown(0) && Time.time > nextTime){
            nextTime = Time.time + timeToSpawn;
            dropSquare();
        }
    }


    void dropSquare()
    {
        Vector2 position = new Vector2(Random.Range(-screenHalfWorldUnits.x, screenHalfWorldUnits.x), screenHalfWorldUnits.y + transform.localScale.y);
        spawnObject = Resources.Load("PreFabs/Asteroids/AsterHuge1") as GameObject;
        // GameObject obj = (GameObject)Instantiate(Resources.Load<GameObject>(astreoidLevels["Beginner"][0]), position, Quaternion.identity);
        int angle = findAngle(position, Input.mousePosition);
        print(Input.mousePosition);
        GameObject obj = Instantiate(spawnObject, position, Quaternion.Euler(0,0, angle));
        // Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
        // body.AddForce(new Vector2(0,1)) 
        print(RectTransformUtility.WorldToScreenPoint(Camera.main, obj.transform.position));
        // obj.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.transform.position);
        print(obj.transform.position);
    
    }

    int findAngle(Vector2 orgin, Vector2 point){
        float opp = point.x - orgin.x;
        float adj = point.y - orgin.y;

        float angle = 180 - (Mathf.Rad2Deg * Mathf.Atan(opp/adj));
        return (int)angle;
    }
}
