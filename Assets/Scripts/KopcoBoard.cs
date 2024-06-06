using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopcoBoard : MonoBehaviour
{

    [SerializeField] GameObject tileTemplate;
    [SerializeField] GameObject chessPieceTemplate;

    kopcoRepresentation representation;

    List<OneDChessPieceController> pieces = new List<OneDChessPieceController>();
    List<OneDChessTileController> tiles = new List<OneDChessTileController>();

    OneDChessPieceController selectedPiece;
    OneDChessTileController selectedTile;
    private ClickAndDrag clicked = ClickAndDrag.Instance;

    /*  human player = 1
        ai player = -1 */
    int playerTurn;
    
    private AIOpponent aiPlayer = new AIOpponentMinMax();
    GameOutcome outcome = GameOutcome.UNDETERMINED;

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
        //GenerateBoard();
        GeneratePieces();
    }

    void OnEnable(){
        //OneDChessTileController.OnTileClicked += OnTileClicked;
        //GameUIController.OnResetClicked += ResetGame;
    }

    void OnDisable(){
        //OneDChessTileController.OnTileClicked -= OnTileClicked;
        //GameUIController.OnResetClicked -= ResetGame;
    }

    int RollPlayerTurn(){
        return Random.Range(0f, 1f) > 0.5 ? 1 : -1;
    }

    void ResetGame(){
        outcome = GameOutcome.UNDETERMINED;
        representation = new kopcoRepresentation();
        playerTurn = RollPlayerTurn();

        if(playerTurn == -1){
            StartCoroutine(AITurnCoroutine());
        }
        
        UpdateInfoMessage();
        //GeneratePieces();
    }

    void GenerateBoard(){
        int[] board = representation.GetAs1DArray();

        for(int i = 0; i < board.Length; i++){
            GameObject tile = Instantiate(tileTemplate);

            tile.transform.SetParent(transform);
            tile.transform.position = new Vector2(-board.Length/2f + i + 0.5f, 0);

            OneDChessTileController tileController = tile.GetComponent<OneDChessTileController>();
            tileController.Selected = false;
            tileController.TileNumber = i;
            tiles.Add(tileController);
        }
    } 

    void GeneratePieces(){
        foreach(OneDChessPieceController piece in pieces){
            if(piece != null){
                Destroy(piece.gameObject);
            }
        } 
        pieces.Clear();
        

        int[] board = representation.GetAs1DArray();

        for(int i = 0; i < board.Length; i++){
            if(board[i] == 0){
                pieces.Add(null);
                continue;
            }

            GameObject piece = Instantiate(chessPieceTemplate);
            piece.transform.SetParent(transform);
            piece.transform.position = new Vector2(-board.Length/2f + i + 0.5f, 0);

            OneDChessPieceController pieceController = piece.GetComponent<OneDChessPieceController>();
            pieceController.SetPieceType(Mathf.Abs(board[i]));
            pieceController.SetColour(Mathf.Sign(board[i]) >= 0 ? lightPieces : darkPieces);
            pieces.Add(pieceController);
        }
    }
    

    void OnTileClicked(int tileNumber){
        //ignore human moves if it is AI's turn
        if(playerTurn != 1 && outcome != GameOutcome.UNDETERMINED){
            return;
        }

        //move piece
        if(clicked.isDragging == true && selectedTile.TileNumber != tileNumber){
            Move move = new Move(selectedTile.TileNumber, tileNumber, selectedPiece.TypeNumber * playerTurn );

            //check of valid move first and destory piece if captured
            if(representation.IsValidMove(move, playerTurn)){
                MakeMove(move);

                if(outcome == GameOutcome.UNDETERMINED)
                    StartCoroutine(AITurnCoroutine());
            }

            selectedPiece = null;
            selectedTile = null;
            
            foreach(OneDChessTileController tile in tiles){
                tile.Selected = false;
            }
        }
        //highlight selected tile
        else if(pieces[tileNumber] != null){
            tiles[tileNumber].Selected = !tiles[tileNumber].Selected;
            selectedPiece = pieces[tileNumber];
            selectedTile = tiles[tileNumber];
        }
    }

    void MakeMove(Move move){
        Debug.Log(move.From + ", " + move.To + ", " + playerTurn);

       pieces[move.From].transform.position = tiles[move.To].transform.position;

        //capture piece
        if(pieces[move.To] != null){
            Destroy(pieces[move.To].gameObject);
            pieces[move.To] = null;
        }

        pieces[move.To] = pieces[move.From];
        pieces[move.From] = null;
        

        Debug.Log(move);
        representation.MakeMove(move, playerTurn);

        outcome = representation.GetGameOutcome();

        if(outcome == GameOutcome.UNDETERMINED){
            playerTurn *= -1;
        }

        UpdateInfoMessage();

        
        Debug.Log(representation.GetGameOutcome());
        Debug.Log(representation);
    }

    void UpdateInfoMessage(){
        if(outcome == GameOutcome.UNDETERMINED){
            string player = playerTurn == 1 ? "Human" : "AI";
            GameUIController.instance.SetInfoText(player + " player's turn...");
        }
        else if(outcome == GameOutcome.DRAW){
            GameUIController.instance.SetInfoText("It's a DRAW");
        }
        else if(outcome == GameOutcome.PLAYER1){
            GameUIController.instance.SetInfoText("Human player WINS!");
        }
        else if(outcome == GameOutcome.PLAYER2){
            GameUIController.instance.SetInfoText("AI player WINS!");
        }
    }

    IEnumerator AITurnCoroutine(){
        yield return new WaitForSeconds(1f);
        Move move = aiPlayer.GetMove(representation, playerTurn);

        if(move != null){
            MakeMove(move);
        }
    }
}
