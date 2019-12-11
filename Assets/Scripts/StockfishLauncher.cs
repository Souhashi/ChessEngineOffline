using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Text;
using UnityEngine;

public class StockfishLauncher : MonoBehaviour
{
    Process myProcess;
    private static StockfishLauncher _instance;
    public static StockfishLauncher Instance { get { return _instance; } }
    public static StringBuilder output = new StringBuilder();
    GameObject s, enemypiece;
    Piece p;
    string s_name;
    // Start is called before the first frame update
    string targetpiece, targetsquare;
    void Start()
    {
        myProcess = new Process();
        myProcess.StartInfo = new ProcessStartInfo("C:\\Users\\SOUHASHI\\Downloads\\stockfish-10-win\\stockfish-10-win\\Windows\\stockfish_10_x64.exe");
        myProcess.OutputDataReceived += new DataReceivedEventHandler(MyProcOutputHandler);



        myProcess.StartInfo.UseShellExecute = false;
       
        myProcess.StartInfo.RedirectStandardInput = true;
        myProcess.StartInfo.RedirectStandardOutput = true;
        myProcess.StartInfo.RedirectStandardError = true;
        

        myProcess.Start();
        myProcess.BeginOutputReadLine();
        
        myProcess.BeginErrorReadLine();
        myProcess.StandardInput.WriteLine("ucinewgame");

        
    }
    void TimedFunction()
    {
        string response = output.ToString();
        string[] responses = response.Split('|');
        string bestmove = responses[responses.Length - 1];
        string[] bestmoves = bestmove.Split(' ');
        bestmove = bestmoves[1];
        UnityEngine.Debug.Log(bestmove);
        targetpiece = bestmove.Substring(0, 2);
        targetsquare = bestmove.Substring(2, 2);
        UnityEngine.Debug.Log(targetpiece+" "+targetsquare);
        PerformMove(targetpiece, targetsquare);
    }

    Transform GetChild(Transform t, string name)
    {
        int count = t.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = t.transform.GetChild(i);
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
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
    public void FeedTheStockfish(string FEN)
    {
        UnityEngine.Debug.Log(FEN);
        output.Clear();
        string position = "position fen " + FEN;
        string execute = "go movetime 1000";
        myProcess.StandardInput.WriteLine(position);
        myProcess.StandardInput.WriteLine(execute);
        Invoke("TimedFunction", 2);
        

    }

    void PerformMove(string targetpiece, string targetsquare)
    {
        UnityEngine.Debug.Log(targetpiece + " " + targetsquare);
        Vector3 piece_point = GetChild(GameManager.Instance.transform,targetpiece).position;
        Vector3 target_point = GetChild(GameManager.Instance.transform, targetsquare).position;
        UnityEngine.Debug.Log(piece_point + " " + target_point);
        if (GameManager.Instance.currentBoardStatus == GameManager.Status.FreeMove)
        {
            s_name = targetpiece;
            s = GameManager.Instance.GetPieceAtLocation(piece_point);
            if (s != null)
            {
                p = s.GetComponent<Piece>();
                if (p != null)
                {
                    if (p.pColor.ToString() == "Black")
                    {
                        //Debug.Log(p.piecetype);
                        BoardGenerator.Instance.SetPiece(s_name);
                        GameManager.Instance.CanPawnEP();
                        p.SetStatusMaterial(Piece.Status.Selected);
                        GameManager.Instance.currentPiece = s;
                        
                        
                        GameManager.Instance.currentBoardStatus = GameManager.Status.PieceSelected;
                    }
                }
            }

            if (GameManager.Instance.currentBoardStatus == GameManager.Status.PieceSelected)
            {
                GameManager.Instance.currentpos = targetsquare;
                BoardGenerator.Instance.GetPoint(GameManager.Instance.currentpos);
                if (p != null)
                {
                   
                        enemypiece = GameManager.Instance.GetPieceAtLocation(target_point);
                        GameManager.Instance.currentBoardStatus = GameManager.Status.PieceMoving;
             
                }
            }

            if (GameManager.Instance.currentBoardStatus == GameManager.Status.PieceMoving)
            {

                if (p != null)
                {
                    if (enemypiece != null)
                    {
                        GameManager.Instance.Allpieces.Remove(enemypiece);
                        GameManager.Instance.Capture(enemypiece);
                        GameManager.Instance.halfmove = 0;
                    }

                    if (p.piecetype == Piece.Pieces.Rook && Input.GetKey(KeyCode.Q) && GameManager.Instance.CanCastle(s))
                    {
                        GameManager.Instance.Castle(p, target_point);
                        p.hasMovedBefore = true;
                        GameManager.Instance.halfmove++;
                    }
                    if (p.piecetype == Piece.Pieces.Peon)
                    {
                        GameManager.Instance.isAboutToEP(p, target_point);
                        p.SetPiece(target_point);
                        p.hasMovedBefore = true;
                        GameManager.Instance.halfmove = 0;
                    }
                    else
                    {
                        p.SetPiece(target_point);
                        p.hasMovedBefore = true;
                        if (enemypiece == null) { GameManager.Instance.halfmove++; }

                    }
                    GameManager.Instance.ResetEP();
                   
                    p.num_of_moves++;

                    //GameManager.Instance.ResetEP();
                    BoardGenerator.Instance.SetPosition(GameManager.Instance.currentpos);
                    List<GameObject> t = GameManager.Instance.IsKingInCheck();
                    if (t.Count != 0)
                    {

                        GameManager.Instance.SetCheck(true);
                        UnityEngine.Debug.Log("King in check " + "Player:" + GameManager.Instance.current_player);
                        GameManager.Instance.ScanForCheckMate(GameManager.Instance.current_player, t);

                    }

                    GameManager.Instance.SetCheck(false);
                    GameManager.Instance.isScanComplete = false;
                    p.SetStatusMaterial(Piece.Status.Free);
                    if (GameManager.Instance.canBePromoted(p))
                    {
                        UnityEngine.Debug.Log(p.piecetype + " can be promoted!");
                        GameManager.Instance.ActivatePromotionMenu();
                        Time.timeScale = 0f;
                    }
                    UnityEngine.Debug.Log(GameManager.Instance.current_player);

                    if (!GameManager.Instance.IsPaused)
                    {
                        GameManager.Instance.currentBoardStatus = GameManager.Status.FreeMove;
                        BoardGenerator.Instance.GenerateFEN();
                        GameManager.Instance.NextPlayer();
                    }
                }

            }

        }
    }
    private static void MyProcOutputHandler(object sendingProcess,
           DataReceivedEventArgs outLine)
    {
        // Collect the sort command output. 
        if (!String.IsNullOrEmpty(outLine.Data))
        {
            //UnityEngine.Debug.Log(outLine.Data);
            output.Append("|" + outLine.Data  );
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
