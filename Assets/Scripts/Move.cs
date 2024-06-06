using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Move
{
    public int From { get; set; } = 0;
    public int To { get; set; } = 0;
    public int Piece { get; set; } = 0;
    public Vector2Int Position { get; set; } = Vector2Int.zero;

    public Move(int from, int to, int piece)
    {
        From = from;
        To = to;
        Piece = piece;
    }

    public Move(Vector2Int position)
    {
        Position = position;
    }

    public override string ToString()
    {
        return "From: " + From + " / To: " + To + " / Piece: " + Piece + " / Position: " + Position;
    }

    public override bool Equals(object obj)
    {
        Move other = (Move)obj;
        if(other.From != From)
        {
            return false;
        }
        if(other.To != To)
        {
            return false;
        }
        if(other.Piece != Piece)
        {
            return false;
        }
        if(other.Position.x != Position.x)
        {
            return false;
        }
        if(other.Position.y != Position.y)
        {
            return false;
        }

        return true;
    }
/*
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }*/
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
