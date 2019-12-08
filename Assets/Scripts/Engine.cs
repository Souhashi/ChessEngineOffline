using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    private static Engine _instance;
    public static Engine Instance { get { return _instance; } }
    public enum Status { FreeMove, PieceSelected, PieceMoving, CheckMate };
    public enum Player { Black, White };
    public Player current_player;
    public Status currentBoardStatus;
    public List<Piece> black_pieces;
    public List<Piece> white_pieces;
    // Start is called before the first frame update
    void Start()
    {
        GeneratePieces();
        currentBoardStatus = Status.FreeMove;
        current_player = Player.White;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void GeneratePieces()
    {
        black_pieces = new List<Piece>();
        white_pieces = new List<Piece>();
        GameObject[] board_pieces;
        board_pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (GameObject p in board_pieces)
        {
            Piece piece = p.GetComponent<Piece>();
            if (piece.pColor == Piece.Color.White)
            {
                white_pieces.Add(piece);
            }
            else
            {
                black_pieces.Add(piece);
            }
        }
    }
    public Piece GetPieceAtLocation(Vector3 pos)
    {
        foreach (Piece p in white_pieces)
        {
            Vector3 position = p.gameObject.transform.position;
            if (position.x == pos.x && position.z == pos.z) { return p; }

        }
        foreach (Piece p in black_pieces)
        {
            Vector3 position = p.gameObject.transform.position;
            if (position.x == pos.x && position.z == pos.z) { return p; }
        }
        return null;
    }
}


