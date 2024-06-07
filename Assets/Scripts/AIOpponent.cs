using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AIOpponent
{
    Move GetMove(Irepresentation representation, int player);
}

