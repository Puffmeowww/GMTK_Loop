using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image;  // 要淡入淡出的UI对象
    public float fadeDuration = 1f;  // 淡入淡出时间
    public float waitDuration = 1f;  // 显示停留时间
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
            // 淡入
            yield return StartCoroutine(FadeIn(startAlpha, 1f));
            // 等待
            yield return new WaitForSeconds(waitDuration);
            // 淡出
            yield return StartCoroutine(FadeIn(1f, startAlpha));
            // 等待
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator FadeIn(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        Color color = image.color; // 拿到当前颜色

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            color.a = alpha;             // 修改alpha值
            image.color = color;        // 重新赋值给image
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }
}
