﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    #region Private Variables

    [Header("Room Canvases")]
    // References to the Canvases in the Game
    [SerializeField]
    private Canvas joinOrCreateRoomCanvas;

    [SerializeField]
    private Canvas currentRoomCanvas;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleJoinOrCreateRoomCanvas(bool toggleValue)
    {
        joinOrCreateRoomCanvas.enabled = toggleValue;
    }
    
    public void ToggleCurrentRoomCanvas(bool toggleValue)
    {
        currentRoomCanvas.enabled = toggleValue;
    }
}
