using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Proyecto26;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsManager : MonoBehaviour
{
    #region Private Variables

    [Header("Question UI Reference")] 
    [SerializeField]
    private TextMeshProUGUI questionText;

    [SerializeField] 
    private TextMeshProUGUI questionNumberText;
    
    [Header("Options UI Reference")] 
    [SerializeField]
    private Option[] options;

    [Header("Timer")] 
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField] 
    private string firebaseDBURL;

    private GameManager _gameManager;
    
    // A reference to all the Questions from Database
    private QuestionCollection _questionCollection = new QuestionCollection();

    private int _numberOfQuestions;

    // Storing reference to the Current Question Index
    private int _currentQuestionIndex = 0;

    // A local reference to the Current Question
    private Question _currentQuestion;

    // Setting the time for a Question to be shown
    private float _questionTime = 10;

    private Color _normalButtonColor = new Color32(98, 70, 234, 255);
    
    private Color _correctOptionColor = new Color32(18, 162, 90, 255);

    // A reference to store the Selected Option by the Player
    private int _selectedOption = -1;

    // A check to make sure a Player can only select a Single Option per Question
    private bool _isAnswerSelectedFortheCurrentQuestion = false;
    
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Question Data in the beginning
        RetrieveFromDatabase();
        
        // Set Game Manager
        _gameManager = GameManager.Instance;
    }
    
    private void RetrieveFromDatabase()
    {
        RestClient.Get<QuestionCollection>(firebaseDBURL).
            Then(response =>
        {
            _questionCollection = response;

            // Set the number of Questions
            _numberOfQuestions = _questionCollection.questions.Length;
        });
    }

    private void IsOptionCorrect()
    {
        if (_currentQuestion.answer == _selectedOption)
        {
            PhotonNetwork.LocalPlayer.AddScore(1);
        }
        
        // Set the bool back to false for the next Question
        _isAnswerSelectedFortheCurrentQuestion = false;
        
        // Set SelectedOption to default
        _selectedOption = -1;
    }

    /// <summary>
    /// Which sets the Question Text and the Option fields based
    /// on the Current Question
    /// </summary>
    public void SetCurrentQuestionFields()
    {
        // Set the Current Question object
        _currentQuestion = _questionCollection.questions[_currentQuestionIndex];

        // Set the Current Question Number in UI
        questionNumberText.text = "Question " + (_currentQuestionIndex + 1);
        
        // Set the Question Text
        questionText.text = _currentQuestion.question;
        
        // Loop through Options Array to set Option fields
        for (int i = 0; i < options.Length; i++)
        {
            options[i].SetOptionFields(_currentQuestion.options[i], i);
        }
    }

    public IEnumerator StartQuestionTimer()
    {
        while (_questionTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            
            // Reduce the question Time
            _questionTime--;
            
            // Update the Time in the App
            timerText.text = _questionTime.ToString();
        }

        StartCoroutine(WaitToShowAnswerAndThenNextQuestion(5f));
    }

    private void NextQuestion()
    {
        // Set Question Time back to default;
        _questionTime = 10;

        // Increase the Current Question Index
        _currentQuestionIndex++;
        
        if (!IsCurrentQuestionLast())
        {
            Debug.Log(_currentQuestionIndex);
        
            // Set the Current Question fields
            SetCurrentQuestionFields();
        
            // Start Timer again
            StartCoroutine(StartQuestionTimer());
        }
        else
        {
            _gameManager.CheckEndOfGame();
        }
    }

    private bool IsCurrentQuestionLast()
    {
        return !(_currentQuestionIndex < _numberOfQuestions);
    }

    private IEnumerator WaitToShowAnswerAndThenNextQuestion(float timeToWait)
    {
        var correctOptionButtonImage = options[_currentQuestion.answer].GetComponent<Image>();
        
        // Check if the answer Selected by the Player is correct
        IsOptionCorrect();
        
        // Show the Option which is correct
        ToggleOptionButtonColor(correctOptionButtonImage, true);
        
        yield return new WaitForSeconds(timeToWait);
        
        // Change the Button color back to Default
        ToggleOptionButtonColor(correctOptionButtonImage, false);

        // If the Question is not the last, go to the next one
        if (!IsCurrentQuestionLast())
            NextQuestion();
    }

    /// <summary>
    /// Set the Color to the Option which is Correct
    /// </summary>
    /// <param name="image"></param>
    private void ToggleOptionButtonColor(Image image, bool toggleValue)
    {
        image.color = toggleValue ? _correctOptionColor : _normalButtonColor;
    }

    public void SetSelectedOption(int optionNumber)
    {
        if (_isAnswerSelectedFortheCurrentQuestion) return;
        
        _selectedOption = optionNumber;
        _isAnswerSelectedFortheCurrentQuestion = true;
    }
}
