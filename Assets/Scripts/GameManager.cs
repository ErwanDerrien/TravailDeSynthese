using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using Numitor.SDK.Model.QuestionModel;
using Numitor.SDK.DAO.QuestionDao;

using Numitor.SDK.Model.AnswerModel;
using Numitor.SDK.DAO.AnswerDao;

public class GameManager : MonoBehaviour
{

    [SerializeField] Button ButtonAnswer0;
    [SerializeField] Button ButtonAnswer1;
    [SerializeField] Button ButtonAnswer2;
    [SerializeField] Button ButtonAnswer3;
    [SerializeField] TextMeshProUGUI QuestionPlaceholder;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder0;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder1;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder2;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder3;
    Button[] AnswerButtons;
    [SerializeField] TextMeshProUGUI PointsPlaceholder;




    // HTTP request related variables
    string baseUrl = "https://uhx1g7zs22.execute-api.ca-central-1.amazonaws.com/default/numitor";
    QuestionHttpDao questionHttpDao = null;
    AnswerHttpDao answerHttpDao = null;

    // Questions/Answers related variables
    public static Question question;
    public static Answer answer;
    public static int questionQuantity;
    public static int currentQuestionNumber;

    // Points related variables
    public static int points;


    // Timer related variables
    public static bool timerIsRunning = false;
    public static float timeRemaining = 3;



    void Start()
    {
        points = 0;
        AnswerButtons = new Button[4];
        AnswerButtons[0] = ButtonAnswer0;
        AnswerButtons[1] = ButtonAnswer1;
        AnswerButtons[2] = ButtonAnswer2;
        AnswerButtons[3] = ButtonAnswer3;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            Debug.Log("Timer is running");

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 3;
                timerIsRunning = false;

                SelectRandomQuestion();
            }
        }
    }

    public void SelectQuestionQuantity(int quantity)
    {
        questionQuantity = quantity;
        currentQuestionNumber = 1;
        SelectRandomQuestion();
    }

    public async void SelectRandomQuestion()
    {

        Debug.Log("currentQuestionNumber: " + currentQuestionNumber);

        if (currentQuestionNumber == questionQuantity)
        {
            showEndScreen();
            return;
        }

        currentQuestionNumber += 1;


        if (questionHttpDao == null)
            questionHttpDao = new QuestionHttpDao(baseUrl + "/question");
        question = await questionHttpDao.Get();

        QuestionPlaceholder.text = question.question;

        AnswerPlaceholder0.text = question.answers[0];
        AnswerPlaceholder1.text = question.answers[1];
        AnswerPlaceholder2.text = question.answers[2];
        AnswerPlaceholder3.text = question.answers[3];

        for (int i = 0; i < 4; i = i + 1)
        {
            ColorBlock colors = AnswerButtons[i].colors;
            colors.normalColor = Color.blue;
            AnswerButtons[i].colors = colors;
            AnswerButtons[i].interactable = true;
        }

        SceneManager.LoadScene("QuestionsPage");

    }

    public async void SelectAnswer(int index)
    {

        if (answerHttpDao == null)
            answerHttpDao = new AnswerHttpDao(baseUrl + "/answer");
        answer = await answerHttpDao.Get(question.id);

        if (answer.index == index)
        {
            points += question.points;
            PointsPlaceholder.text = "Points : " + points;

            ColorBlock colors = AnswerButtons[index].colors;
            colors.disabledColor = Color.green;
            AnswerButtons[index].colors = colors;
        }
        else
        {
            ColorBlock colors = AnswerButtons[index].colors;
            colors.disabledColor = Color.red;
            AnswerButtons[index].colors = colors;
        }

        for (int i = 0; i < 4; i = i + 1)
        {
            AnswerButtons[i].interactable = false;
            if (i == index)
            {
                continue;
            }
            ColorBlock colors = AnswerButtons[i].colors;
            colors.disabledColor = Color.grey;
            AnswerButtons[i].colors = colors;
        }

        SceneManager.LoadScene("QuestionsPage");

        timerIsRunning = true;
    }

    public void showEndScreen()
    {
        Debug.Log("Will be implemented later");
    }
}
