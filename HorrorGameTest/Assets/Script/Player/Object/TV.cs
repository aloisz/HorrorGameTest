using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV : MonoBehaviour
{
    [SerializeField] private Light light;
    public bool canRunCurve;
    [SerializeField] private AnimationCurve lightCurve;
    private float graph, increment, incrementGradientValue;
    
    [SerializeField] private Gradient lightGradient;
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
}
