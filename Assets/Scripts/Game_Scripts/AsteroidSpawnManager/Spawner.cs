using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    Dictionary<string,List<string>> astreoidLevels = new Dictionary<string,List<string>>();

    string asteroid;
    float health;
    float decreaseHealth;

    string[] beginner = {"PreFabs/Asteroids/AsterHuge1", "PreFabs/Asteroids/AsterHuge2", "PreFabs/Asteroids/AsterHuge3"};
    string[] easy = {"PreFabs/Asteroids/AsterBig1", "PreFabs/Asteroids/AsterBig2", "PreFabs/Asteroids/AsterBig3", "PreFabs/Asteroids/AsterBig4"};
    string[] medium = {"PreFabs/Asteroids/AsterMed1", "PreFabs/Asteroids/AsterMed2", "PreFabs/Asteroids/AsterMed3", "PreFabs/Asteroids/AsterMed4", "PreFabs/Asteroids/AsterMed5"};
    string[] hard = {"PreFabs/Asteroids/AsterSmall1", "PreFabs/Asteroids/AsterSmall2", "PreFabs/Asteroids/AsterSmall3", "PreFabs/Asteroids/AsterSmall4", "PreFabs/Asteroids/AsterSmall5", "PreFabs/Asteroids/AsterSmall6"};

    GameObject spawnObject;
    public float timeToSpawn = 1f;
    float nextTime;
    Vector2 screenHalfWorldUnits;
    List<GameObject> spawns = new List<GameObject>();

    public GameObject player;


    void Start()
    {
        screenHalfWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        nextTime = Time.time + timeToSpawn;
        initalizeDict();
        setDifficulty();

        // string temp = astreoidLevels["Beginner"][0];
        // GameObject obj = Resources.Load<GameObject>(temp);

        // print(obj);

        // print(temp);
        
    }

    // Update is called once per frame
    void Update()
    {
        setDifficulty();
        dropSquare();
        destroyObjects();
    }

    void setDifficulty(){
        // Debug.Log(asteroid);
        if (PlayerPrefs.GetFloat("Score") < 1000){
            asteroid = astreoidLevels["Beginner"][Random.Range(0, beginner.Length)];
            spawnObject = Resources.Load(asteroid) as GameObject;
            spawnObject.GetComponent<FallingBlock>().setAcceleration(1f);
            spawnObject.GetComponent<FallingBlock>().setIncreaseHealth(10);
            health = 100;
            decreaseHealth = health * .25f;
            player.GetComponent<PlayerRigidBody>().setLaserDistance(15f);
            player.GetComponent<PlayerRigidBody>().setDamage(2.5f);
            timeToSpawn = 3.5f;
        }
        else if (PlayerPrefs.GetFloat("Score") >= 1000 && PlayerPrefs.GetFloat("Score") < 2500){
            asteroid = astreoidLevels["Easy"][Random.Range(0, easy.Length)];
            spawnObject = Resources.Load(asteroid) as GameObject;
            spawnObject.GetComponent<FallingBlock>().setAcceleration(1.1f);
            spawnObject.GetComponent<FallingBlock>().setIncreaseHealth(5);
            health = 75;
            decreaseHealth = health * .25f;
            player.GetComponent<PlayerRigidBody>().setLaserDistance(10f);
            player.GetComponent<PlayerRigidBody>().setDamage(2f);
            timeToSpawn = 2.5f;
        }
        else if (PlayerPrefs.GetFloat("Score") >= 2500 && PlayerPrefs.GetFloat("Score") < 5000){
            asteroid = astreoidLevels["Medium"][Random.Range(0, medium.Length)];
            spawnObject = Resources.Load(asteroid) as GameObject;
            spawnObject.GetComponent<FallingBlock>().setAcceleration(1.2f);
            spawnObject.GetComponent<FallingBlock>().setIncreaseHealth(2.5f);
            health = 50;
            decreaseHealth = health * .25f;
            player.GetComponent<PlayerRigidBody>().setLaserDistance(7.5f);
            player.GetComponent<PlayerRigidBody>().setDamage(1.5f);
            timeToSpawn = 2f;
        }
        else if (PlayerPrefs.GetFloat("Score") >= 5000){
            asteroid = astreoidLevels["Hard"][Random.Range(0, hard.Length)];
            spawnObject = Resources.Load(asteroid) as GameObject;
            spawnObject.GetComponent<FallingBlock>().setAcceleration(1.3f);
            spawnObject.GetComponent<FallingBlock>().setIncreaseHealth(1);
            health = 25;
            decreaseHealth = health * .25f;
            player.GetComponent<PlayerRigidBody>().setLaserDistance(5f);
            player.GetComponent<PlayerRigidBody>().setDamage(1);
            timeToSpawn = 1.5f;
        }
    }

    void dropSquare()
    {
        if (Time.time > nextTime){
            nextTime = Time.time + timeToSpawn;
            Vector2 position = new Vector2(Random.Range(-screenHalfWorldUnits.x, screenHalfWorldUnits.x), screenHalfWorldUnits.y + transform.localScale.y);
            // Debug.Log(asteroid);

            GameObject obj = Instantiate(spawnObject, position, Quaternion.identity); 
            obj.GetComponent<FallingBlock>().setHealth(health);
            spawns.Add(obj);
    
        }
        
    }

    void destroyObjects()
    {
        for (int i = 0; i < spawns.Count; i++){
            if (spawns[i] == null){
                spawns.RemoveAt(i);
            }
            else{
                if (spawns[i].transform.position.y < -screenHalfWorldUnits.y - spawns[i].transform.localScale.y && spawns[i].GetComponent<FallingBlock>().isDestroyed != true){
                    player.SendMessage("decreaseHealth", decreaseHealth);
                    Destroy(spawns[i]);
                    spawns.RemoveAt(i);
                }
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
