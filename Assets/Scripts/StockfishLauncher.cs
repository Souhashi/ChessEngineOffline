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
    // Start is called before the first frame update
    
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
        output.Clear();
        string position = "position fen " + FEN;
        string execute = "go movetime 1000";
        myProcess.StandardInput.WriteLine(position);
        myProcess.StandardInput.WriteLine(execute);
        Invoke("TimedFunction", 2);
        

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
