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
    
    [SerializeField] Button ButtonAnswer1;
    [SerializeField] Button ButtonAnswer2;
    [SerializeField] Button ButtonAnswer3;
    [SerializeField] Button ButtonAnswer4;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder1;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder2;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder3;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder4;

    int questionQuantity;

    string baseUrl = "https://uhx1g7zs22.execute-api.ca-central-1.amazonaws.com/default/numitor/";
    QuestionHttpDao questionHttpDao;

    Question question;

    void Start()
    {
        Debug.Log("Quantity : " + questionQuantity);
        questionHttpDao = new QuestionHttpDao(baseUrl + "/question");
        
        question = questionHttpDao.Get().Result;
        Debug.Log("question" + question);
    }

    void Update()
    {
        
    }

    public void SelectQuestionQuantity(int quantity)
    {
        Debug.Log("Quantity : " + quantity);
        questionQuantity = quantity;
        SceneManager.LoadScene("QuestionsPage");
    }



}
