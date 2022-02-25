using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Numitor.SDK.Model.QuestionModel;
using Numitor.SDK.DAO.QuestionDao;

public class GameManager : MonoBehaviour
{

    [SerializeField] Button Button5;
    [SerializeField] Button Button10;
    [SerializeField] Button Button15;

    [SerializeField] Button ButtonAnswer0;
    [SerializeField] Button ButtonAnswer1;
    [SerializeField] Button ButtonAnswer2;
    [SerializeField] Button ButtonAnswer3;
    [SerializeField] TextMeshProUGUI QuestionPlaceholder;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder0;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder1;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder2;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder3;

    int questionQuantity;

    string baseUrl = "https://uhx1g7zs22.execute-api.ca-central-1.amazonaws.com/default/numitor";
    QuestionHttpDao questionHttpDao = null;

    Question question;

    void Start()
    {
        Debug.Log("Bonjour Dana");
        Debug.Log(AnswerPlaceholder0.text);
    }

    void Update()
    {

    }

    public async void SelectQuestionQuantity(int quantity)
    {
        questionQuantity = quantity;
        SceneManager.LoadScene("QuestionsPage");

        if (questionHttpDao == null)
            questionHttpDao = new QuestionHttpDao(baseUrl + "/question");
        question = await questionHttpDao.Get();

        QuestionPlaceholder.text = question.question;

        
        
        AnswerPlaceholder0.text = question.answers[0];
        AnswerPlaceholder0.ForceMeshUpdate(true);
        AnswerPlaceholder1.text = question.answers[2];
        AnswerPlaceholder2.text = question.answers[2];
        AnswerPlaceholder3.text = question.answers[3];
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "QuestionsPage") {
            AnswerPlaceholder0.text = "-Loading-";
            AnswerPlaceholder1.text = "-Loading-";
            AnswerPlaceholder2.text = "-Loading-";
            AnswerPlaceholder3.text = "-Loading-";
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    public void SelectAnswer(int index)
    {
        Debug.Log(index);
    }

}
