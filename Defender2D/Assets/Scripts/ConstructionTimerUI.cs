using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour {
    
    [SerializeField] private BuildingConstruction buildingConstruction;

    private Image constructionProgressImage;

    void Awake() {
        constructionProgressImage = transform.Find("mask").Find("image").GetComponent<Image>();
    }

    void Update() {
        constructionProgressImage.fillAmount = buildingConstruction.GetConstructionTimerNormalized();
    }

}
