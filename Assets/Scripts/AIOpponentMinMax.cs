using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIOpponentMinMax : AIOpponent
{
    private Ievaluator eval;
    private int depth = 1;

    private int score;

    private bool outcomeNeither;
    private bool outcomePositive;
    private bool outcomenegative;

    private Irepresentation currentRep;
    private Irepresentation nextRep;

    private int maxEval;
    private int minEval;
    private int evaluation;

    private int bestPossibleMove;
    private int bestPossibleScore;
    private bool reachedScore;
    private List<int> outcomes;
    private List<Move> moves;
    private bool once = true;
    
    
    private void SetDepthBasedOnDifficulty()
    {
        if (once)
        {
            if( GameManagerSIngleplayer.Instance != null)
            {
                depth = GameManagerSIngleplayer.Instance.isEasy;
            }
            else
            {
                depth = 1; 
            }
            Debug.Log(depth);
            once = false;
        }
    }

    
    public Move GetMove(Irepresentation representation, int player)
    {
        SetDepthBasedOnDifficulty();
        reachedScore = false;
        eval = new KopcoEvaluator();

        bestPossibleMove = 0;
        bestPossibleScore = int.MinValue;

        outcomes = new List<int>();
        moves = representation.GetPossibleMoves(player);


        foreach (int i in Enumerable.Range(0, moves.Count))
        {
            currentRep = representation.Duplicate();
            currentRep.MakeMove(moves[i], player);
            score = MiniMax(currentRep, depth, -player, int.MinValue, int.MaxValue);
            outcomes.Add(score);

            if (score > bestPossibleScore)
            {
                bestPossibleScore = score;
                bestPossibleMove = i;
                reachedScore = true;
            }

            if (score == 0 && !reachedScore)
            {
                bestPossibleMove = i;
            }
            
        }

        outcomeNeither = outcomes.Contains(0);
        outcomePositive = outcomes.Contains(1);
        outcomenegative = outcomes.Contains(-1);

        if (outcomeNeither && !outcomePositive && !outcomenegative)
        {
            bestPossibleMove = Random.Range(0, moves.Count);
        }
        else
        {
            Outcomes(representation, player);
        }

        return moves[bestPossibleMove];
    }

    private void Outcomes(Irepresentation representation, int player)
    {
        for (int i = 0; i < outcomes.Count; i++)
        {
            if (outcomes[i] == -1)
            {
                currentRep = representation.Duplicate();
                currentRep.MakeMove(currentRep.GetPossibleMoves(player)[i], player);

                if (currentRep.GetGameOutcome() == GameOutcome.PLAYER2)
                {
                    bestPossibleMove = i;
                    break;
                }
            }

            if (outcomes[i] == 1)
            {
                currentRep = representation.Duplicate();
                currentRep.MakeMove(currentRep.GetPossibleMoves(-player)[i], -player);

                if (currentRep.GetGameOutcome() == GameOutcome.PLAYER1)
                {
                    bestPossibleMove = i;
                }
            }
        }
    }

    public int MiniMax(Irepresentation rep, int depth, int player, int alphaPruning, int betaPruning)
    {
        if (depth == 0 || rep.GetGameOutcome() != GameOutcome.UNDETERMINED)
        {
            return eval.GetEvaluation(rep);
        }

        if (player == 1)
        {
            return MaxOutcome(rep, depth, player, alphaPruning, betaPruning);
        }
        else
        {
            return MinOutcome(rep, depth, player, alphaPruning, betaPruning);
        }
    }

    private int MaxOutcome(Irepresentation rep, int depth, int player, int alpha, int beta)
    {
        int maxEval = int.MinValue;

        foreach (Move posMove in rep.GetPossibleMoves(player))
        {
            nextRep = rep.Duplicate();
            nextRep.MakeMove(posMove, player);

            int evaluation = MiniMax(nextRep, depth - 1, -player, alpha, beta);
            maxEval = Mathf.Max(maxEval, evaluation);

            alpha = Mathf.Max(alpha, evaluation);
            if (beta <= alpha) break;
        }

        return maxEval;
    }

    private int MinOutcome(Irepresentation rep, int depth, int player, int alpha, int beta)
    {
        int minEval = int.MaxValue;

        foreach (Move posMove in rep.GetPossibleMoves(player))
        {
            nextRep = rep.Duplicate();
            nextRep.MakeMove(posMove, player);

            int evaluation = MiniMax(nextRep, depth - 1, -player, alpha, beta);
            minEval = Mathf.Min(minEval, evaluation);

            beta = Mathf.Min(beta, evaluation);
            if (beta <= alpha) break;
        }

        return minEval;
    }
}
