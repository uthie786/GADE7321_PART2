using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntPiece : kopcoPiece
{
    public GruntPiece(kopcoRepresentation representation) : base(representation){}
    public override bool IsValidMove(int player, int fromPosition, int toPosition){
        if(!IsValidMoveGeneric(player, fromPosition, toPosition)){
            //Debug.Log("Failed Generic Checks");
            return false;
        }

        
        if(Mathf.Abs(fromPosition - toPosition) != 2){
            //Debug.Log("Knight Failed Distance Check" + Mathf.Abs(fromPosition - toPosition));
            //Debug.Log("Failed Distance Check");
            return false;
        }

        return true;
    }
    public override List<int> GetPossiblePositions(int position, int piece){
        List<int> possiblePositions = new List<int>();
        int[] board = representation.GetAs1DArray();
        int[] offsets = new int[]{-8, 8, -6, 6};
        int player = Math.Sign(piece);

        foreach(int offset in offsets){
            int possiblePosition = position + offset;

            if(possiblePosition < 0 || possiblePosition >= board.Length){
                continue;
            }

            if(board[possiblePosition] != 0 && Math.Sign(board[possiblePosition]) == player){
                continue;
            }

            possiblePositions.Add(possiblePosition);
        }

        return possiblePositions;
    }
}
