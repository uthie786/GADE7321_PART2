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
                
                if (tile == 0 || Math.Sign(tile) != player)
                {
                    continue;
                }

                int index = y * boardSize + x; 
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
            case 1: return new GruntPiece(this).GetMoves(position, piece); 
            case 2: return new BrutePiece(this).GetMoves(position, piece); 
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

        if(IsPlayer1BruteAlive() == false){
            return GameOutcome.PLAYER2;
        }
        if (IsPlayer2BruteAlive() == false)
        {
            return GameOutcome.PLAYER1;
        }
        if(IsLightGruntsAlive() == false){
            return GameOutcome.PLAYER2;
        }
        if(IsDarkGruntsAlive() == false){
            return GameOutcome.PLAYER1;
        }
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
    public bool IsBruteInCheck(int player)
    {
        int brutePosition = GetPiecePosition(PieceType.BRUTE, player);
        if (brutePosition == -1)
        {
            return false;
        }
        List<int> opponentPiecePositions = PlayerPieceIndices(player * -1);
        
        foreach (int opponentPiecePosition in opponentPiecePositions)
        {
            List<Move> moves = GetPieceMoves(opponentPiecePosition, board[opponentPiecePosition]);
            
            foreach (Move move in moves)
            {
                if (move.To == brutePosition)
                {
                    
                    return true;
                }
            }
        }

        return false;
    }
    int GetPiecePosition(PieceType type, int player){
        int pieceToFind = (int)type * player;

        for(int i = 0; i < board.Length; i++){
            if(pieceToFind == board[i]){
                return i;
            }
        }

        return -1;
    }
    List<int> PlayerPieceIndices(int player){
        List<int> pieces = new List<int>();

        for(int i = 0; i < board.Length; i++){
            if(board[i] != 0 && Math.Sign(board[i]) == player){
                pieces.Add(i);
            }
        }
        return pieces;
    }
    
    public bool IsPlayer1BruteAlive(){
        foreach(int tile in board){
            if (tile == 2)
            {
                return true;
            }
        }

        return false;
    }
    public bool IsPlayer2BruteAlive(){
        foreach(int tile in board){
            if (tile == -2)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsLightGruntsAlive()
    {
        foreach(int tile in board){
            if (tile == 1)
            {
                return true;
            }
        }

        return false;
    }
    public bool IsDarkGruntsAlive()
    {
        foreach(int tile in board){
            if (tile == -1)
            {
                return true;
            }
        }

        return false;
    }
}
