using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Procedural/Rule")]
public class Rule : ScriptableObject
{
    public string letter;
    [SerializeField]
    private string[] results = null;

    [SerializeField]
    private bool randomRulePerIteration = false;

    public string GetResult()
    {
        if(randomRulePerIteration)
        {
            int randIndex = UnityEngine.Random.Range(0, results.Length);
            return results[randIndex];
        }
        return results[0];

    }
}
