using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    
    [SerializeField] private HealthSystem healthSystem;

    private Transform barTransform;

    void Awake() {
        barTransform = transform.Find("bar");
    }

    void Start() {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OHealed;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OHealed(object sender, System.EventArgs e) {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar() {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisible() {
        if (healthSystem.IsFullHealth()) {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
    }
}
