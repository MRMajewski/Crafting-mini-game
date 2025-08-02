using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuPanel;
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private CanvasGroup exitPanel;
    [SerializeField] private float fadeDuration = 1f;

    private Sequence activeSequence=null;

    [ContextMenu("INIT")]
    public void OpeningSequence()
    {
        if (!fadePanel.gameObject.activeSelf)
        {
            fadePanel.gameObject.SetActive(true);
        }

        fadePanel.alpha = 1f;
        fadePanel.DOFade(0f, fadeDuration);

        mainMenuPanel.alpha = 1f;
    }

    public void InitMainMenu()
    {
      //  activeSequence = DOTween.Sequence();
        OpeningSequence();
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
        StartGameSequence();

    }


    private void StartGameSequence()
    {

     

        if (activeSequence != null && activeSequence.IsActive())
        {
            activeSequence.Kill();
        }

        fadePanel.gameObject.SetActive(true);
        fadePanel.alpha = 0f;
        activeSequence = DOTween.Sequence();
        activeSequence
            .Append(fadePanel.DOFade(1f, fadeDuration)).
             AppendInterval(fadeDuration/2)
            .Append(mainMenuPanel.DOFade(0f, 0f))
            .Append(fadePanel.DOFade(0f, fadeDuration))
            .AppendInterval(fadeDuration / 4)
            .OnComplete(() =>
            {
                fadePanel.alpha = 0f;
                fadePanel.gameObject.SetActive(false);
              
                GameController.Instance.PauseGame(false );
                this.gameObject.SetActive(false);
            });

    }

    public void OpenExitPanel()
    {
        exitPanel.gameObject.SetActive(true);
        exitPanel.alpha = 0f;
        exitPanel.DOFade(1f, fadeDuration / 2f);
    }

    public void ReturnFromExitPanel()
    {
        if (activeSequence != null && activeSequence.IsActive())
        {
            activeSequence.Kill();
        }

        // Stwórz nową sekwencję

        activeSequence = DOTween.Sequence();
        activeSequence
            .Append(exitPanel.DOFade(0f, fadeDuration / 2f))
            .OnComplete(() =>
            {
                exitPanel.alpha = 0f;
                exitPanel.gameObject.SetActive(false);
            });
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
}
