using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    [SerializeField] private Transform pfWoodHarvester;
    private Camera mainCam;
    void Start() {
        mainCam = Camera.main;
    }
    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Instantiate(pfWoodHarvester, GetMouseWorldPosition(), Quaternion.identity);
        }
    }

    private Vector3 GetMouseWorldPosition() {
        Vector3 mouseWorldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}
