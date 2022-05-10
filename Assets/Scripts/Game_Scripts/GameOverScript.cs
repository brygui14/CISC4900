using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverScript : MonoBehaviour
{
    const int STARTMENUINDEX = 0;
    const int NUM_HIGH_SCORES = 5;
    const string NAME_KEY = "Player";
    const string SCORE_KEY = "Score";

    public GameObject saveButton, InputField, saveScoreButton, score, saved;
    // Start is called before the first frame update
    void Start()
    {
        InputField.SetActive(false);
        saveScoreButton.SetActive(false);
        saved.SetActive(false);
        score.GetComponent<TextMeshProUGUI>().SetText(Mathf.Round(PlayerPrefs.GetFloat("Score")).ToString());
    }

    void Update() {
        if (Input.GetKeyDown("space")){
            loadMenu();
        }   
    }
    public void loadName(){
        InputField.SetActive(true);
        saveScoreButton.SetActive(true);
        saveButton.SetActive(false);
    }

    public void saveScore(){
        saveScoreButton.SetActive(false);
        saved.SetActive(true);
        saveScores();
    }

    void saveScores()
    {
        int playerScore = (int)Mathf.Round(PlayerPrefs.GetFloat("Score"));
        string playerName = InputField.GetComponent<TMP_InputField>().text;

        if (playerName.Equals("")){
            playerName = "Player";
        }

        for (int i = 1; i <= NUM_HIGH_SCORES; i++)
        {
            string currentNameKey = NAME_KEY + i;
            string currentScoreKey = SCORE_KEY + i;

            if (PlayerPrefs.HasKey(currentScoreKey))
            {
                int currentScore = PlayerPrefs.GetInt(currentScoreKey);
                if (playerScore > currentScore)
                {
                    int tempScore = currentScore;
                    string tempName = PlayerPrefs.GetString(currentNameKey);

                    PlayerPrefs.SetString(currentNameKey, playerName);
                    PlayerPrefs.SetInt(currentScoreKey, playerScore);

                    playerName = tempName;
                    playerScore = tempScore;
                }

            }
            else
            {
                PlayerPrefs.SetString(currentNameKey, playerName);
                PlayerPrefs.SetInt(currentScoreKey, playerScore);
                return;
            }
        }
    }

    void loadMenu(){
        SceneManager.LoadSceneAsync(STARTMENUINDEX, LoadSceneMode.Single);
    }

}
