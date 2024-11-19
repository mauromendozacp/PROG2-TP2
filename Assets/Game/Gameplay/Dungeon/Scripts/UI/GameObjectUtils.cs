
using UnityEngine;


public static class GameObjectUtils
{
    public static GameObject FindObject(string name)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
