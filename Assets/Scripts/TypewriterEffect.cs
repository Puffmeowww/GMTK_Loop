using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;


public class TypewriterEffect : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    private string fullText;
    public float delay = 0.05f;

    private void Awake()
    {
        textComponent = gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void StartTypeWriter(string newText)
    {
        fullText = newText;
        StopAllCoroutines();
        StartCoroutine(RevealText());
    }

    IEnumerator RevealText()
    {
        textComponent.text = fullText;
        textComponent.maxVisibleCharacters = 0;

        for (int i = 0; i <= fullText.Length; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }
    }

    public void ShowAllText()
    {
        textComponent.maxVisibleCharacters = fullText.Length;
    }

    public bool IsAllText()
    {
        if(textComponent.maxVisibleCharacters >= fullText.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}