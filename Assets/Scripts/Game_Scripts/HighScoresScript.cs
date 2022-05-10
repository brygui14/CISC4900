using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighScoresScript : MonoBehaviour
{
    
    const string NAME_KEY = "Player";
    const string SCORE_KEY = "Score";
    const int MENUINDEX = 0;

    [SerializeField] RectTransform fader;

    [SerializeField] TextMeshProUGUI[] players;
    [SerializeField] TextMeshProUGUI[] scores;

    

    void Start() {
        ShowHighScores();
    }
    public void loadMenu(){
        SceneManager.LoadSceneAsync(MENUINDEX, LoadSceneMode.Single);
    }

    void ShowHighScores()
    {
        for (int i = 0; i < 5; i++)
        {
            players[i].SetText(PlayerPrefs.GetString(NAME_KEY + (i+1)).ToString());
            scores[i].SetText(PlayerPrefs.GetInt(SCORE_KEY + (i+1)).ToString());
        }

    }
}
