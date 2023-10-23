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
        public bool paramterValue;
    }

    public List<DialogueParameter> parameters = new List<DialogueParameter>();

    public void UpdateParameter(string parameter, bool value)
    {
       bool found = false;
       foreach(var kvp in parameters)
       {
            if(kvp.parameterKey == parameter)
            {
                kvp.paramterValue = value;
                found = true;
                break;
            }
       }

       if(!found)
       {
            Debug.Log("Dialogue Parameter not Found");
       }
    }

}
