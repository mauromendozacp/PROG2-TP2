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

    public PROJECTILE_TYPE Type => type;

    public Action onRelease = null;

    public abstract void Init();

    public virtual void Get()
    {
        gameObject.SetActive(true);
    }

    public virtual void Release()
    {
        gameObject.SetActive(false);
    }
}
