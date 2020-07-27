using System.Collections;
using System.Collections.Generic;
using Models;
using Proyecto26;
using UnityEngine;

public class QuestionsManager : MonoBehaviour
{
    #region Private Variables

    [SerializeField] 
    private string firebaseDBURL;

    private QuestionCollection _questionCollection = new QuestionCollection();
    private Question _question = new Question();

    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        RetrieveFromDatabase();
    }
    
    private void RetrieveFromDatabase()
    {
        RestClient.Get<QuestionCollection>(firebaseDBURL).
            Then(response =>
        {
            _questionCollection = response;
        });
    }
}
