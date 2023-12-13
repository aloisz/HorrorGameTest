using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button playButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;

    
    void Start()
    {
        playButton.onClick.AddListener(PlayTheGame);
        quitButton.onClick.AddListener(QuitGame);
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
