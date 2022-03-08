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
        Vector2 position = new Vector2(Random.Range(-screenHalfWorldUnits.x, -3), screenHalfWorldUnits.y + transform.localScale.y);
        Debug.Log(position);
        position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        Debug.Log(position);
        spawnObject = Resources.Load("PreFabs/UIAsteroid/AsterSmall1UI") as GameObject;
        

        GameObject obj = Instantiate(spawnObject, position, Quaternion.Euler(0,0, 0));
        obj.transform.SetParent(gameObject.transform); 
        // obj.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.transform.position);
        Rigidbody2D asteroid = obj.GetComponent<Rigidbody2D>();

        asteroid.AddForce(new Vector2(Input.mousePosition.x - position.x, Input.mousePosition.y - position.y), ForceMode2D.Impulse);

        spawns.Add(obj);
    }

    
}
