using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartControls : MonoBehaviour
{
    private GameObject StartButton;
    private GameObject ExitButton;
    private Image ScreenShade;

    // Start is called before the first frame update
    void Start()
    {
        StartButton = GameObject.Find("StartGame");
        StartButton.GetComponent<Button>().onClick.AddListener(OnStartClick);

        ExitButton = GameObject.Find("ExitGame");
        ExitButton.GetComponent<Button>().onClick.AddListener(OnExitClick);

        ScreenShade = GameObject.Find("ScreenShade").GetComponent<Image>();
    }

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
                StartButton.GetComponent<Text>().color = SSColor;
                ExitButton.GetComponent<Text>().color = SSColor;
                yield return null;
            }

            SceneManager.LoadScene("Shrine of Origin");
        }
        else
        {
            while (ScreenShade.color.a > 0)
            {
                fadeAmount = SSColor.a - (fadeSpeed * Time.deltaTime);

                SSColor = new Color(SSColor.r, SSColor.g, SSColor.b, fadeAmount);
                ScreenShade.color = SSColor;
                StartButton.GetComponent<Text>().color = SSColor;
                ExitButton.GetComponent<Text>().color = SSColor;
                yield return null;
            }
        }
    }

    private void OnStartClick()
    {
        StartCoroutine(FadeScreenShade());
    }

    private void OnExitClick()
    {
         Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
