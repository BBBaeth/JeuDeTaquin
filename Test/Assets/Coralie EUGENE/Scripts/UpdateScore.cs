using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UpdateScore : MonoBehaviour
{
    public Text textField;

    void Start()
    {
        GameManagerSingleton.Instance.victoryEvent.AddListener(UpdateText);
    }

    void OnEnable()
    {
        UpdateText();
    }

    void OnDisable()
    {
        GameManagerSingleton.Instance.victoryEvent.RemoveListener(UpdateText);
    }

    public void UpdateText()
    {
        textField.text = GameManagerSingleton.Instance.score.ToString();
    }
}
