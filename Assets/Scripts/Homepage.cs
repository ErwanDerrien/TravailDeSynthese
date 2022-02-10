using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Homepage : MonoBehaviour
{
    public static void LoadSceneSelect()
    {
        SceneManager.LoadScene("QuizSelect");
    }
}
