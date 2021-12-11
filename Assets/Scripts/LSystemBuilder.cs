using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LSystemBuilder : MonoBehaviour
{
    public Rule[] rules;

    public string axiom;

    [Range(0,10)]
    public int iterationLimit = 1;

    private void Start()
    {
        //Debug.Log(Generate());
    }

    public string Generate(string word = null)
    {
        if(word == null)
        {
            word = axiom;
        }

        return GenerateUtil(word);
    }

    private string GenerateUtil(string word, int currentIndex = 0)
    {
        if(currentIndex >= iterationLimit)
        {
            return word;
        }

        StringBuilder stringBuilder = new StringBuilder();

        foreach(char c in word)
        {
            stringBuilder.Append(c);

            ProcessRulesUtil(stringBuilder, c, currentIndex);
        }

        return stringBuilder.ToString();
       
    }

    private void ProcessRulesUtil(StringBuilder stringBuilder, char c, int currentIndex)
    {
        foreach(Rule rule in rules)
        {
            if(rule.letter == c.ToString())
            {
                stringBuilder.Append(GenerateUtil(rule.GetResult(), currentIndex + 1));
            }
        }
    }
}
