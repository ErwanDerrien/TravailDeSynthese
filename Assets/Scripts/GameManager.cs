using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UnityInitializer;

public class GameManager : MonoBehaviour
{

    [SerializeField] Button Button5;
    [SerializeField] Button Button10;
    [SerializeField] Button Button15;

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
