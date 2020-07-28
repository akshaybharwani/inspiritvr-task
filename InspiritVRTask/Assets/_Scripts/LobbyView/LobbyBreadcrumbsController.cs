using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBreadcrumbsController : MonoBehaviour
{
    #region Private Variables

    [Header("Label Image References")] 
    [SerializeField]
    private Image playerLoginLabelImage;
    
    [SerializeField]
    private Image createJoinRoomLabelImage; 
    
    [SerializeField]
    private Image allPlayersLabelImage;

    // Color values to be used
    private Color activeColor = new Color32(98, 70, 234, 255);
    private Color inActiveColor = new Color32(34, 34, 34, 255);

    #endregion

    /// <summary>
    /// Changes colors of the Create/Join Room Label
    /// </summary>
    /// <param name="toggleValue"></param>
    public void ToggleCreateJoinRoomLabel(bool toggleValue)
    {
        
        createJoinRoomLabelImage.color = toggleValue ? activeColor : inActiveColor;
    }

    /// <summary>
    /// Changes colors of the All Players Label
    /// </summary>
    /// <param name="toggleValue"></param>
    public void ToggleAllPlayersLabel(bool toggleValue)
    {
        allPlayersLabelImage.color = toggleValue ? activeColor : inActiveColor;
    }
}
