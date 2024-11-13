using UnityEngine;

public class ArrowProjectile : ItemProjectile
{
    private BoxCollider boxCollider = null;
    private Rigidbody rb = null;
    private int damage = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckLayerInMask(targetLayer, collision.gameObject.layer))
        {
            IDamageable recieveDamage = collision.gameObject.GetComponent<IDamageable>();
            recieveDamage?.TakeDamage(damage);

            onRelease?.Invoke();
        }
    }

    public override void Init()
    {
        boxCollider.excludeLayers = ~targetLayer;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void FireArrow(float force, Vector3 direction)
    {
        transform.forward = direction;
        rb.AddForce(force * direction, ForceMode.Impulse);
    }

    public override void Release()
    {
        base.Release();

        rb.velocity = Vector3.zero;
    }
}