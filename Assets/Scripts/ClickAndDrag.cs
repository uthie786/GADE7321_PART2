using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickAndDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 offset;

    void OnMouseDown()
    {
        // Calculate the offset between the object's position and the mouse position
       // offset = transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
        Debug.Log("MouseDown");
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToEmptyPosition();
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

        
    }

    private void SnapToEmptyPosition()
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        foreach (Collider2D collider in nearbyColliders)
        {
            if (!collider.gameObject.CompareTag("Pawn") && collider.gameObject != gameObject)
            {
                transform.position = collider.transform.position;
                return;
            }
        }
        
    }
}
