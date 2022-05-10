using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{

    TextMeshProUGUI score;


    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        score.SetText("Score: " + Mathf.Round(PlayerPrefs.GetFloat("Score")));
    }

    void increaseScore(int value){
        float scorePoint = PlayerPrefs.GetFloat("Score");
        PlayerPrefs.SetFloat("Score", scorePoint + value);

    }
}
