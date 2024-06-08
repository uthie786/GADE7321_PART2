using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class kopcoPiece
{
    protected kopcoRepresentation representation;

    public kopcoPiece(kopcoRepresentation representation)
    {
        this.representation = representation;
    }

    public abstract bool IsValidMove(int player, int fromPosition, int toPosition);
    public abstract List<int> GetPossiblePositions(int position, int piece);

    public bool IsValidMove(Move move, int player){
        return IsValidMove(player, move.From, move.To);
    }

    public virtual List<Move> GetMoves(int position, int piece)
    {
        List<Move> moves = new List<Move>();
        List<int> possiblePositions = GetPossiblePositions(position, piece);

        int player = Math.Sign(piece);

        foreach(int possiblePosition in possiblePositions){
            Move move = new Move(position, possiblePosition, piece);
            moves.Add(move);
        }

        return moves;
    }

    protected bool IsValidMoveGeneric(int player, int fromPosition, int toPosition)
    {
        int[] board = representation.GetAs1DArray();

        if(fromPosition >= board.Length || fromPosition < 0 || toPosition >= board.Length || toPosition < 0){
            return false;
        }

        //not actually moving anything
        if(fromPosition == toPosition){
            return false;
        }

        //can't move a piece that doesn't exist
        if(board[fromPosition] == 0){
            return false;
        }

        //can't move our opponent's pieces
        
        if(Math.Sign(board[fromPosition]) != player){
            return false;
        }

        //can't move on to our own pieces
        if(board[toPosition] != 0 && Math.Sign(board[toPosition]) == player){
            return false;
        }
        kopcoRepresentation repDup = (kopcoRepresentation) representation.Duplicate();
        Move move = new Move(fromPosition, toPosition, board[fromPosition]);
        repDup.MakeMove(move, player);
        if(repDup.IsBruteInCheck(player))
        {
            
            return false;
        }

        return true;
    }
}
