using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector2 position) {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceCount = 0;

        foreach (Collider2D hit in hits) {
            if (hit.TryGetComponent<ResourceNode>(out var resourceNode)) {
                // It's a resource node!
                if(resourceNode.resourceType == resourceGeneratorData.resourceType) {
                    // Same type!
                    nearbyResourceCount++;
                }
            }
        }

        nearbyResourceCount = Mathf.Clamp(nearbyResourceCount, 0, resourceGeneratorData.maxResourceAmount);

        return nearbyResourceCount;
    }



    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;

    void Awake() {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    void Start() {
        
        int nearbyResourceCount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);
        
        if (nearbyResourceCount == 0) {
            // No resources nearby
            timerMax = 0;
            GetComponentInChildren<Animator>().enabled = false;
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

    public ResourceGeneratorData GetResourceGeneratorData() {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized() {
        if (timerMax != 0) {
            return timer / timerMax;
        } else {
            return 0;
        }
    }

    public float GetAmountGeneratedPerSecond() {
        if (timerMax != 0) {
            return 1 / timerMax;
        } else {
            return 0;
        }
    }
}
