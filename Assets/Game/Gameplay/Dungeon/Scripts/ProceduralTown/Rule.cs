using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS
{
    [CreateAssetMenu(menuName ="ProceduralTown/Rule")]
    public class Rules : ScriptableObject
    {
        public string letter;

        [SerializeField] private string[] results = null;
        [SerializeField] private bool randomResult = false;

        public string GetResult()
        {
            if(randomResult)
            {
                int randonIndex = UnityEngine.Random.Range(0, results.Length);
                return results[randonIndex];
            }
            return results[0];
        }
    }
}
