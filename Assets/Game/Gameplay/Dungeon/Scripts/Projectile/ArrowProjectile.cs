using UnityEngine;

public class ArrowProjectile : ItemProjectile
{
    [SerializeField] private MeshFilter meshFilter = null;

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
            IDamagable recieveDamage = collision.gameObject.GetComponent<IDamagable>();
            recieveDamage?.Damage(damage);
        }

        if (!released)
        {
            onRelease?.Invoke();
        }
    }

    public override void Init()
    {

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

    public void SetMesh(Mesh mesh)
    {
        meshFilter.mesh = mesh;
    }

    public override void Release()
    {
        base.Release();

        rb.velocity = Vector3.zero;
    }
}
