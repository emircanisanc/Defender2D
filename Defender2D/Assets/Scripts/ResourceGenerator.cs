using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {
    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;

    void Awake() {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    void Start() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceCount = 0;

        foreach (Collider2D hit in hits) {
            if (hit.TryGetComponent<ResourceNode>(out var resourceNode)) {
                // It's a resource node!
                if(resourceNode.resourceType == resourceGeneratorData.resourceType) {
                    // Same type!
                    nearbyResourceCount++;
                    if(resourceGeneratorData.maxResourceAmount == nearbyResourceCount) {
                        // Reached to limit!
                        break;
                    }
                }
            }
        }

        if (nearbyResourceCount == 0) {
            // No resources nearby
            enabled = false;
        } else {
            // Calculate generation speed by nearby resource amount
            timerMax = (resourceGeneratorData.timerMax / 2f) +
             resourceGeneratorData.timerMax *
             (1 - (float)nearbyResourceCount / resourceGeneratorData.maxResourceAmount);
        }
    }

    void Update() {
        timer -= Time.deltaTime;
        if(timer <= 0f) {
            timer += timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }
}
