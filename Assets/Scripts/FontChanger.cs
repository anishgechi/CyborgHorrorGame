using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FontChanger : MonoBehaviour
{
    public TMP_FontAsset[] Fonts;
    public string TargetTag; 

    private static FontChanger instance;
    private int SelectedFontIndex = 0; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded; 
        SelectedFontIndex = PlayerPrefs.GetInt("SelectedFontIndex", 0);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeFont(SelectedFontIndex); 
    }

    private static void ChangeFont(int fontIndex)
    {
        if (instance == null || fontIndex < 0 || fontIndex >= instance.Fonts.Length)
        {
            return;
        }

        TMP_FontAsset selectedFont = instance.Fonts[fontIndex];

        foreach (var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            TMP_Text[] textElements = rootObject.GetComponentsInChildren<TMP_Text>(true);
            foreach (TMP_Text textElement in textElements)
            {
                if (textElement.CompareTag(instance.TargetTag))
                {
                    textElement.font = selectedFont;
                }
            }
        }
    }

    public static void SetSelectedFontIndex(int fontIndex)
    {
        if (fontIndex >= 0 && fontIndex < instance.Fonts.Length)
        {
            instance.SelectedFontIndex = fontIndex;
            PlayerPrefs.SetInt("SelectedFontIndex", fontIndex);
            ChangeFont(fontIndex); 
        }
    }
}






