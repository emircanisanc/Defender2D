using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {
    private GameObject spriteGameObject;
    [SerializeField] private ResourceNearbyOverlay resourceNearbyOverlay;

    void Awake() {
        spriteGameObject = transform.Find("sprite").gameObject;

        Hide();
    }

    void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    void Update() {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        if(e.activeBuildingType == null) {
            Hide();
            resourceNearbyOverlay.Hide();
        } else {
            Show(e.activeBuildingType.sprite);
            resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
        }
    }

    private void Show(Sprite ghostSprite) {
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        spriteGameObject.SetActive(true);
    }

    private void Hide() {
        spriteGameObject.SetActive(false);
    }
}
