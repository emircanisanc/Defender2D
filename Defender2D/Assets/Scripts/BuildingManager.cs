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

    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    void Awake() {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }
    
    void Update() {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            if(activeBuildingType != null && CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition())) {
                if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray)) {
                    ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                    Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
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

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position) {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] hits = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = hits.Length == 0;
        if (!isAreaClear) return false;  // Colliders overlapping!

        hits = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach (Collider2D hit in hits) {
            if (hit.TryGetComponent<BuildingTypeHolder>(out var buildingTypeHolder)) {
                // Has BuildingTypeHolder
                if(buildingTypeHolder.buildingType == buildingType) {
                    // Another same type building in area!
                    return false;
                }
            }
        }

        float maxConstructionRadius = 25;
        hits = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D hit in hits) {
            if (hit.TryGetComponent<BuildingTypeHolder>(out var buildingTypeHolder)) {
                // Has BuildingTypeHolder
                return true;
            }
        }

        return false;
    }
}
