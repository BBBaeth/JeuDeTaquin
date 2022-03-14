using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManagerSingleton : MonoBehaviour
{
    [SerializeField] bool _isPersistent = true;
    [HideInInspector] public static GameManagerSingleton Instance { get { return _instance; } }
    private static GameManagerSingleton _instance;

    [Tooltip("Difficulty represents the number of moves made to randomize the taquin")]
    public int _difficulty = 10;
    public int score = 0;
    public int highScore = 0;

    [HideInInspector] public UnityEvent victoryEvent;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Preventing creation of duplicate singleton");
            Destroy(this.gameObject);
        }
        _instance = this;
        if (_isPersistent)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        victoryEvent = new UnityEvent();

        if (!PlayerPrefs.HasKey("score"))
        {
            PlayerPrefs.SetInt("score", 0);
            highScore = 0;
        }
        else
        {
            highScore = PlayerPrefs.GetInt("score");
        }

    }
    public void ResolvedTaquin()
    {
        // add score
        score += 1000;
        // generate new Taquin
        TaquinGameplaySingleton.Instance.mustGenerate = true;
        victoryEvent.Invoke();
    }

    public void EndGame()
    {
        // save score
        if (score > highScore)
        {
            PlayerPrefs.SetInt("score", score);
            highScore = PlayerPrefs.GetInt("score");
        }
        
        highScore = PlayerPrefs.GetInt("score");
        // cleaning variables

        score = 0;
        TaquinGameplaySingleton.Instance.cellIsCorrect.Clear();

    }


}
