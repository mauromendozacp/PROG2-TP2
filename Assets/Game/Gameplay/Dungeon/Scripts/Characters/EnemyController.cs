using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    public void Damage(int damageAmount)
    {
        Debug.Log("Get Some Damage, But not too much");
    }

    
}
