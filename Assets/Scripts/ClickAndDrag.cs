using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    public bool isDragging = false;
    private Vector2 offset;
    private Vector2 origPosition;
    
    public static ClickAndDrag Instance{get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void OnMouseDown()
    {
        // Calculate the offset between the object's position and the mouse position
       // offset = transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
    }

    void Update()
    {
        // Check if the object is being dragged
        if (isDragging)
        {
            // Calculate the new position of the object based on the mouse position
            Vector2 newPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = newPosition;
        }
        CheckMove();

        
    }

    private void SnapToEmptyPosition()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        foreach (Collider2D collider in nearbyColliders)
        {
            if (!collider.gameObject.CompareTag("Pawn") && collider.gameObject != gameObject && !collider.gameObject.CompareTag("King"))
            {
                transform.position = collider.transform.position;
                transform.parent = collider.transform;
                
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

                    if (x + 8 <GameManger.Instance.board.Length && GameManger.Instance.board[x + 8].transform.childCount == 0)
                    {
                        GameManger.Instance.board[x + 8].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    if(x - 8 >=0 && GameManger.Instance.board[x -8].transform.childCount == 0)
                    {
                        GameManger.Instance.board[x - 8].GetComponent<CircleCollider2D>().enabled = true;
                    }

                    if ( x - 6 >=0 && GameManger.Instance.board[x - 6].transform.childCount == 0)
                    {
                        GameManger.Instance.board[x - 6].GetComponent<CircleCollider2D>().enabled = true;
                    }

                    if (x + 7 <GameManger.Instance.board.Length && GameManger.Instance.board[x + 7].transform.childCount == 0 && gameObject.CompareTag("King") )
                    {
                        GameManger.Instance.board[x + 7].GetComponent<CircleCollider2D>().enabled = true;
                    }

                    if (x + 1 <GameManger.Instance.board.Length && GameManger.Instance.board[x + 1].transform.childCount == 0 && gameObject.CompareTag("King"))
                    {
                        GameManger.Instance.board[x + 1].GetComponent<CircleCollider2D>().enabled = true;
                    }

                    if (x - 7 >=0 && GameManger.Instance.board[x - 7].transform.childCount == 0 && gameObject.CompareTag("King"))
                    {
                        GameManger.Instance.board[x - 7].GetComponent<CircleCollider2D>().enabled = true;
                    }

                    if (x - 1 >=0 && GameManger.Instance.board[x - 1].transform.childCount == 0 && gameObject.CompareTag("King"))
                    {
                        GameManger.Instance.board[x - 1].GetComponent<CircleCollider2D>().enabled = true;
                    }
                    return;
                }
            }
        }

       


    }
}
