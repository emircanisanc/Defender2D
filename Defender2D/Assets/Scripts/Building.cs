using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;
    private Transform buildingDemolishBtn;
    private Transform buildingRepairBtn;

    void Awake() {
        buildingDemolishBtn = transform.Find("pfBuildingDemolishBtn");
        buildingRepairBtn = transform.Find("pfBuildingRepairBtn");

        HideBuildingDemolishBtn();
        HideBuildingRepairBtn();
    }

    void Start() {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;

        healthSystem.OnDied += HealthSystem_OnDied;

    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        if (healthSystem.IsFullHealth()) {
            HideBuildingRepairBtn();
        }
    }
    
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        ShowBuildingRepairBtn();
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
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

    private void ShowBuildingRepairBtn() {
        if (buildingRepairBtn != null) {
            buildingRepairBtn.gameObject.SetActive(true);
        }
    }

    private void HideBuildingRepairBtn() {
        if (buildingRepairBtn != null) {
            buildingRepairBtn.gameObject.SetActive(false);
        }
    }
}
