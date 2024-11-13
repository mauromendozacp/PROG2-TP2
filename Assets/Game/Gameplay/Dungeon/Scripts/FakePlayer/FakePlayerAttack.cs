using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FakePlayerAttack : MonoBehaviour
{
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private LayerMask _enemyLayer;

    public void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, _attackRange, _enemyLayer);
        Debug.Log($"Enemigos detectados: {hitEnemies.Length}");
        foreach (Collider enemy in hitEnemies)
        {
            IDamagable damageable = enemy.GetComponent<IDamagable>();
            if (damageable != null)
            {
                Debug.Log($"Aplicando daño a: {enemy.name}");
                damageable.Damage(_attackDamage);
            }
            else
            {
                Debug.LogWarning($"El objeto {enemy.name} no implementa IDamagable.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
