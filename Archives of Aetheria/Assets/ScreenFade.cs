using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    private Image ScreenShade;

    public IEnumerator FadeScreenShade(bool fadeToBlack = true, float fadeSpeed = 7.5f)
    {
        Color SSColor = ScreenShade.color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (ScreenShade.color.a < 1)
            {
                fadeAmount = SSColor.a + (fadeSpeed * Time.deltaTime);

                SSColor = new Color(SSColor.r, SSColor.g, SSColor.b, fadeAmount);
                ScreenShade.color = SSColor;
                yield return null;
            }
        }
        else
        {
            while (ScreenShade.color.a > 0)
            {
                fadeAmount = SSColor.a - (fadeSpeed * Time.deltaTime);

                SSColor = new Color(SSColor.r, SSColor.g, SSColor.b, fadeAmount);
                ScreenShade.color = SSColor;
                yield return null;
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ScreenShade = GameObject.Find("ScreenShade").GetComponent<Image>();

        StartCoroutine(FadeScreenShade(false));
    }
}
