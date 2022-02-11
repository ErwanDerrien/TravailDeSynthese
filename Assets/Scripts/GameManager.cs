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

    int questionQuantity;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Quantity : " + questionQuantity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectQuestionQuantity(int quantity) {
        Debug.Log("Quantity : " + quantity);
        questionQuantity = quantity;
        SceneManager.LoadScene("QuestionsPage");
    }

}
