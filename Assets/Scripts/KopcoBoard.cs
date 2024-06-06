using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KopcoBoard : MonoBehaviour
{
    [SerializeField] Color darkSquares = Color.black;
    [SerializeField] Color lightSquares = Color.white;
    [SerializeField] Color darkSquaresHighlight = Color.yellow;
    [SerializeField] Color lightSquaresHighlight = Color.yellow;
    [SerializeField] Color darkPieces = Color.magenta;
    [SerializeField] Color lightPieces = Color.cyan;
    [SerializeField] GameObject tileTemplate;
    [SerializeField] GameObject chessPieceTemplate;

    kopcoRepresentation representation;

    List<KopcoPieceController> pieces = new List<KopcoPieceController>();
    List<KopcoTileController> tiles = new List<KopcoTileController>();

    KopcoPieceController selectedPiece;
    KopcoTileController selectedTile;
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
        GenerateBoard();
        GeneratePieces();
    }

    void OnEnable()
    {
        //Debug.Log("enabled");
        KopcoTileController.OnTileClicked += OnTileClicked;
        //GameUIController.OnResetClicked += ResetGame;
    }

    void OnDisable()
    {
        //Debug.Log("disabled");
        KopcoTileController.OnTileClicked -= OnTileClicked;
        //GameUIController.OnResetClicked -= ResetGame;
    }

    int RollPlayerTurn()
    {
        //return Random.Range(0, 2) == 1 ? 1 : -1;
        return 1;
    }

    void ResetGame()
    {
        outcome = GameOutcome.UNDETERMINED;
        representation = new kopcoRepresentation();
        playerTurn = RollPlayerTurn();

        if(playerTurn == -1)
        {
            StartCoroutine(AITurnCoroutine());
        }
        
        UpdateInfoMessage();
        GeneratePieces();
    }

    void GenerateBoard()
    {
        int boardSize = 7;
        GameObject[,] board = new GameObject[boardSize, boardSize];

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                GameObject tile = Instantiate(tileTemplate);

                tile.transform.SetParent(transform);
                tile.transform.position = new Vector2(-boardSize / 2f + x + 0.5f, -boardSize / 2f + y + 0.5f);

                KopcoTileController tileController = tile.GetComponent<KopcoTileController>();
                bool isLightSquare = (x + y) % 2 == 0;
                tileController.NormalColour = isLightSquare ? lightSquares : darkSquares;
                tileController.SelectedColor = isLightSquare ? lightSquaresHighlight : darkSquaresHighlight;
                tileController.Selected = false;
                tileController.TileNumber = y * boardSize + x;
                tiles.Add(tileController);

                board[x, y] = tile;
            }
        }
    } 

    void GeneratePieces()
    {
        foreach (KopcoPieceController piece in pieces)
        {
            if (piece != null)
            {
                Destroy(piece.gameObject);
            }
        }
        pieces.Clear();

        int boardSize = 7;
        int[,] board = representation.GetAs2DArray();

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (board[x, y] == 0)
                {
                    pieces.Add(null);
                    continue;
                }
                
                if (y > 1 && y < boardSize - 2)
                {
                    pieces.Add(null);
                    continue;
                }

                GameObject piece = Instantiate(chessPieceTemplate);
                piece.transform.SetParent(transform);
                piece.transform.position = new Vector2(-boardSize / 2f + x + 0.5f, -boardSize / 2f + y + 0.5f);
                KopcoPieceController pieceController = piece.GetComponent<KopcoPieceController>();
                pieceController.SetPieceType(Mathf.Abs(board[x, y]));
                if (y < 2) {
                    pieceController.SetColour(lightPieces); 
                } else if (y >= boardSize - 2) {
                    pieceController.SetColour(darkPieces); 
                }

                pieces.Add(pieceController);
            }
        }
    }
    

    void OnTileClicked(int tileNumber)
    {
        //Debug.Log(playerTurn + " yes");
        
        if(playerTurn != 1 && outcome != GameOutcome.UNDETERMINED)
        {
            return;
        }
       
        if(selectedTile != null && selectedTile.TileNumber != tileNumber)
        {
           
            Move move = new Move(selectedTile.TileNumber, tileNumber, selectedPiece.TypeNumber * playerTurn );
           // Debug.Log(representation.IsValidMove(move, playerTurn));
            if(representation.IsValidMove(move, playerTurn)){
                MakeMove(move);
                //Debug.Log("Move");
                Debug.Log(outcome);
                if (outcome == GameOutcome.UNDETERMINED)
                {
                    StartCoroutine(AITurnCoroutine());
                }
                    
            }

            selectedPiece = null;
            selectedTile = null;
            
            foreach(KopcoTileController tile in tiles){
                tile.Selected = false;
            }
        }
        else if(pieces[tileNumber] != null){
            tiles[tileNumber].Selected = !tiles[tileNumber].Selected;
            selectedPiece = pieces[tileNumber];
            selectedTile = tiles[tileNumber];
        }
    }

    void MakeMove(Move move){
        //Debug.Log(move.From + ", " + move.To + ", " + playerTurn);
        pieces[move.From].transform.position = tiles[move.To].transform.position;
       if(pieces[move.To] != null){
            Destroy(pieces[move.To].gameObject);
            pieces[move.To] = null;
       }
       pieces[move.To] = pieces[move.From];
       pieces[move.From] = null;
       //Debug.Log(move);
        representation.MakeMove(move, playerTurn);
        outcome = representation.GetGameOutcome();
        if(outcome == GameOutcome.UNDETERMINED){
            playerTurn *= -1;
        }
        UpdateInfoMessage();
        //Debug.Log(representation.GetGameOutcome());
        //Debug.Log(representation);
    }

    void UpdateInfoMessage(){
        if(outcome == GameOutcome.UNDETERMINED){
            string player = playerTurn == 1 ? "Human" : "AI";
        }
        else if(outcome == GameOutcome.DRAW){
        }
        else if(outcome == GameOutcome.PLAYER1){
        }
        else if(outcome == GameOutcome.PLAYER2){
        }
    }

    IEnumerator AITurnCoroutine()
    {
        //Debug.Log("AIStarts");
        yield return new WaitForSeconds(1f);
        Move aiMove = aiPlayer.GetMove(representation, playerTurn);
      // if(aiMove != null)
      // {
           //MakeMove(move);
      // }
    }
}
