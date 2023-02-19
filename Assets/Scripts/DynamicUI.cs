using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform backgroundRectTransform;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RectTransform rectTransform;

    void Awake(){
        //backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        //textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        //rectTransform = transform.GetComponent<RectTransform>();

        //SetText("Testing...");
        this.gameObject.SetActive(false);
    }


    public void SetText(string tooltipText){
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8,8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    void Update(){
        rectTransform.anchoredPosition = (Input.mousePosition / canvasRectTransform.localScale.x) + new Vector3(5,5,5);
        
    }

}
