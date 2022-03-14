using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.Events;
public class UpdatePreviewImages : MonoBehaviour
{
    public List<Image> _imageFields;
    public RenderTaquin _taquinRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (_imageFields == null)
        {
            Debug.LogError("missing image fields in " + gameObject.name);
        }
        if (!_taquinRenderer && !(_taquinRenderer = FindObjectOfType<RenderTaquin>()))
        {
            Debug.LogError(gameObject.name + " needs an object with a RenderTaquin component");
        }

        // every time player changes a cell, we want to update the preview
        GameManagerSingleton.Instance.modifiedTaquin.AddListener(UpdatePreview);
    }

    void UpdatePreview()
    {
        for (int i = 0; i < _imageFields.Count; i++)
        {
            if (_taquinRenderer.cells.Count > i)
            {
                _imageFields[i].sprite = _taquinRenderer.cells[i]._currentImage;
            }
        }
    }
}
