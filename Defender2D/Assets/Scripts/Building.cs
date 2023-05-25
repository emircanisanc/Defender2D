using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;
    private Transform buildingDemolishBtn;

    void Awake() {
        buildingDemolishBtn = transform.Find("pfBuildingDemolishBtn");
        
        HideBuildingDemolishBtn();
    }

    void Start() {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        Destroy(gameObject);
    }

    void OnMouseEnter() {
        ShowBuildingDemolishBtn();
    }

    void OnMouseExit() {
        HideBuildingDemolishBtn();
    }

    private void ShowBuildingDemolishBtn() {
        if (buildingDemolishBtn != null) {
            buildingDemolishBtn.gameObject.SetActive(true);
        }
    }

    private void HideBuildingDemolishBtn() {
        if (buildingDemolishBtn != null) {
            buildingDemolishBtn.gameObject.SetActive(false);
        }
    }
}
