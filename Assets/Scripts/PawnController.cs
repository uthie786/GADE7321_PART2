using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour
{
    private bool isMovePossible;
  
    void Start()
    {
      
    }

   
    void Update()
    {
        CheckMove();
    }

    void CheckMove()
    {
        if (ClickAndDrag.Instance.isDragging)
        {
            for (int x = 0; x < GameManger.Instance.board.Length; x++)
            {
                if (gameObject.transform.parent.name == GameManger.Instance.board[x].name)
                {
                    if (GameManger.Instance.board[x + 6].transform.childCount == 0)
                    {
                        
                        GameManger.Instance.board[x + 6].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else
                    {
                        Debug.Log(GameManger.Instance.board[x+6] + "is available");
                        GameManger.Instance.board[x + 7].GetComponent<CircleCollider2D>().enabled = false;
  
                    }
                    return;
                }
            }
        }
    }
}
