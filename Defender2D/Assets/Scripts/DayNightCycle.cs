using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class DayNightCycle : MonoBehaviour {
    
    [SerializeField] private Gradient gradient;
    [SerializeField] private float secondsPerDay = 10f;
    
    private Light2D light2D;
    private float dayTime;
    private float dayTimeSpeed;

    void Awake() {
        light2D = GetComponent<Light2D>();
        dayTimeSpeed = 1 / secondsPerDay;
    }

    void Update() {
        dayTime += Time.deltaTime * dayTimeSpeed;
        light2D.color =gradient.Evaluate(dayTime % 1f);
    }
}
