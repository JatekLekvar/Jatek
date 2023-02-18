using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    private RectTransform backgroundRectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;

    void Awake(){
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        SetText("Testing...");
    }


    private void SetText(string tooltipText){
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8,8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    void Update(){
        rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
    }

}
