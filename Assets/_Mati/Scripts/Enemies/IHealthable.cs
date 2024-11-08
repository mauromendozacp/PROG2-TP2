using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealtheable
{
    public int GetCurrentHealth();
    public int GetMaxHealth();
}
