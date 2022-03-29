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

using Numitor.SDK.Model.AuthorModel;
using Numitor.SDK.DAO.LoginDao;

public class GameManager : MonoBehaviour
{

    [SerializeField] Button ButtonAnswer0;
    [SerializeField] Button ButtonAnswer1;
    [SerializeField] Button ButtonAnswer2;
    [SerializeField] Button ButtonAnswer3;
    Button[] AnswerButtons;
    [SerializeField] TextMeshProUGUI QuestionPlaceholder;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder0;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder1;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder2;
    [SerializeField] TextMeshProUGUI AnswerPlaceholder3;
    [SerializeField] TextMeshProUGUI PointsPlaceholder;

    [SerializeField] InputField EmailInputField;
    [SerializeField] InputField PasswordInputField;
    // HTTP request related variables
    string baseUrl = "https://uhx1g7zs22.execute-api.ca-central-1.amazonaws.com/default/numitor";
    QuestionHttpDao questionHttpDao = null;
    AnswerHttpDao answerHttpDao = null;
    LoginHttpDao loginHttpDao = null;

    // Questions/Answers related variables
    public static Question question;
    public static Answer answer;
    public static int questionQuantity;
    public static int currentQuestionNumber;

    // Points related variables
    public static int points;


    // Timer related variables
    public static bool timerIsRunning;
    public static float timeRemaining;



    void Start()
    {
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

        points = 0;
        currentQuestionNumber = 1;
        timerIsRunning = false;
        timeRemaining = 3;

        SelectRandomQuestion();
    }

    public async void SelectRandomQuestion()
    {
        if (currentQuestionNumber == questionQuantity + 1)
        {
            ShowEndScreen();
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

    public static void ShowQuestionSelect()
    {
        SceneManager.LoadScene("QuizSelect");
    }

    public void ShowEndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }
    public void ShowHomeScreen()
    {
        SceneManager.LoadScene("Homepage");
    }

    public async void Login()
    {
        Author author = new Author(EmailInputField.text, PasswordInputField.text);

        if (loginHttpDao == null)
            loginHttpDao = new LoginHttpDao(baseUrl + "/login");
        string response = await loginHttpDao.Get(author);

        if (response != null)
        {
            Debug.Log("Success, the redirect will be implemented");
        }
        else
        {
            Debug.Log("Unsuccessful login, a error message will be implemented");
        }

    }

}
