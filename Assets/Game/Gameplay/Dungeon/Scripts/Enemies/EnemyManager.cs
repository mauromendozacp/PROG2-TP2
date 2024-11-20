using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject _panelInventory;
    bool _isPanelInventoryActive = false;
    public static EnemyManager Instance { get; private set; }

    private List<Enemy> _enemies = new List<Enemy>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        if(_panelInventory == null) _panelInventory = GameObjectUtils.FindObject("PanelInventory");
    }
    

    public void RegisterEnemy(Enemy enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
    }

    public void OnPlayerDefeated()
    {
        foreach (var enemy in _enemies)
        {
            enemy.VictoryAgainstPlayer();
        }
    }

    public void OnPlayerVictory()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Die();
        }
    }

    public void TogglePause()
    {
        foreach (var enemy in _enemies)
        {
            enemy.TogglePause();
        }
    }

    
    private void Update()
    {
        if(!_isPanelInventoryActive && _panelInventory.activeInHierarchy)
        {
            _isPanelInventoryActive = true;
            TogglePause();
        }
        else if(_isPanelInventoryActive && !_panelInventory.activeInHierarchy)
        {
            _isPanelInventoryActive = false;
            TogglePause();
        }
    }
}
