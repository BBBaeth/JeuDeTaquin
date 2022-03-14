using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TaquinGameplaySingleton : MonoBehaviour
{

    private static TaquinGameplaySingleton _instance;
    [HideInInspector] public static TaquinGameplaySingleton Instance { get { return _instance; } }

    [HideInInspector] public Dictionary<TaquinCell, bool> cellIsCorrect = new Dictionary<TaquinCell, bool>();
    [HideInInspector] public Dictionary<TaquinCell, Sprite> correctAnswers = new Dictionary<TaquinCell, Sprite>();
    [Space(10)]
    [Header("Android Settings")]
    public List<Sprite> androidLogoParts;
    [Space(10)]
    [Header("Apple Settings")]
    public List<Sprite> appleLogoParts;
    public List<Sprite> spritesList;
    [HideInInspector] public UnityEvent modifiedTaquin;
    public TaquinCell _emptyCell;
    public int       _emptyCellID;
    public bool       mustGenerate = true;

    public TaquinCell _selectedCell = null;

    public float _selectedHeight = 0.15f;


#region Initialization

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Preventing creation of duplicate singleton");
            Destroy(this.gameObject);
        }
        _instance = this;

#if PLATFORM_ANDROID || UNITY_EDITOR
        spritesList = androidLogoParts;
#elif UNITY_IOS
        spritesList = appleLogoParts;
#endif

        modifiedTaquin = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        modifiedTaquin.AddListener(CheckVictory);
    }

    void LateUpdate()
    {
        if (mustGenerate)
        {
            GenerateNewTaquin();
            mustGenerate = false;
        }
    }

#endregion
#region victory check
    private void CheckVictory()
    {
        if (AllCellsCorrect())
        {
            // player wins !
            GameManagerSingleton.Instance.ResolvedTaquin();
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
#endregion

#region input

    void Update()
    {
        DetectTouch();
    }

    private void DetectTouch()
    {
         if (Input.touchCount > 0)
         {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
    
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        hit.transform.gameObject.TryGetComponent<TaquinCell>(out TaquinCell _touchedCell);
                        if (_touchedCell && _touchedCell != _emptyCell)
                        {
                            _selectedCell = _touchedCell;
                            _selectedCell.IsSelected();
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    Ray ray2 = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit2;
    
                    if (Physics.Raycast(ray2, out hit2, 100))
                    {
                        hit2.transform.gameObject.TryGetComponent<TaquinCell>(out TaquinCell _touchedCell);
                        if (_touchedCell != null && _touchedCell == _emptyCell)
                        {
                            SwitchCells(_touchedCell, _selectedCell);
                            _selectedCell.IsReleased();
                            _selectedCell = null;
                        }
                    }
                    break;
                default:
                    break;
            }
         }
         else
         {
             _selectedCell.IsReleased();
             _selectedCell = null;
         }
    }

#endregion


    private void SwitchCells(TaquinCell empty, TaquinCell other)
    {

        // check if switch is possible (neighbour cells)

        bool moveUp = empty._cellValue > 2 ? true : false;
        bool moveDown = empty._cellValue < 6 ? true : false;
        bool moveLeft = empty._cellValue % 3 == 0 ? false : true;
        bool moveRight = (empty._cellValue + 1) % 3 == 0 ? false : true;

        if ((empty._cellValue + 3 == other._cellValue && moveDown) || (empty._cellValue - 3 == other._cellValue && moveUp)
        || (empty._cellValue + 1 == other._cellValue && moveRight) || (empty._cellValue - 1 == other._cellValue && moveLeft))
        {
            Sprite backup = empty._currentImage;
            empty._currentImage = other._currentImage;
            other._currentImage = backup;
            _emptyCell = other;
            modifiedTaquin.Invoke();
        }
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

#region GenerateNewTaquin

    public void GenerateNewTaquin()
    {
        List<Sprite> grid = spritesList;

        // make sure taquin is modified
        Random.InitState(Random.Range(200,300));
        grid = RandomizeList(GameManagerSingleton.Instance._difficulty, grid);


        // update renders
        for (int i = 0; i < grid.Count; i++)
        {
            foreach (TaquinCell cell in cellIsCorrect.Keys)
            {
                if (cell._cellValue == i)
                {
                    cell._currentImage = grid[i];
                    if (cell._cellValue == _emptyCellID)
                    {
                        _emptyCell = cell;
                    }
                }
            }
        }

        // call event
        if (modifiedTaquin != null)
        {
            modifiedTaquin.Invoke();
        }
    }
    public List<Sprite> RandomizeList(int iterationsNumber, List<Sprite> grid)
    {
        // -----
        // 0 1 2
        // 3 4 5
        // 6 7 8
        // -----

        // we always start with empty cell being the middle one
        int emptyCell = 4;
        bool randomResult;
        Sprite backup = grid[emptyCell];
        Random.InitState(Random.Range(0,100));

        for (int i = 0; i < iterationsNumber; i++)
        {
            // depending on row and column some movements are available
            bool moveUp = emptyCell > 2 ? true : false;
            bool moveDown = emptyCell < 6 ? true : false;
            bool moveLeft = emptyCell % 3 == 0 ? false : true;
            bool moveRight = (emptyCell + 1) % 3 == 0 ? false : true;

            randomResult = (Random.Range(0, 5) >= 3);

            // discard right or left
            if (moveRight && moveLeft)
            {
                moveRight = randomResult;
                moveLeft = !randomResult;
            }
            // discard up or down
            // we can use same random as anyway only one movement is made
            if (moveUp && moveDown)
            {
                moveUp = randomResult;
                moveDown = !randomResult;
            }
            int target = moveLeft == true ? -1 : +1; // horizontal move
            if (Random.Range(0, 5) >= 3)            // vertical move
            {
                target = moveUp == true ? -3 : +3;
            }

            // switch both sprites
            grid[emptyCell] = grid[emptyCell + target];
            grid[emptyCell + target] = backup;
            emptyCell = emptyCell + target;
            backup = grid[emptyCell];
        }
        _emptyCellID = emptyCell;

        return (grid);
    }

#endregion



}
