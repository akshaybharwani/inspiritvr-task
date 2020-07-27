using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Proyecto26;
using TMPro;
using UnityEngine;

public class QuestionsManager : MonoBehaviour
{
    #region Private Variables

    [Header("Question UI Reference")] 
    [SerializeField]
    private TextMeshProUGUI questionText;
    
    [Header("Options UI Reference")] 
    [SerializeField]
    private Option[] options;
    
    [SerializeField] 
    private string firebaseDBURL;
    
    private QuestionCollection _questionCollection = new QuestionCollection();

    private int _currentQuestionIndex = 0;

    private Question _currentQuestion;

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the Question Data in the beginning
        RetrieveFromDatabase();
    }
    
    private void RetrieveFromDatabase()
    {
        RestClient.Get<QuestionCollection>(firebaseDBURL).
            Then(response =>
        {
            _questionCollection = response;
            
            SetCurrentQuestionFields();
        });
    }

    public void IsOptionCorrect(int optionNumberSelected)
    {
        if (_currentQuestion.answer == optionNumberSelected)
        {
            PhotonNetwork.LocalPlayer.AddScore(1);
        }
    }

    /// <summary>
    /// Which sets the Question Text and the Option fields based
    /// on the Current Question
    /// </summary>
    private void SetCurrentQuestionFields()
    {
        // Set the Current Question object
        _currentQuestion = _questionCollection.questions[_currentQuestionIndex];

        // Set the Question Text
        questionText.text = _currentQuestion.question;
        
        // Loop through Options Array to set Option fields
        for (int i = 0; i < options.Length; i++)
        {
            options[i].SetOptionFields(_currentQuestion.options[i], i);
        }
    }
}
