using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour {
    
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType) {
        Transform pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType( buildingType);
        return buildingConstruction;
    }


    private float constructionTimer;
    private float constructionTimerMax;
    private BuildingTypeSO buildingType;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionMaterial;

    void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        constructionMaterial = spriteRenderer.material;
    }

    void Update() {
        constructionTimer -= Time.deltaTime;

        constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());
        if (constructionTimer <= 0f) {
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType(BuildingTypeSO buildingType) {
        this.buildingType = buildingType;

        this.constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        spriteRenderer.sprite = buildingType.sprite;

        boxCollider2D.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2D.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;

        buildingTypeHolder.buildingType = buildingType;
    }

    public float GetConstructionTimerNormalized() {
        return 1 - constructionTimer / constructionTimerMax;
    }
}
