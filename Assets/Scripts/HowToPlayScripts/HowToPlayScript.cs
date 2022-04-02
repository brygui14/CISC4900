using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayScript : MonoBehaviour
{
    
    const int STARTMENUINDEX = 0;
    const int GAMEPLAYINDEX = 1;
    const int OPTIONSMENUINDEX = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Boost")){
            SceneManager.LoadSceneAsync(GAMEPLAYINDEX, LoadSceneMode.Single);
        }
    }
}
