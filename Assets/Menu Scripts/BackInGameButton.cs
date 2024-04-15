using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackInGameButton : MonoBehaviour

{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    public void BackToMenu()
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
