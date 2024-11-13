using UnityEngine;

public class AttackItem : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer = default;

    private MeshCollider meshCollider = null;
    private int damage = 0;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(targetLayer, other.gameObject.layer))
        {
            IDamageable recieveDamage = other.gameObject.GetComponent<IDamageable>();
            recieveDamage?.TakeDamage(damage);
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void ToggleCollider(bool status)
    {
        meshCollider.enabled = status;
    }
}
