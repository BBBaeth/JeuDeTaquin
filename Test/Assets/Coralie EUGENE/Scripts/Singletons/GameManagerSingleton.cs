using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class GameManagerSingleton : MonoBehaviour
{
    [SerializeField] bool _isPersistent = true;
    [HideInInspector] public static GameManagerSingleton Instance { get { return _instance; } }
    private static GameManagerSingleton _instance;

    [Space(10)]
    [Header("Android Settings")]
    public List<Sprite> androidLogoParts;
    [Space(10)]
    [Header("Apple Settings")]
    public List<Sprite> appleLogoParts;

    private List<Sprite> spritesList;
    [HideInInspector] public Dictionary<TaquinCell, bool> cellIsCorrect = new Dictionary<TaquinCell, bool>();

    public UnityEvent modifiedTaquin;

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
#if PLATFORM_ANDROID || UNITY_EDITOR
        spritesList = androidLogoParts;
#elif UNITY_IOS
        spritesList = appleLogoParts;
#endif

        modifiedTaquin = new UnityEvent();
    }

    void Start()
    {
        // check for victory every time a change was made

        modifiedTaquin.AddListener(CheckVictory);
    }

    private void CheckVictory()
    {
        if (AllCellsCorrect())
        {
            // player wins !
        }
    }

    private bool AllCellsCorrect()
    {
        // is there one cell that does not have _isCorrect to true ?

        foreach (bool isCorrect in cellIsCorrect.Values)
        {
            if (!isCorrect)
            {
                return (false);
            }
        }

        // all correct !

        return (true);
    }


    // returns matching sprite in dictionnary for given index
    public Sprite GetSpriteAtIndex(int _id)
    {
        if (spritesList.Count > _id)
        {
            return (spritesList[_id]);
        }
        return (null);
    }

}
