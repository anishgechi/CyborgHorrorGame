using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    private TMP_Text buttonText;
    private Color originalColor;
    private FontStyles originalFontStyle;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        originalColor = buttonText.color;
        originalFontStyle = buttonText.fontStyle;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonText.color = Color.red;
        buttonText.fontStyle |= FontStyles.Bold;
        buttonText.fontStyle |= FontStyles.Strikethrough;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = originalColor;
        buttonText.fontStyle = originalFontStyle;
    }
}
