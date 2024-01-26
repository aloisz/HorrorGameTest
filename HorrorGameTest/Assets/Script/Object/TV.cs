 using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Player;
using UnityEngine;

public class TV : Obj
{
    [SerializeField] private Light light;
    public bool canRunCurve;
    [SerializeField] private AnimationCurve lightCurve;
    private float graph, increment, incrementGradientValue;
    
    [SerializeField] private Gradient lightGradient;
    [SerializeField] private AudioSource musicClip;
    void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    
    void Update()
    {
        if (canRunCurve)
        {
            increment += Time.deltaTime;
            incrementGradientValue += Time.deltaTime;
            
            graph = lightCurve.Evaluate(increment);
            light.intensity = graph;
            
            light.color = lightGradient.Evaluate(incrementGradientValue);
            if (incrementGradientValue > 1)
            {
                incrementGradientValue = 0;
            }
        }
    }

    public override void Interact(PlayerController player)
    {
        canRunCurve = false;
        light.intensity = 0;
        musicClip.mute = true;
    }
}
