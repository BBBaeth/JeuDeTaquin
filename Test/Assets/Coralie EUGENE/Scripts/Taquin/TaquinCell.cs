using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaquinCell : MonoBehaviour
{
    public Sprite          _currentImage;
    [SerializeField] int   _cellValue;
    [SerializeField] Image _ImageField;

    [HideInInspector] public bool _isCorrect = false;

    void Awake()
    {
        if (_ImageField == null)
        {
            Debug.LogError("Taquin cell " + gameObject.name + " is missing its image field");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add this cell to gameManager dictionnary 
        
        UpdateCellStatusInManager();
    }

    // Update is called once per frame
    void UpdateCellStatusInManager()
    {
        if (!GameManagerSingleton.Instance.cellIsCorrect.ContainsKey(this))
        {
            GameManagerSingleton.Instance.cellIsCorrect.Add(this, _isCorrect);
        }
        else
        {
            GameManagerSingleton.Instance.cellIsCorrect[this] = _isCorrect;
        }
    }
}
