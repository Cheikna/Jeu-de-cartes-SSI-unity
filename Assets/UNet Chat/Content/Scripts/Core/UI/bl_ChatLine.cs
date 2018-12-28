using UnityEngine;
using System.Collections;

public class bl_ChatLine : MonoBehaviour
{
    private CanvasGroup m_canvas;

    public void FadeInTime(float t,float speed)
    {
        m_canvas = GetComponent<CanvasGroup>();
        StartCoroutine(Fade(t,speed));
    }

    IEnumerator Fade(float t,float speed)
    {
        yield return new WaitForSeconds(t);
        while(m_canvas.alpha > 0)
        {
            m_canvas.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        Destroy(gameObject);
    }
}