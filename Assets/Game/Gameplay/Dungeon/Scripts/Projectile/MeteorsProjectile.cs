using UnityEngine;

public class MeteorsProjectile : ItemProjectile
{
    [SerializeField] private float initialForce = 0f;

    private Rigidbody rb = null;
    private int damage = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.AddForce(-transform.up * initialForce, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(targetLayer, other.gameObject.layer))
        {
            IDamageable recieveDamage = other.gameObject.GetComponent<IDamageable>();
            recieveDamage?.TakeDamage(damage);
        }
    }

    public override void Init()
    {

    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public override void Release()
    {
        base.Release();

        rb.velocity = Vector3.zero;
    }
}
