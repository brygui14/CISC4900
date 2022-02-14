using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    Dictionary<string,List<string>> astreoidLevels = new Dictionary<string,List<string>>();

    string[] beginner = {"PreFabs/Asteroids/AsterHuge1", "PreFabs/Asteroids/AsterHuge2", "PreFabs/Asteroids/AsterHuge3"};
    string[] easy = {"PreFabs/Asteroids/AsterBig1", "PreFabs/Asteroids/AsterBig2", "PreFabs/Asteroids/AsterBig3", "PreFabs/Asteroids/AsterBig4"};
    string[] medium = {"PreFabs/Asteroids/AsterMed1", "PreFabs/Asteroids/AsterMed2", "PreFabs/Asteroids/AsterMed3", "PreFabs/Asteroids/AsterMed4", "PreFabs/Asteroids/AsterMed5"};
    string[] hard = {"PreFabs/Asteroids/AsterSmall1", "PreFabs/Asteroids/AsterSmall2", "PreFabs/Asteroids/AsterSmall3", "PreFabs/Asteroids/AsterSmall4", "PreFabs/Asteroids/AsterSmall5", "PreFabs/Asteroids/AsterSmall6"};

    GameObject spawnObject;
    public float timeToSpawn = 1f;
    float nextTime;
    Vector2 screenHalfWorldUnits;
    List<GameObject> spawns = new List<GameObject>();


    void Start()
    {
        screenHalfWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        nextTime = Time.time + timeToSpawn;
        initalizeDict();

        string temp = astreoidLevels["Beginner"][0];
        GameObject obj = Resources.Load<GameObject>(temp);

        print(obj);

        print(temp);
    }

    // Update is called once per frame
    void Update()
    {
        dropSquare();
        destroyObjects();
    }

    void dropSquare()
    {
        if (Time.time > nextTime){
            nextTime = Time.time + timeToSpawn;
            Vector2 position = new Vector2(Random.Range(-screenHalfWorldUnits.x, screenHalfWorldUnits.x), screenHalfWorldUnits.y + transform.localScale.y);
            spawnObject = Resources.Load(astreoidLevels["Beginner"][0]) as GameObject;
            // GameObject obj = (GameObject)Instantiate(Resources.Load<GameObject>(astreoidLevels["Beginner"][0]), position, Quaternion.identity);
            GameObject obj = Instantiate(spawnObject, position, Quaternion.identity); 
            spawns.Add(obj);
    
        }
        
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

    void initalizeDict()
    {
        astreoidLevels.Add("Beginner", new List<string>(beginner));
        astreoidLevels.Add("Easy", new List<string>(easy));
        astreoidLevels.Add("Medium", new List<string>(medium));
        astreoidLevels.Add("Hard", new List<string>(hard));
    }
}
