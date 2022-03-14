using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpdateHighestScore : MonoBehaviour
{
    public Text textField;
    void Start()
    {
        textField.text = GameManagerSingleton.Instance.highScore.ToString();
    }

    void OnEnable()
    {
        if (GameManagerSingleton.Instance != null)
        {
            textField.text = GameManagerSingleton.Instance.highScore.ToString();
        }
    }

}
