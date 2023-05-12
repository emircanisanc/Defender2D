using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {
    private BuildingTypeSO buildingType;
    private float timer;
    private float timerMax;
    void Awake() {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        timerMax = buildingType.resourceGeneratorData.timerMax;
    }

    void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0f) {
            timer += timerMax;
            ResourceManager.Instance.AddResource(buildingType.resourceGeneratorData.resourceType, 1);
        }
    }
}
