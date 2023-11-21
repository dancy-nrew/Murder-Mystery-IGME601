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
}
