using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialSc : MonoBehaviour
{
    public TextMeshProUGUI[] TutorialText;
    public float DisplayDuration = 3f;
    public Color targetColor = new Color(0.5f, 0f, 0f); 
    public float colorTransitionDuration = 1f;

    private int currentIndex = 0;
    private Coroutine tutorialCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        tutorialCoroutine = StartCoroutine(ShowTutorial());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShowTutorial()
    {
        while (currentIndex < TutorialText.Length)
        {
            TutorialText[currentIndex].gameObject.SetActive(true);
            yield return StartCoroutine(ColorTransition(TutorialText[currentIndex], targetColor));
            yield return new WaitForSeconds(DisplayDuration);
            TutorialText[currentIndex].gameObject.SetActive(false);
            currentIndex++;
        }

        gameObject.SetActive(false);
    }

    IEnumerator ColorTransition(TextMeshProUGUI text, Color targetColor)
    {
        float elapsedTime = 0f;
        Color startColor = text.color;

        while (elapsedTime < colorTransitionDuration)
        {
            text.color = Color.Lerp(startColor, targetColor, elapsedTime / colorTransitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.color = targetColor;
    }

    public void StopTutorial()
    {
        if (tutorialCoroutine != null)
            StopCoroutine(tutorialCoroutine);
    }
}

