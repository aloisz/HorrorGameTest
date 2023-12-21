using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup MainMenuCG;


    [SerializeField] private UnityEngine.UI.Button playButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;
    
    
    void Start()
    {
        MainMenuCG.DOFade(0, 0);
        StartCoroutine(EnableScreen());
        
        playButton.onClick.AddListener(PlayTheGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    IEnumerator EnableScreen()
    {
        yield return new WaitForSeconds(1.25f);
        MainMenuCG.DOFade(1, .2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayTheGame()
    {
        SceneManager.LoadScene(1);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
