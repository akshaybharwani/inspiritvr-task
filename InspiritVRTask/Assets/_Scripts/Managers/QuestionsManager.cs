using Proyecto26;
using UnityEngine;

public class QuestionsManager : MonoBehaviour
{
    #region Private Variables

    [SerializeField] 
    private string firebaseDBURL;

    private QuestionCollection _questionCollection = new QuestionCollection();

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
