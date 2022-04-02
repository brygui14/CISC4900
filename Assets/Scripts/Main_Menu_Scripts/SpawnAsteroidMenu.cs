using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroidMenu : MonoBehaviour
{
    
    GameObject spawnObject;
    public float timeToSpawn = .25f;
    float nextTime;
    Vector2 screenHalfWorldUnits;
    List<GameObject> spawns = new List<GameObject>();

   
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
        destroyObjects();
    }

    void destroyObjects()
    {
        for (int i = 0; i < spawns.Count; i++){
            if (spawns[i].transform.position.y < -screenHalfWorldUnits.y - spawns[i].transform.localScale.y)
            {
                Destroy(spawns[i]);
                spawns.RemoveAt(i);
            }
        }  
    }

    void dropSquare()
    {
        Vector2 position = new Vector2(-screenHalfWorldUnits.x + -transform.localScale.x, screenHalfWorldUnits.y + transform.localScale.y);
        Debug.Log(position);
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0){
            position.x = screenHalfWorldUnits.x + transform.localScale.x;
        }
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 0){
            position.y = screenHalfWorldUnits.y - 2;
        }
        Debug.Log(position);
        position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        spawnObject = Resources.Load("PreFabs/UIAsteroid/AsterHuge1UI") as GameObject;
        

        GameObject obj = Instantiate(spawnObject, position, Quaternion.Euler(0,0, 0));
        obj.transform.SetParent(gameObject.transform); 
        Rigidbody2D asteroid = obj.GetComponent<Rigidbody2D>();

        asteroid.AddForce(new Vector2(Input.mousePosition.x - position.x, Input.mousePosition.y - position.y), ForceMode2D.Impulse);

        spawns.Add(obj);
    }

    
}
