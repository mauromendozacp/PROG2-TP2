using UnityEngine;

public class AttackItem : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer = default;

    private BoxCollider boxCollider = null;
    private int damage = 0;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(targetLayer, other.gameObject.layer))
        {
            IDamagable recieveDamage = other.gameObject.GetComponent<IDamagable>();
            recieveDamage?.Damage(damage);
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void ToggleCollider(bool status)
    {
        boxCollider.enabled = status;
    }
}
