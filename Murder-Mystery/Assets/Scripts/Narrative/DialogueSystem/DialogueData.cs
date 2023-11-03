using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu()]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public class DialogueParameter
    {
        public string parameterKey;
        public bool parameterValue;
    }

    [SerializeField]
    private bool bResetDataOnGameStart = false;

    public List<DialogueParameter> parameters = new List<DialogueParameter>();

    public void UpdateParameter(string parameter, bool value)
    {
       bool found = false;
       foreach(var kvp in parameters)
       {
            if(kvp.parameterKey == parameter)
            {
                kvp.parameterValue = value;
                found = true;
                break;
            }
       }

       if(!found)
       {
            Debug.Log("Dialogue Parameter not Found");
       }
    }

    public bool CheckCondition(string parameter, bool value)
    {
        //Debug.Log("Checking " + parameter + " is " + value);
        foreach (var kvp in parameters)
        {
            if (kvp.parameterKey == parameter)
            {
               return (kvp.parameterValue == value);
            }
        }

        Debug.Log("Dialogue Parameter not Found");
        return false;
        
    }

    // Reseting all parameter bools to false at start of game for debugging purposes
    private void Awake()
    {
        Debug.Log("Awakening dialogue data");
        if(bResetDataOnGameStart)
        {
            foreach(var kvp in parameters)
            {
                kvp.parameterValue = false;
            }
        }
    }
}
