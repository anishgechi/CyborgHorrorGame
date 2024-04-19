using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsBackButton : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text buttonText;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

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

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        if (settingsMenuUI != null)
        {
            settingsMenuUI.SetActive(false);
        }
    }
}
