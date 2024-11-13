using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePoolController : MonoBehaviour
{
    [SerializeField] private ItemProjectile[] projectilePrefabs = null;
    [SerializeField] private Transform poolHolder = null;

    private List<ObjectPool<ItemProjectile>> projectilePools = null;

    private void Start()
    {
        projectilePools = new List<ObjectPool<ItemProjectile>>();
        for (int i = 0; i < projectilePrefabs.Length; i++)
        {
            projectilePools[i] = new ObjectPool<ItemProjectile>(
                () =>
                {
                    return CreateProjectile(i);
                }, 
                GetProjectile, ReleaseProjectile, DestroyProjectile);
        }
    }

    public ItemProjectile GetProjectileItem(int id)
    {
        return projectilePools[id].Get();
    }

    private ItemProjectile CreateProjectile(int id)
    {
        ItemProjectile projectileItem = Instantiate(projectilePrefabs[id], poolHolder);
        projectileItem.Init();
        projectileItem.onRelease = () => projectilePools[id].Release(projectileItem);

        return projectileItem;
    }

    private void GetProjectile(ItemProjectile projectile)
    {
        projectile.Get();
    }

    private void ReleaseProjectile(ItemProjectile projectile)
    {
        projectile.Release();
    }

    private void DestroyProjectile(ItemProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
