using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopcoTileController : MonoBehaviour
{
    public delegate void TileClickAction(int tileNumber);
    public static event TileClickAction OnTileClicked;

    SpriteRenderer renderer;
    bool selected = false;

    public int TileNumber { get; set; } = -1;

    public Color NormalColour { get; set; } = Color.grey;
    public Color SelectedColor { get; set; } = Color.grey;

    public bool Selected { 
        get => selected; 
        set{
            selected = value;
            if(selected){
                renderer.color = SelectedColor;
            }else{
                renderer.color = NormalColour;
            }
        }
    }

    void Awake() 
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() 
    {

        if(OnTileClicked != null){
            OnTileClicked(TileNumber);
        }
    }
}
