using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour {
    

    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceType;

    void Awake() {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => {
            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 2;
            ResourceAmount[] resourceAmountCost = new ResourceAmount[] {new ResourceAmount() { resourceType = goldResourceType, amount = repairCost }};
            if (ResourceManager.Instance.CanAfford(resourceAmountCost)) {
                // CAN AFFORD THE REPAIRS
                healthSystem.HealFull();
                ResourceManager.Instance.SpendResources(resourceAmountCost);
            } else {
                // CANNOT AFFORD
                TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer { timer = 2f });
            }
        });
    }
}
