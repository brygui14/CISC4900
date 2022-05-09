using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Slider slider;

    public GameObject player;

    void Start(){
            slider.maxValue = 100;
        
    }

    void Update() {
        slider.value = player.GetComponent<PlayerRigidBody>().gethealth();
    }

}
