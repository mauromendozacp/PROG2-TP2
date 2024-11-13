using System;

using UnityEngine;

public abstract class ItemProjectile : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayer = default;

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
