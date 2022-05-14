using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    void Start(){
        
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume");
    }

    public void setVolume(float value){
        PlayerPrefs.SetFloat("Volume", value);
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume");
    }
}
