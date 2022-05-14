using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotate : MonoBehaviour
{
    private float angle = 5;
    float speed = 1f;
    float amount = 1f;

    Animator anim;

    void Start(){
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, angle * Time.deltaTime));
    }

    void shake(){
        transform.position = new Vector3(Mathf.Sin(Time.time * speed) * amount, 0, 0);
    }    
    
    IEnumerator Tremble() {
           for ( int i = 0; i < 5; i++)
           {
               transform.localPosition += new Vector3(.25f, 0, 0);
               yield return new WaitForSeconds(0.05f);
               transform.localPosition -= new Vector3(.25f, 0, 0);
               yield return new WaitForSeconds(0.05f);
           }
     }

     void playAnim(){
         transform.position = new Vector3(0, -7, 0);
         anim.Play("EarthExplosion");
     }


     void destroy(){
         Destroy(gameObject);
     }
}
