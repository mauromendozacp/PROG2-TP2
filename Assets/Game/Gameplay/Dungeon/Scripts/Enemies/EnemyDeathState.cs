using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : IEnemyState
{
    Enemy _controller;

    public EnemyDeathState(Enemy controller)
    {
        _controller = controller;
    }
    public void EnterState()
    {
        _controller.SetAnimator("Die", true);
        _controller.ResetAgentDestination();
        //_controller.Collider.enabled = false;     // Desactivar colisiones

        // Opcional: Activar destrucción tras animación
        //_controller.StartCoroutine(DestroyAfterDelay(_controller, 3f)); // 3 segundos de delay
    }

    public void Execute() { }

    /*
    private IEnumerator DestroyAfterDelay(Enemy controller, float delay)
    {
        yield return new WaitForSeconds(delay);
        Object.Destroy(controller.gameObject);
    }
    */
}
