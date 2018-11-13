using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeDetector : MonoBehaviour {

    public float ShakeDetectionThreshold;
    public float MinShakeInterval;

    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;

	// Use this for initialization
	void Start () {

        sqrShakeDetectionThreshold = Mathf.Pow(ShakeDetectionThreshold, 2);
	}

    public bool isShaking()
    {
        if (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold
            && Time.unscaledTime >= timeSinceLastShake + MinShakeInterval)
        {
            timeSinceLastShake = Time.unscaledTime;
            return true;
        }
        else
        {
            return false;
        }
        
        
    }
}
