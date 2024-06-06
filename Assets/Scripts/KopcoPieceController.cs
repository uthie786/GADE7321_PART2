using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopcoPieceController : MonoBehaviour
{ SpriteRenderer renderer;

    [SerializeField] Sprite brute;
    [SerializeField] Sprite grunt;
    [SerializeField] PieceType pieceType = PieceType.BRUTE;
    
    public PieceType Type
    {
        get => pieceType;
    }
    
    public int TypeNumber
    {
        get => (int) pieceType;
    }

    void Awake() 
    {
        renderer = GetComponent<SpriteRenderer>();
        SetPieceType(pieceType);
    }

    public void SetPieceType(int type){
        SetPieceType((PieceType) type);
    }

    public void SetPieceType(PieceType type)
    {
        pieceType = type;
        switch(pieceType){
            case PieceType.BRUTE : renderer.sprite = brute; break;
            case PieceType.GRUNT : renderer.sprite = grunt; break;
           
        }
    }
    
    public void SetColour(Color color)
    {
        renderer.color = color;
    }
}

public enum PieceType 
{
    BRUTE = 1,
    GRUNT = 2,
 
}
