using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour {

    public static GameAssets Instance { 
        get {
            if (instance == null) {
                instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return instance;
        }
    }
    private static GameAssets instance;

    public Transform pfEnemy;
    public Transform pfArrow;
    public Transform pfBuildingDestroyedParticles;
    public Transform pfBuildingPlacedParticles;
    public Transform pfEnemyDieParticles;

    public BuildingTypeListSO buildingTypeList;
    public ResourceTypeListSO resourceTypeList;

}
