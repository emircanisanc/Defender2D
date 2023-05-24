using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour {

    public static ArrowProjectile Create(Vector3 position, Enemy enemy) {
        Transform pfArrowProjectile = Resources.Load<Transform>("pfArrowProjectile");
        Transform arrowTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }

    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float moveSpeed = 20f;
    private float timeToDie = 2f;
    private int damageAmount = 10;

    void Update() {
        Vector3 moveDir;

        if (targetEnemy != null) {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        } else {
            moveDir = lastMoveDir;
        }

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        timeToDie -= Time.deltaTime;
        if (timeToDie < 0f) {
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy targetEnemy) {
        this.targetEnemy = targetEnemy;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Enemy>(out var enemy)) {
            // HIT AN ENEMY
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            
            Destroy(gameObject);
        }    
    }
}
