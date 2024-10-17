using System;
using System.Data;
using System.Text;
using UnityEngine;

namespace SVS
{
    public class LSystemGenerator : MonoBehaviour
    {
        public Rules[] rules;
        public string rootSentece;
       
        [Range(0,10)] public int iteractionLimit = 1;

        public bool randomIgnoreRuleModifier = true;
       
        [Range(0,1)] public float chanceToIgnoreRule = 0.3f;

        private void Start()
        {
            Debug.Log(GenerateSentence());    
        }
        public string GenerateSentence(string word = null)
        {
            if( word == null)
            {
                word = rootSentece;
            }
            return GrowRecursive(word);
        }

        private string GrowRecursive(string word, int iteractionIndex = 0)
        {
            if(iteractionIndex >= iteractionLimit)
            {
                return word;
            }

            StringBuilder newWord = new StringBuilder();

            foreach (var c in word)
            {
                newWord.Append(c);
                ProcessRulesRecursively(newWord, c, iteractionIndex);
            }

            return newWord.ToString();
        }

        private void ProcessRulesRecursively(StringBuilder newWord, char c, int iteractionIndex)
        {
            
            foreach(var rule in rules)
            {
                if(rule.letter == c.ToString())
                {
                    if(randomIgnoreRuleModifier)
                    {
                        if(UnityEngine.Random.value < chanceToIgnoreRule)
                        {
                            return;
                        }
                    }
                    newWord.Append(GrowRecursive(rule.GetResult(), iteractionIndex + 1));
                }
            }
        }
    }
}
