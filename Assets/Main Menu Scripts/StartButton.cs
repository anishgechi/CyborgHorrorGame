using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text buttonText;
    public Image fadeImage;
    public float fadeDuration = 1f;

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
        buttonText.fontStyle |= FontStyles.Strikethrough;
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvas());
    }

    IEnumerator FadeInCanvas()
    {
        CanvasGroup canvasGroup = fadeImage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            yield break;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene("GameScene");
    }
}



















