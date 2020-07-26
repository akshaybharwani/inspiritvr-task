using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            // If there are no Instances already available
            if (_instance == null)
            {
                // Finds all the Objects of Type T
                T[] results = Resources.FindObjectsOfTypeAll<T>();

                // If the length of the Array is 0 ie, no such Objects exists
                if (results.Length == 0)
                {
                    Debug.Log("Results length is 0 of " + typeof(T));
                    return null;
                }

                // If the length is more than 1
                if (results.Length > 1)
                {
                    Debug.Log("Results length is 0 of " + typeof(T));
                    return null;
                }

                // If there is only 1 Object which exists of Type T
                _instance = results[0];
            }
            
            // Return the Object
            return _instance;
        }
    }
}
