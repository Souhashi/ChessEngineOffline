using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private static BoardGenerator _instance;
    public static BoardGenerator Instance { get { return _instance; } }
    string enpassantb, enpassantw;
    Dictionary<string, Vector2Int> positions = new Dictionary<string, Vector2Int>();
    Dictionary<Vector2Int, string> inversepositions = new Dictionary<Vector2Int, string>();
    string piece, target_pos;

    string[,] Chessboard = new string[8,8] { {"R", "N", "B", "Q", "K", "B", "N", "R" },
                                             { "P", "P", "P", "P", "P", "P", "P", "P"},
                                             { " ", " ", " ", " ", " ", " ", " ", " "},
                                             { " ", " ", " ", " ", " ", " ", " ", " "},
                                             { " ", " ", " ", " ", " ", " ", " ", " "},
                                             { " ", " ", " ", " ", " ", " ", " ", " "},
                                             { "p", "p", "p", "p", "p", "p", "p", "p"},
                                             { "r", "n", "b", "q", "k", "b", "n", "r"}};

    public void SetPiece(string piece)
    {
        this.piece = piece;
    }

    public void SetPosition(string position)
    {
        target_pos = position;
        Debug.Log(piece + ", " + target_pos);
        int x = positions[piece].x;
        int y = positions[piece].y;

        int nx = positions[target_pos].x;
        int ny = positions[target_pos].y;

        string temp = Chessboard[y, x];

        Chessboard[y, x] = " ";
        
        Chessboard[ny, nx] = temp;
        Debug.Log(position.Substring(1));
        PossibleEP(piece, target_pos, Chessboard[ny, nx]);
    }

    public void PossibleEP(string p, string p1, string piece)
    {
        switch (GameManager.Instance.current_player)
        {
            case GameManager.Player.White:
                string ip = p.Substring(1);
                string tp = p1.Substring(1);
                string file = p.Substring(0, 1);
                if (ip == "2" && tp == "4" && piece == "P")
                {
                    enpassantw = file + "3";
                }
                else
                {
                    enpassantw = "-";
                }
                break;
            case GameManager.Player.Black:
                string bip = p.Substring(1);
                string btp = p1.Substring(1);
                string bfile = p.Substring(0, 1);
                if (bip == "7" && btp == "5" && piece == "p")
                {
                    enpassantb = bfile + "6";
                }
                else {
                    enpassantb = "-";
                }
                break;
        }
    }

    public void GenerateFEN()
    {
        string FEN = "";
        int emptysquares;
        for (int i = 7; i >= 0; i--)
        {
            emptysquares = 0;
            for (int j = 0; j < 8; j++)
            {
                
                if (Chessboard[i, j] == " ")
                {
                    emptysquares++;
                }
               
                else if(Chessboard[i, j] != " ")
                {
                    if (emptysquares != 0)
                    {
                        FEN += emptysquares + Chessboard[i, j];
                        emptysquares = 0;
                    }
                    else
                    {
                        FEN += Chessboard[i, j];
                    }

                }
               


            }
            if (emptysquares != 0 && emptysquares !=8)
            {
                FEN += emptysquares;
            }
            if (emptysquares == 8)
            {
                FEN += "8";
            }
            if (i != 0) { FEN += "/"; }
           
        }

        if (GameManager.Instance.current_player.ToString() == "White")
        {
            FEN += " b";
        }
        else
        {
            FEN += " w";
        }

        FEN += " " + GameManager.Instance.GenerateCastlingRights();

        if (GameManager.Instance.current_player.ToString() == "White")
        {
            FEN += " " + enpassantw;
        }
        else
        {
            FEN += " " + enpassantb;
        }
        FEN += " " + GameManager.Instance.halfmove;
        FEN += " " + GameManager.Instance.fullmove;

        if (GameManager.Instance.current_player.ToString() == GameManager.Instance.Human.ToString())
        {
            
            StockfishLauncher.Instance.FeedTheStockfish(FEN);
            
            
        }
        
    }

   

public void GetPoint(string piece)
    {
        int x = positions[piece].x;
        int y = positions[piece].y;
        Debug.Log(Chessboard[y,x]);
    }

    void BuildPositionLookup()
    {
        string[] ranks = { "a", "b", "c", "d", "e", "f", "g", "h" };
        string[] files = { "1", "2", "3", "4", "5", "6", "7", "8" };

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
               // Debug.Log(ranks[i]+files[j] + " " + Chessboard[i,j]);
                string coord = ranks[i] + files[j];
                Vector2Int num_coord = new Vector2Int(i, j);
                positions.Add(coord, num_coord);
                inversepositions.Add(num_coord, coord);
            }
        }
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

    
    void Start()
    {
        BuildPositionLookup();
        enpassantb = enpassantw = "-";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
