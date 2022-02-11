using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] Button middle;
    [SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ___SelectQuestionQuantity(int quantity) {
        Debug.Log("Quantity : " + quantity);
    }
}
