using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public static Enemy Create(Vector3 position) {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }


    private Transform targetTransform;
    private Rigidbody2D rb2d;
    private float moveSpeed = 6f;
    private int damage = 10;
    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        if (BuildingManager.Instance.GetHQBuilding() != null) {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(2f, 0.1f);
        ChromaticAberrationEffect.Instance.SetWeight(0.2f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        KillSelf();
    }

    private void KillSelf() {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(5f, 0.15f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        ChromaticAberrationEffect.Instance.SetWeight(0.4f);
        Destroy(gameObject);
    }

    void Update() {
        HandleMovement();
        HandleTargeting();
    }

    void OnCollisionEnter2D(Collision2D other) {
        Building building;
        if (other.gameObject.TryGetComponent<Building>(out building)) {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(damage);
            KillSelf();
        }
    }
    
    private void HandleTargeting() {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f) {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void HandleMovement() {
        if(targetTransform != null) {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            rb2d.velocity = moveDir * moveSpeed;
        } else {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void LookForTargets() {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray) {
            Building building = collider2D.GetComponent<Building>();
            if (building != null) {
                // Is a building!
                if (targetTransform == null) {
                    targetTransform = building.transform;
                } else {
                    if(Vector3.Distance(transform.position, building.transform.position) < 
                       Vector3.Distance(transform.position, targetTransform.position)) {
                        // Closer!
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (targetTransform == null) {
                if (BuildingManager.Instance.GetHQBuilding() != null) {
                    targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
            }
        }
    }
}
