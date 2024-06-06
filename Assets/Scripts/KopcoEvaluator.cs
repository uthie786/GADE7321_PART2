using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopcoEvaluator : Ievaluator
{
    public int GetEvaluation(Irepresentation representation){
        GameOutcome outcome = representation.GetGameOutcome();

        if(outcome == GameOutcome.PLAYER1){
            return 1;
        }
        if(outcome == GameOutcome.PLAYER2){ // change this
            return -1;
        }

        return 0;
    }
}
