using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    #region Private Variables

    [SerializeField] 
    private string gameVersion = "0.0.0";

    public string GameVersion => gameVersion;
    
    [SerializeField] 
    private string nickName = "InspiritVRQuizGame";

    public string NickName
    {
        get
        {
            // Adding a random number at the end of the name so as to 
            // differentiate between Server Name and Room Name
            int value = Random.Range(0, 9999);
            return nickName + value;
        }
    }

    #endregion

}
