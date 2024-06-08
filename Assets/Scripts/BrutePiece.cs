using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrutePiece : kopcoPiece
{
    public BrutePiece(kopcoRepresentation representation) : base(representation) { }

    public override bool IsValidMove(int player, int fromPosition, int toPosition)
    {
        if (!IsValidMoveGeneric(player, fromPosition, toPosition))
        {
            return false;
        }
        
        int boardSize = 7;
        int fromX = fromPosition % boardSize;
        int fromY = fromPosition / boardSize;
        int toX = toPosition % boardSize;
        int toY = toPosition / boardSize;
        
        if (Mathf.Abs(fromX - toX) <= 1 && Mathf.Abs(fromY - toY) <= 1)
        {
            return true;
        }
        
        return false;
    }

    public override List<int> GetPossiblePositions(int position, int piece)
    {
        List<int> possiblePositions = new List<int>();
        int[,] board = representation.GetAs2DArray();
        int boardSize = 7;
        int player = Math.Sign(piece);

        int x = position % boardSize;
        int y = position / boardSize;

        int[,] offsets = new int[,]
        {
            { -1, -1 }, { 0, -1 }, { 1, -1 },
            { -1, 0 },            { 1, 0 },
            { -1, 1 },  { 0, 1 },  { 1, 1 }
        };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newX = x + offsets[i, 0];
            int newY = y + offsets[i, 1];

            if (newX < 0 || newX >= boardSize || newY < 0 || newY >= boardSize)
            {
                continue;
            }

            int possiblePosition = newY * boardSize + newX;

            if (board[newX, newY] != 0 && Math.Sign(board[newX, newY]) == player)
            {
                continue;
            }

            possiblePositions.Add(possiblePosition);
        }

        return possiblePositions;
    }
}
