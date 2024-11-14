using UnityEngine;

public class MeteorsProjectile : ItemProjectile
{
    [SerializeField] private Meteor[] meteors = null;
    [SerializeField] private ParticleSystem circleParticle = null;
    [SerializeField] private float duration = 0f;

    public override void Init()
    {
        for (int i = 0; i < meteors.Length; i++)
        {
            meteors[i].SetTargetLayer(targetLayer);
        }
    }

    public void SetDamage(int damage)
    {
        for (int i = 0; i < meteors.Length; i++)
        {
            meteors[i].SetDamage(damage);
        }
    }

    private void CallReleasePool()
    {
        onRelease?.Invoke();
    }

    public override void Get()
    {
        base.Get();

        for (int i = 0; i < meteors.Length; i++)
        {
            meteors[i].StartFall();
        }

        Invoke("CallReleasePool", duration);
        circleParticle.Play();
    }

    public override void Release()
    {
        base.Release();

        for (int i = 0; i < meteors.Length; i++)
        {
            meteors[i].Restart();
        }
    }
}
