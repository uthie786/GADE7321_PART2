using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickAndDrag : MonoBehaviour
{
    public bool isDragging = false;
    private Vector2 offset;
    private Vector2 origPosition;
    private AIOpponent aiPlayer = new AIOpponentMinMax();
    
    kopcoRepresentation representation;
     public int playerTurn = 1;
    
    public static ClickAndDrag Instance{get; private set; }
    


    private void Awake()
    {
        Instance = this;
    }
    

    void OnMouseDown()
    {
       CheckPawns();
       origPosition = gameObject.transform.position;
       isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToEmptyPosition();
        foreach (GameObject obj in GameManger.Instance.board)
        {
            obj.GetComponent<CircleCollider2D>().enabled = false;
        }
        CheckPawns();
    }

    void Update()
    {
        
        // Check if the object is being dragged
        if (isDragging)
        {
            
            if (GameManger.Instance.num % 2 == 0 && SceneManager.GetActiveScene().name != "Singleplayer")
            {
                //Debug.Log(SceneManager.GetActiveScene().name);
                if (gameObject.CompareTag("Pawn2") || gameObject.CompareTag("King2"))
                {
                    Vector2 newPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                    transform.position = newPosition;
                }
            }
            else
            {
                if (gameObject.CompareTag("Pawn") || gameObject.CompareTag("King"))
                {
                    Vector2 newPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                    transform.position = newPosition;
                }
            }
            
            
        }
        CheckMove();

        
    }

    

    private void SnapToEmptyPosition()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider.CompareTag("Board"))
            {
                transform.position = collider.transform.position;
                if (collider.transform.childCount > 0)
                {
                    if(collider.transform.GetChild(0).gameObject == GameObject.FindWithTag("King"))
                    {
                        GameManger.Instance.GameOver("Player 2");
                    }
                    if (collider.transform.GetChild(0).gameObject == GameObject.FindWithTag("King2"))
                    {
                        GameManger.Instance.GameOver("Player 1");
                    }
                    Destroy(collider.transform.GetChild(0).gameObject);
                }
                transform.parent = collider.transform;
                GameManger.Instance.num++;
               CheckPawns();
                return;
            }
            else
            {
                transform.position = origPosition;
            }
        }
        
    }
    void CheckMove()
    {
        if (isDragging)
        {
            for (int x = 0; x < GameManger.Instance.board.Length; x++)
            {
                if (gameObject.transform.parent.name == GameManger.Instance.board[x].name)
                {
                    if (x + 6 <GameManger.Instance.board.Length && GameManger.Instance.board[x + 6].transform.childCount == 0)
                    {
                        GameManger.Instance.board[x + 6].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if ( x + 6 <GameManger.Instance.board.Length && !gameObject.CompareTag(GameManger.Instance.board[x + 6].transform.GetChild(0).tag) && GameManger.Instance.board[x + 6].transform.childCount > 0)
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x + 6].transform.GetChild(0).CompareTag("Pawn"))||(gameObject.CompareTag("King2") &&
                                GameManger.Instance.board[x +6].transform.GetChild(0).CompareTag("Pawn2")) || (gameObject.CompareTag("Pawn") && GameManger.Instance.board[x + 6].transform.GetChild(0).CompareTag("King")) || (gameObject.CompareTag("Pawn2") && GameManger.Instance.board[x + 6].transform.GetChild(0).CompareTag("King2")))
                        {
                            
                        }
                        else
                        {
                            GameManger.Instance.board[x + 6].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }
                    if (x + 8 <GameManger.Instance.board.Length && GameManger.Instance.board[x + 8].transform.childCount == 0)
                    {
                        GameManger.Instance.board[x + 8].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if ( x + 8 <GameManger.Instance.board.Length && !gameObject.CompareTag(GameManger.Instance.board[x + 8].transform.GetChild(0).tag) && GameManger.Instance.board[x + 8].transform.childCount > 0)
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x + 8].transform.GetChild(0).CompareTag("Pawn"))||(gameObject.CompareTag("King2") &&
                                GameManger.Instance.board[x + 8].transform.GetChild(0).CompareTag("Pawn2"))|| (gameObject.CompareTag("Pawn") && GameManger.Instance.board[x + 8].transform.GetChild(0).CompareTag("King"))||(gameObject.CompareTag("Pawn2") && GameManger.Instance.board[x + 8].transform.GetChild(0).CompareTag("King2")))
                        {
                            
                        }
                        else
                        {
                            GameManger.Instance.board[x + 8].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }
                    
                    if(x - 8 >=0 && GameManger.Instance.board[x -8].transform.childCount == 0)
                    {
                        GameManger.Instance.board[x - 8].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if ( x - 8 >=0 && !gameObject.CompareTag(GameManger.Instance.board[x - 8].transform.GetChild(0).tag) && GameManger.Instance.board[x -8].transform.childCount > 0)
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x -8].transform.GetChild(0).CompareTag("Pawn"))||(gameObject.CompareTag("King2") &&
                                GameManger.Instance.board[x -8].transform.GetChild(0).CompareTag("Pawn2"))||( gameObject.CompareTag("Pawn") && GameManger.Instance.board[x -8].transform.GetChild(0).CompareTag("King"))||(gameObject.CompareTag("Pawn2") && GameManger.Instance.board[x -8].transform.GetChild(0).CompareTag("King2")))
                        {
                            
                        }
                        else
                        {
                            GameManger.Instance.board[x - 8].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }

                    if ( x - 6 >=0 && GameManger.Instance.board[x - 6].transform.childCount == 0)
                    {
                        GameManger.Instance.board[x - 6].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if ( x - 6 >=0 && !gameObject.CompareTag(GameManger.Instance.board[x -6].transform.GetChild(0).tag) && GameManger.Instance.board[x -6].transform.childCount > 0)
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x - 6].transform.GetChild(0).CompareTag("Pawn"))|| (gameObject.CompareTag("King2") &&
                            GameManger.Instance.board[x - 6].transform.GetChild(0).CompareTag("Pawn2"))|| (gameObject.CompareTag("Pawn") && GameManger.Instance.board[x - 6].transform.GetChild(0).CompareTag("King"))||(gameObject.CompareTag("Pawn2") && GameManger.Instance.board[x - 6].transform.GetChild(0).CompareTag("King2")))
                        {
                        }
                        else
                        {
                            GameManger.Instance.board[x - 6].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }

                    if (x + 7 <GameManger.Instance.board.Length && GameManger.Instance.board[x + 7].transform.childCount == 0 && (gameObject.CompareTag("King") || gameObject.CompareTag("King2")))
                    {
                        GameManger.Instance.board[x + 7].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if (x + 7 <GameManger.Instance.board.Length && !gameObject.CompareTag(GameManger.Instance.board[x + 7].transform.GetChild(0).tag) && GameManger.Instance.board[x + 7].transform.childCount > 0 && (gameObject.CompareTag("King")|| gameObject.CompareTag("King2")))
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x + 7].transform.GetChild(0).CompareTag("Pawn"))||(gameObject.CompareTag("King2") &&
                            GameManger.Instance.board[x + 7].transform.GetChild(0).CompareTag("Pawn2")))
                        {
                            
                        }
                        else
                        {
                            GameManger.Instance.board[x + 7].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }

                    if (x + 1 <GameManger.Instance.board.Length && GameManger.Instance.board[x + 1].transform.childCount == 0 && (gameObject.CompareTag("King")|| gameObject.CompareTag("King2")))
                    {
                        GameManger.Instance.board[x + 1].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if (x + 1 <GameManger.Instance.board.Length && !gameObject.CompareTag(GameManger.Instance.board[x + 1].transform.GetChild(0).tag) && GameManger.Instance.board[x + 1].transform.childCount > 0 && (gameObject.CompareTag("King")|| gameObject.CompareTag("King2")))
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x + 1].transform.GetChild(0).CompareTag("Pawn"))||(gameObject.CompareTag("King2") &&
                            GameManger.Instance.board[x + 1].transform.GetChild(0).CompareTag("Pawn2")))
                        {
                            
                        }
                        else
                        {
                            GameManger.Instance.board[x + 1].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }

                    if (x - 7 >=0 && GameManger.Instance.board[x - 7].transform.childCount == 0 && (gameObject.CompareTag("King")|| gameObject.CompareTag("King2")))
                    {
                        GameManger.Instance.board[x - 7].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if ( x - 7 >=0 && !gameObject.CompareTag(GameManger.Instance.board[x -7].transform.GetChild(0).tag) && GameManger.Instance.board[x -7].transform.childCount > 0 && (gameObject.CompareTag("King")|| gameObject.CompareTag("King2")))
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x -7].transform.GetChild(0).CompareTag("Pawn"))||(gameObject.CompareTag("King2") &&
                                GameManger.Instance.board[x -7].transform.GetChild(0).CompareTag("Pawn2")))
                        {
                            
                        }
                        else
                        {
                            GameManger.Instance.board[x - 7].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }

                    if (x - 1 >=0 && GameManger.Instance.board[x - 1].transform.childCount == 0 && (gameObject.CompareTag("King")|| gameObject.CompareTag("King2")))
                    {
                        GameManger.Instance.board[x - 1].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    else if (x - 7 >=0 &&!gameObject.CompareTag(GameManger.Instance.board[x -1].transform.GetChild(0).tag) && GameManger.Instance.board[x -1].transform.childCount > 0 && (gameObject.CompareTag("King")|| gameObject.CompareTag("King2")))
                    {
                        if ((gameObject.CompareTag("King") &&
                            GameManger.Instance.board[x -1].transform.GetChild(0).CompareTag("Pawn"))|| (gameObject.CompareTag("King2") &&
                                GameManger.Instance.board[x -1].transform.GetChild(0).CompareTag("Pawn2")))
                        {
                            
                        }
                        else
                        {
                            GameManger.Instance.board[x - 1].GetComponent<CircleCollider2D>().enabled = true;
                        }
                    }
                    return;
                }
                
            }
        }
    }

    void OpponentMove(Move move)
    {
        
    }

    void CheckPawns()
    {
        Debug.Log(GameObject.FindGameObjectsWithTag("Pawn").Length);
        Debug.Log(GameObject.FindGameObjectsWithTag("Pawn2").Length);
        if (GameObject.FindGameObjectsWithTag("Pawn").Length == 0)
        {
            GameManger.Instance.GameOver("Player 2");
        }
        if (GameObject.FindGameObjectsWithTag("Pawn2").Length == 0)
        {
            GameManger.Instance.GameOver("Player 1");
        }
    }
    
    IEnumerator AITurnCoroutine(){
        yield return new WaitForSeconds(1f);
        Move move = aiPlayer.GetMove(representation, playerTurn);

        if(move != null){
            MakeMove(move);
        }
    }
  
    
}
