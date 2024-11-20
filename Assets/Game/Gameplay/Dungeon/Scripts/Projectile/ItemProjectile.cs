using System;

using UnityEngine;

public enum PROJECTILE_TYPE
{
    NONE,
    ARROW,
    METEORS
}

public abstract class ItemProjectile : MonoBehaviour
{
    [SerializeField] protected PROJECTILE_TYPE type = default;
    [SerializeField] protected LayerMask targetLayer = default;

    protected bool released = false;
    public Action onRelease = null;

    public PROJECTILE_TYPE Type => type;

    public abstract void Init();

    public virtual void Get()
    {
        released = false;
        gameObject.SetActive(true);
    }

    public virtual void Release()
    {
        released = true;
        gameObject.SetActive(false);
    }
}
