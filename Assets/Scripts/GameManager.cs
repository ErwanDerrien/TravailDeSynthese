
using System;
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
    [SerializeField] TextMeshProUGUI LoginFeedback;
    [SerializeField] Button SubmissionButton;
    [SerializeField] TextMeshProUGUI SubmissionFeedback;
    [SerializeField] InputField QuestionInputField;
    [SerializeField] InputField PointsInputField;
    [SerializeField] InputField Answer1InputField;
    [SerializeField] InputField Answer2InputField;
    [SerializeField] InputField Answer3InputField;
    [SerializeField] InputField Answer4InputField;
    [SerializeField] Toggle Toggle1;
    [SerializeField] Toggle Toggle2;
    [SerializeField] Toggle Toggle3;
    [SerializeField] Toggle Toggle4;



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
        answer = null;
        var task = answerHttpDao.Get(question.id).Wait();
        answer = task.Result;

        Debug.Log("bhay: " + answer);

        if (answer.index == index)
        {
            points += Int32.Parse(question.points);
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


    public void ShowAuthorLogin() {
        SceneManager.LoadScene("AuthorLogin");
    }

    public async void Login()
    {
        string email = EmailInputField.text;
        string password = PasswordInputField.text;

        if (email == "" || password == "")
        {
            LoginFeedback.text = "Veuillez remplir les champs avant de les soumettre";
            return;
        }

        Author author = new Author(email, password);

        if (loginHttpDao == null)
            loginHttpDao = new LoginHttpDao(baseUrl + "/login");
        string response = await loginHttpDao.Get(author);

        if (response != null)
        {
            Debug.Log("Success, redirect incoming...");
            SceneManager.LoadScene("AddQuestion");
        }
        else
        {
            LoginFeedback.text = "Aucun compte ne correspond aux coordonnées";
        }

    }
    public async void SubmitQuestion()
    {
        SubmissionFeedback.text = " En cours d'envoi...";

        string questionText = QuestionInputField.text;
        string pointsText = PointsInputField.text;
        string answerText0 = Answer1InputField.text;
        string answerText1 = Answer2InputField.text;
        string answerText2 = Answer3InputField.text;
        string answerText3 = Answer4InputField.text;

        // Answer submission preparation
        bool toggle0 = Toggle1.isOn;
        bool toggle1 = Toggle2.isOn;
        bool toggle2 = Toggle3.isOn;
        bool toggle3 = Toggle4.isOn;

        bool[] toggles = new bool[4];
        toggles[0] = toggle0;
        toggles[1] = toggle1;
        toggles[2] = toggle2;
        toggles[3] = toggle3;

        int answerIndex = -1;

        for (int i = 0; i < 4; i++)
        {
            if (toggles[i])
            {
                if (answerIndex != -1)
                {
                    SubmissionFeedback.text = "Please only select one checkbox fo the correct answer";
                    return;
                }
                answerIndex = i;
            }
        }

        Guid uuid = Guid.NewGuid();

        // Question submission

        string[] answers = new string[4];
        answers[0] = answerText0;
        answers[1] = answerText1;
        answers[2] = answerText2;
        answers[3] = answerText3;

        Question questionToSubmit = new Question(uuid.ToString(), questionText, answers, pointsText);
        if (questionHttpDao == null)
            questionHttpDao = new QuestionHttpDao(baseUrl + "/question");
        string questionResponse = await questionHttpDao.Create(questionToSubmit);

        if (questionResponse != null)
        {
            SubmissionFeedback.text = "Succès, la question a bien été soumise";
        }
        else
        {
            SubmissionFeedback.text = "Une erreur innatendu est survenue durant la soumission de la question...";
            return;
        }

        // Answer submission
        Answer answerToSubmit = new Answer(questionResponse, answerIndex);

        if (answerHttpDao == null)
            answerHttpDao = new AnswerHttpDao(baseUrl + "/answer");
        string answerResponse = await answerHttpDao.Create(answerToSubmit);

        if (questionResponse != null)
        {
            SubmissionFeedback.text = "Succès, la question a bien été soumise";
        }
        else
        {
            SubmissionFeedback.text = "Une erreur innatendu est survenue durant la soumission de la réponse...";
            return;
        }

        Debug.Log("answerResponse: " + answerResponse);
    }

}
