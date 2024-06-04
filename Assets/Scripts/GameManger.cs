using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    public GameObject[] board;
    [SerializeField] private GameObject pawns;
    private bool isPlayer1 = true;
    [SerializeField] private GameObject king;
    [SerializeField] private GameObject pawn2;
    [SerializeField] private GameObject king2;
    [SerializeField] private GameObject p1Text;
    [SerializeField] private GameObject p2Text;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private Text endText;
    [SerializeField] private GameObject difficultyScreen;
    public int num;
    public bool isEasy;
    public static GameManger Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        endScreen.SetActive(false);
        p1Text.SetActive(true);
        p2Text.SetActive(false);
        num = 1;
        InitializeBoard();
        if (SceneManager.GetActiveScene().name == "Singleplayer")
        {
            Time.timeScale = 0;
            difficultyScreen.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (num % 2 == 0)
        {
            p1Text.SetActive(false);
            p2Text.SetActive(true);
        }
        else
        {
            p1Text.SetActive(true);
            p2Text.SetActive(false);
        }
    }

    private void InitializeBoard()
    {
        for (int x = 7; x <= 13; x++)
        {
          GameObject pawn = Instantiate(pawns, board[x].transform);
        }

        for (int x = 35; x <= 41; x++)
        {
          GameObject pawn = Instantiate(pawn2, board[x].transform);
        }

        Instantiate(king2, board[45].transform);
        Instantiate(king, board[3].transform);
    }

    public void GameOver(string winner)
    {
        endText.text = winner + " WINS!";
        p1Text.SetActive(false);
        p2Text.SetActive(false);
        endScreen.SetActive(true);
    }
    
    
}
