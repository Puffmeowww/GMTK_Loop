using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image; 
    public float fadeDuration = 1f;
    public float waitDuration = 1f;
    public float startAlpha = 0.2f;


    void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeLoop());
    }

    IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeIn(startAlpha, 1f));
            yield return new WaitForSeconds(waitDuration);
            yield return StartCoroutine(FadeIn(1f, startAlpha));
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator FadeIn(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        Color color = image.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            color.a = alpha;
            image.color = color; 
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }
}
