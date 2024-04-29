using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public GameObject[] board;
    [SerializeField] private GameObject pawns;
    private bool isPlayer1 = true;
    [SerializeField] private GameObject king;
    // Start is called before the first frame update
    void Start()
    {
        InitializeBoard();
        Debug.Log(board.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeBoard()
    {
        for (int x = 7; x <= 13; x++)
        {
          GameObject pawn = Instantiate(pawns, board[x].transform);
        }

        for (int x = 35; x <= 41; x++)
        {
          GameObject king = Instantiate(pawns, board[x].transform);
        }

        Instantiate(king, board[45].transform);
        Instantiate(king, board[3].transform);
    }
    
    
}
