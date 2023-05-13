using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGeneratorOverlay : MonoBehaviour {
    [SerializeField] private ResourceGenerator resourceGenerator;
    
    private Transform barTransform;

    void Start() {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        barTransform = transform.Find("bar");
        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
        transform.Find("text").GetComponent<TextMeshPro>().text = (Mathf.Round(resourceGenerator.GetAmountGeneratedPerSecond() * 10f) / 10f).ToString();
    }

    void Update() {
        barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1, 1);
    }
}
