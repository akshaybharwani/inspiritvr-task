using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Option : MonoBehaviour
{
    #region Private Variables

    [Header("Option Text")] 
    [SerializeField]
    private TextMeshProUGUI optionText;

    private int _optionNumber;

    private QuestionsManager _questionsManager;

    #endregion

    private void Start()
    {
        // Set reference to QuestionsManager Component
        _questionsManager = FindObjectOfType<QuestionsManager>();
    }

    /// <summary>
    /// Set Option fields based on the current Question
    /// </summary>
    /// <param name="optionTextFromDB"></param>
    /// <param name="optionNumberFromDB"></param>
    public void SetOptionFields(string optionTextFromDB, int optionNumberFromDB)
    {
        optionText.text = optionTextFromDB;
        _optionNumber = optionNumberFromDB;
    }

    /// <summary>
    /// Which checks If the Option selected is correct or not
    /// </summary>
    public void onClick_OptionButton()
    {
        _questionsManager.SetSelectedOption(_optionNumber);
    }
}
