using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kopcoRepresentation : Irepresentation
{
    int[] board = new int[49];

    private int[] boardStart = new int[49]
    {
        0, 0, 0, 2, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1,
        0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0,
        -1, -1, -1, -1, -1, -1, -1,
        0, 0, 0, -2, 0, 0, 0
    };

    public kopcoRepresentation()
    {
        Array.Copy(boardStart, board, boardStart.Length);
    }

    public kopcoRepresentation(int[] board)
    {
        this.board = board;
    }

    public Irepresentation Duplicate()
    {
        return new kopcoRepresentation(board.Clone() as int[]);
    }

    public int[] GetAs1DArray()
    {
        return board;
    }

    public int[,] GetAs2DArray()
    {
        int boardSize = 7;
        int[,] board2D = new int[boardSize, boardSize];
        int index = 0;

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                board2D[x, y] = board[index];
                index++;
            }
        }

        return board2D;
    }

    public List<Move> GetPossibleMoves(int player)
    {List<Move> moves = new List<Move>();
        int boardSize = 7;
        int[,] board2D = GetAs2DArray();

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                int tile = board2D[x, y];

                // No need to check empty tiles or pieces that belong to other player
                if (tile == 0 || Math.Sign(tile) != player)
                {
                    continue;
                }

                int index = y * boardSize + x; // Calculate the index for GetPieceMoves
                foreach (Move move in GetPieceMoves(index, tile))
                {
                    if (IsValidMove(move, player))
                    {
                        moves.Add(move);
                    }
                }
            }
        }

        return moves;
    }
    List<Move> GetPieceMoves(int position, int piece)
    {
        int pieceType = Mathf.Abs(piece);
        
        switch (pieceType){
            case 1: return new GruntPiece(this).GetMoves(position, piece); //pawn
            case 2: return new BrutePiece(this).GetMoves(position, piece); //king
            default: return new List<Move>();
        }
    }

    public bool MakeMove(Move move, int player)
    {
        board[move.From] = 0;
        board[move.To] = move.Piece;
        return true;  
    }
    
    public bool IsValidMove(Move move, int player)
    {
        int pieceType = Mathf.Abs(move.Piece);
        bool isValid = false;

        switch (pieceType)
        {
            case 1:
                isValid = new GruntPiece(this).IsValidMove(player, move.From, move.To);
                break;
            case 2:
                isValid = new BrutePiece(this).IsValidMove(player, move.From, move.To);
                break;
        }
        
        int[] board = GetAs1DArray();
        int toPosition = move.To;
        int destinationPiece = board[toPosition];

        if (destinationPiece != 0 && Math.Sign(destinationPiece) == player)
        {
            
            return false;
        }

        return isValid;
    }

    public GameOutcome GetGameOutcome()
    { int player1Moves = GetPossibleMoves(1).Count;
        int player2Moves = GetPossibleMoves(-1).Count;

        if(IsOnlyBrutesLeft()){
            return GameOutcome.DRAW;
        }
        //still undetermined
        if (player1Moves == 0)
        {
            return GameOutcome.PLAYER2; 
        }
        if (player2Moves == 0)
        {
            return GameOutcome.PLAYER1; 
        }
        
        return GameOutcome.UNDETERMINED; 
    }
    
    public bool IsOnlyBrutesLeft(){
        foreach(int tile in board){
            if(Mathf.Abs(tile) > 1){
                return false;
            }
        }
        return true;
    }
   

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
