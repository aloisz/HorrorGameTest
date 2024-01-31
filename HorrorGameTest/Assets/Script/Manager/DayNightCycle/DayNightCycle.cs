using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DayNightCycle : MonoBehaviour
{
    private Vector3 sunPos;
    public float dayDuration = 10.0f; // Durée d'une journée en secondes
    private Light sunLight;
    void Start()
    {
        sunLight = GetComponent<Light>();   
    }

    
    void Update()
    {
        float timeOfDay = Mathf.Repeat(Time.time, dayDuration * 60) / (dayDuration * 60 ); // Normalisation du temps
        float sunAngle = Mathf.Lerp(0, 360, timeOfDay); // Angle du soleil pendant la journée
        
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, 0, 0));

        sunLight.intensity = sunAngle > 0 && sunAngle < 180 ? 1 : 0;
    }
}
