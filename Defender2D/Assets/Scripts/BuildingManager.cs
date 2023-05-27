using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs {
        public BuildingTypeSO activeBuildingType;
    }


    [SerializeField] private Building hqBuilding;

    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    void Awake() {
        Instance = this;

        buildingTypeList = GameAssets.Instance.buildingTypeList;

        FindObjectOfType<OptionsUI>().OnPauseToggled += OnGamePauseToggled;
    }

    private void OnGamePauseToggled(object sender, EventArgs e) {
        enabled = !enabled;
    }

    void Start() {
        hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            if (activeBuildingType != null) {
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage)) {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray)) {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    } else {
                        TooltipUI.Instance.Show("Cannot afford " + activeBuildingType.GetConstructionResourceCostString(),
                         new TooltipUI.TooltipTimer { timer = 2f });
                    }
                } else {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
                }
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType) {
        activeBuildingType = buildingType;

        OnActiveBuildingTypeChanged?.Invoke(this,
         new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType } );
    }

    public BuildingTypeSO GetActiveBuildingType() {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage) {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] hits = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = hits.Length == 0;
        if (!isAreaClear) {
            errorMessage = "Area is not clear!";
            return false;  // Colliders overlapping!
        } 

        hits = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach (Collider2D hit in hits) {
            if (hit.TryGetComponent<BuildingTypeHolder>(out var buildingTypeHolder)) {
                // Has BuildingTypeHolder
                if(buildingTypeHolder.buildingType == buildingType) {
                    // Another same type building in area!
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }
        }
        if (buildingType.hasResourceGeneratorData) {
            ResourceGeneratorData resourceGeneratorData = buildingType.resourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

            if (nearbyResourceAmount == 0) {
                errorMessage = "There are no nearby Resource Nodes!";
                return false;
            }
        }
        

        float maxConstructionRadius = 25;
        hits = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D hit in hits) {
            if (hit.TryGetComponent<BuildingTypeHolder>(out var buildingTypeHolder)) {
                // Has BuildingTypeHolder
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "Too far from any other building!";
        return false;
    }

    public Building GetHQBuilding() {
        return hqBuilding;
    }
}
