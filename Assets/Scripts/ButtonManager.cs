using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject RuleScreen;

    [SerializeField] private GameObject difficultyScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowRules()
    {
        RuleScreen.SetActive(true);
    }

    public void Back()
    {
        RuleScreen.SetActive(false);
    }

    public void LoadSingleplayer()
    {

        SceneManager.LoadScene("Singleplayer");
    }

    public void LoadMultiplayer()
    {
        SceneManager.LoadScene("Multiplayer");
    }

    public void EasyDifficulty()
    {
        Time.timeScale = 1;
        difficultyScreen.SetActive(false);
        GameManagerSIngleplayer.Instance.isEasy = 1;
    }
    public void HardDifficulty()
    {
        Time.timeScale = 1;
        difficultyScreen.SetActive(false);
        GameManagerSIngleplayer.Instance.isEasy = 2;
    }
}
