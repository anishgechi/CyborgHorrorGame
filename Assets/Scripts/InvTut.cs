using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvTut : MonoBehaviour
{
    public TextMeshProUGUI textObject;
    public float fadeInDuration = 12f;

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(12f);
        Color startColor = textObject.color;
        startColor.a = 0f;
        textObject.color = startColor;
        float alphaIncrement = Time.deltaTime / fadeInDuration;

        while (textObject.color.a < 1f)
        {
            Color newColor = textObject.color;
            newColor.a += alphaIncrement;
            textObject.color = newColor;
            yield return null;
        }

        Color finalColor = textObject.color;
        finalColor.a = 1f;
        textObject.color = finalColor;
    }
}








