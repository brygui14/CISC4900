using UnityEngine;
using System.Collections;

/// <summary>
/// A very simple PID controller component class.
/// </summary>
public class PID : MonoBehaviour
{
    public float kp = 2f;
    public float ki = 5f;
    public float kd = .1f;

    private float cumError, rateError, lastError;
    public float GetOutput(float error){
        cumError += error * Time.fixedDeltaTime;
        rateError = (error - lastError) / Time.fixedDeltaTime; 
        lastError = error;           

        return kp*error + ki*cumError + kd*rateError;    

    }
}