using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float startingTime;

    [SerializeField]
    private float currentTime;

    public Slider timer;

    private void Start()
    {
        resetTime();
    }

    private void FixedUpdate()
    {
        currentTime += 1 * Time.deltaTime;
        timer.value = CalculatedTime();
    }

    public void resetTime()
    {
        currentTime = 0;
    }

    public float CalculatedTime()
    {
        return currentTime / startingTime;
        
    }


}
