using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    const int STARTMENUINDEX = 0;
    const int GAMEPLAYINDEX = 1;
    const int OPTIONSMENUINDEX = 2;
    private bool Collided = false;
    private float acceleration;
    private float speedMultiplier;
    Vector2 ogPosition;

    Rigidbody2D body;

    int deltaDistance = 3;

    void Start()
    {
        acceleration = Random.Range(30, 125);
        speedMultiplier = Random.Range(.1f, 2);

        ogPosition = transform.position;
        body = GetComponent<Rigidbody2D>();
        
        }

    void FixedUpdate()
    {
        
        if (transform.position.y + deltaDistance < ogPosition.y && !Collided ){
            body.AddForce(new Vector2(0, 1) * acceleration * speedMultiplier * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Collided = true;

        if (gameObject.tag == "Play_Button"){
            StartCoroutine(playGame());
        }
        else if(gameObject.tag == "Options_Button"){
            StartCoroutine(OptionsMenu());
        }
        else if(gameObject.tag == "Quit_Button"){
            StartCoroutine(QuitGame());
        }
        else if(gameObject.tag == "Back_Button"){
            StartCoroutine(BackToMainMenu());
        }
        
    }

    IEnumerator playGame()  
    {
        yield return new WaitForSeconds(5);
        Collided = false;
        SceneManager.LoadSceneAsync(GAMEPLAYINDEX, LoadSceneMode.Single);
    }

    IEnumerator OptionsMenu()  
    {
        // yield return new WaitUntil(() => Collided == true);
        yield return new WaitForSeconds(5);
        Collided = false;
        SceneManager.LoadSceneAsync(OPTIONSMENUINDEX, LoadSceneMode.Single);
    }

    IEnumerator QuitGame()  
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }

    IEnumerator BackToMainMenu(){
        yield return new WaitForSeconds(5);
        Collided = false;
        SceneManager.LoadSceneAsync(STARTMENUINDEX, LoadSceneMode.Single);
    }
}
