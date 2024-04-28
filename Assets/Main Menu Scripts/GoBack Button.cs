using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoBackButton : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text buttonText;
    public GameObject MainMenuPanel; 
    public GameObject SettingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonText.color = Color.red;

        if (MainMenuPanel != null)
        {
            MainMenuPanel.SetActive(true);
        }

        if (SettingsPanel != null)
        {
            SettingsPanel.SetActive(false);
        }
    }
}






