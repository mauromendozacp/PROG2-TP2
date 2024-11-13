using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePoolController : MonoBehaviour
{
    [SerializeField] private ItemProjectile[] projectilePrefabs = null;
    [SerializeField] private Transform poolHolder = null;

    private Dictionary<PROJECTILE_TYPE, ObjectPool<ItemProjectile>> projectilePools = null;

    private void Start()
    {
        projectilePools = new Dictionary<PROJECTILE_TYPE, ObjectPool<ItemProjectile>>();
        for (int i = 0; i < projectilePrefabs.Length; i++)
        {
            ItemProjectile projectile = projectilePrefabs[i];
            projectilePools[projectile.Type] = new ObjectPool<ItemProjectile>(
                () =>
                {
                    return CreateProjectile(projectile.Type);
                }, 
                GetProjectile, ReleaseProjectile, DestroyProjectile);
        }
    }

    public ItemProjectile GetProjectileItem(PROJECTILE_TYPE id)
    {
        return projectilePools[id].Get();
    }

    private ItemProjectile CreateProjectile(PROJECTILE_TYPE id)
    {
        ItemProjectile projectilePrefab = projectilePrefabs.ToList().Find(p => p.Type == id);
        if (projectilePrefab != null)
        {
            ItemProjectile projectileItem = Instantiate(projectilePrefab, poolHolder);
            projectileItem.Init();
            projectileItem.onRelease = () => projectilePools[id].Release(projectileItem);

            return projectileItem;
        }

        return null;
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
