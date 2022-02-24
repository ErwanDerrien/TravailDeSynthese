using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] Button Button5;
    [SerializeField] Button Button10;
    [SerializeField] Button Button15;
    
    [SerializeField] Button ButtonAnswer1;
    [SerializeField] Button ButtonAnswer2;
    [SerializeField] Button ButtonAnswer3;
    [SerializeField] Button ButtonAnswer4;
    [SerializeField] Text AnswerPlaceholder1;
    [SerializeField] Text AnswerPlaceholder2;
    [SerializeField] Text AnswerPlaceholder3;
    [SerializeField] Text AnswerPlaceholder4;

    int questionQuantity;

    void Start()
    {
        Debug.Log("Quantity : " + questionQuantity);
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
