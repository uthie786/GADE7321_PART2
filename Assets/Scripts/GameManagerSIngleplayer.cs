using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerSIngleplayer : MonoBehaviour
{

    public int isEasy = 0;
    public static GameManagerSIngleplayer Instance { get; private set; }

    private void Awake()
    {
        Time.timeScale = 0;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
