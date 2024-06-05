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
        1, 1, 1, 1, 1, 1, 1,
        0, 0, 0, 2, 0, 0, 0
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
        int[,] board2D = new int[1, board.Length];
        int index = 0;

        for (int i = 0; i < board.Length; i++)
        {
            board2D[0, i] = board[index];
            index++;
        }

        return board2D;
    }

    public List<Move> GetPossibleMoves(int player){
        List<Move> moves = new List<Move>();

        for(int i = 0; i < board.Length; i++){
            int tile = board[i];

            //no need to check empty tiles or pieces that belong to other player
            if(tile == 0 || Math.Sign(tile) != player){
                continue;
            }

            foreach(Move move in GetPieceMoves(i, tile)){
                if(IsValidMove(move, player)){
                    moves.Add(move);
                }
            }
        }

        return moves;
    }
    List<Move> GetPieceMoves(int position, int piece){
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
    
    public bool IsValidMove(Move move, int player){
        int pieceType = Mathf.Abs(move.Piece);
        bool isValid = false;

        switch (pieceType){
            case 1: isValid = new GruntPiece(this).IsValidMove(move, player); break; 
            case 2: isValid = new BrutePiece(this).IsValidMove(move, player); break;
        }

        return isValid;
    }

    public GameOutcome GetGameOutcome()
    {
        int player1Moves = GetPossibleMoves(1).Count;
        int player2Moves = GetPossibleMoves(-1).Count;

        //bool player1InCheck = IsKingInCheck(1);
       // bool player2InCheck = IsKingInCheck(-1);

        //win
        if(GameObject.FindGameObjectsWithTag("Pawn2").Length == 0){
            return GameOutcome.PLAYER1;
        }
        if(GameObject.FindGameObjectsWithTag("Pawn").Length == 0){
            return GameOutcome.PLAYER2;
        }

        if (GameObject.FindGameObjectWithTag("King") == null)
        {
            return GameOutcome.PLAYER2;
        }
        if (GameObject.FindGameObjectWithTag("King2") == null)
        {
            return GameOutcome.PLAYER1;
        }

        //still undetermined
        return GameOutcome.UNDETERMINED;  
    }
    
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
