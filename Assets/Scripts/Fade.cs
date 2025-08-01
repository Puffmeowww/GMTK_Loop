using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image;  // Ҫ���뵭����UI����
    public float fadeDuration = 1f;  // ���뵭��ʱ��
    public float waitDuration = 1f;  // ��ʾͣ��ʱ��
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
            // ����
            yield return StartCoroutine(FadeIn(startAlpha, 1f));
            // �ȴ�
            yield return new WaitForSeconds(waitDuration);
            // ����
            yield return StartCoroutine(FadeIn(1f, startAlpha));
            // �ȴ�
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator FadeIn(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        Color color = image.color; // �õ���ǰ��ɫ

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            color.a = alpha;             // �޸�alphaֵ
            image.color = color;        // ���¸�ֵ��image
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }
}
