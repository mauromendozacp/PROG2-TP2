using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private float initialForce = 0f;

    private Vector3 startPosition = Vector3.zero;
    private LayerMask targetLayer = default;
    private int damage = 0;

    private Rigidbody rb = null;

    private void Awake()
    {
        startPosition = transform.localPosition;

        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckLayerInMask(targetLayer, other.gameObject.layer))
        {
            IDamagable recieveDamage = other.gameObject.GetComponent<IDamagable>();
            recieveDamage?.Damage(damage);
        }
    }

    public void SetTargetLayer(LayerMask targetLayer)
    {
        this.targetLayer = targetLayer;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void StartFall()
    {
        rb.AddForce(-transform.up * initialForce, ForceMode.Force);
    }

    public void Restart()
    {
        transform.localPosition = startPosition;
        transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
