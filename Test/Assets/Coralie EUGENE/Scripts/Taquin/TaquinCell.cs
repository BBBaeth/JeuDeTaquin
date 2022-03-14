using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaquinCell : MonoBehaviour
{
    public Sprite          _currentImage;
    [SerializeField]public int   _cellValue;
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
        // Add this cell to GameManager dictionnary 

        UpdateCellStatusInManager();
        TaquinGameplaySingleton.Instance.modifiedTaquin.AddListener(UpdateSprite);
    }

    void OnDestroy()
    {
       TaquinGameplaySingleton.Instance.modifiedTaquin.RemoveListener(UpdateSprite);
    }

    // Update is called once per frame
    void UpdateCellStatusInManager()
    {
        if (!TaquinGameplaySingleton.Instance.cellIsCorrect.ContainsKey(this))
        {
            TaquinGameplaySingleton.Instance.cellIsCorrect.Add(this, _isCorrect);
        }
        else
        {
            TaquinGameplaySingleton.Instance.cellIsCorrect[this] = _isCorrect;
        }
    }

    void isEmpty()
    {
        TaquinGameplaySingleton.Instance._emptyCell = this;
        _ImageField.enabled = false;
    }

    public void UpdateSprite()
    {
        _ImageField.sprite = _currentImage;

        if (this == TaquinGameplaySingleton.Instance._emptyCell)
        {
            isEmpty();
        }
        else
        {
            _ImageField.enabled = true;
        }
        if (_currentImage == TaquinGameplaySingleton.Instance.spritesList[_cellValue])
        {
            _isCorrect = true;
        }
        else
        {
            _isCorrect = false;
        }
        UpdateCellStatusInManager();
    }
}
