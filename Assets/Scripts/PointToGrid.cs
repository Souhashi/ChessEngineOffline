using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToGrid : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject g;
    public GameObject m;
    Vector3 point;
    Vector3 move;
    GameObject selectiontile;
    GameObject movetile;
    GameObject s;
    GameObject enemypiece;
    Piece p;
    List<Vector3> positions;
    List<GameObject> movetiles;
    bool canCastle = true;
    string s_name;
    void Start()
    {
        GenerateMoveTiles();
    }

    // Update is called once per frame
    void Update()
    {
        //GameManager.Instance.CheckCameras();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100);
        if (GameManager.Instance.currentBoardStatus == GameManager.Status.FreeMove)
        {
            

            if (movetile != null && movetile.activeSelf)
            {
                movetile.SetActive(false);
            }
            if (hit.collider != null)
            {
                point = hit.collider.transform.position;
                move = hit.collider.transform.position;
                s_name = hit.collider.name;
                if (selectiontile != null)
                {

                    selectiontile.transform.position = point;
                    //Debug.Log(point);

                }
                else
                {
                    selectiontile = Instantiate(g, point, Quaternion.identity);
                }

            }
            if (Input.GetMouseButtonDown(0))
            {
                s = GameManager.Instance.GetPieceAtLocation(point);
                if (s != null)
                {
                    p = s.GetComponent<Piece>();
                    if (p != null)
                    {
                        if (p.pColor.ToString() == GameManager.Instance.current_player.ToString())
                        {
                            //Debug.Log(p.piecetype);
                            BoardGenerator.Instance.SetPiece(s_name);
                            GameManager.Instance.CanPawnEP();
                            p.SetStatusMaterial(Piece.Status.Selected);
                            GameManager.Instance.currentPiece = s;
                            selectiontile.transform.position = point;
                            positions = p.GetPiecePositions(point);
                            //Debug.Log(positions.Count);
                            Debug.Log(p.num_of_turns);
                            foreach (Vector3 p in positions)
                            {
                              
                                    MatchTileWithPos(p);
                                
                                                                
                               
                            }
                            GameManager.Instance.currentBoardStatus = GameManager.Status.PieceSelected;
                        }
                    }
                }
               
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.ResetBoard();
            }

        }
        if (GameManager.Instance.currentBoardStatus == GameManager.Status.PieceSelected)
        {
            
            /* if (movetile != null && !movetile.activeSelf)
             {
                 movetile.SetActive(true);
             }*/
            if (hit.collider != null)
            {
                point = hit.collider.transform.position;
                GameManager.Instance.currentpos = hit.collider.name;
                    
               
}
            
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("hi");
                BoardGenerator.Instance.GetPoint(GameManager.Instance.currentpos);
                if (p != null)
                {
                    //Debug.Log("Hi"+point);
                    if (isPointinPos(point)) {
                        enemypiece = GameManager.Instance.GetPieceAtLocation(point);
                        GameManager.Instance.currentBoardStatus = GameManager.Status.PieceMoving;
                        //Debug.Log("Move: " + point);
                        
                    }
                   
                    

                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                p.SetStatusMaterial(Piece.Status.Free);
                DeactivateTiles();
                GameManager.Instance.currentBoardStatus = GameManager.Status.FreeMove;

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
                
                positions = p.GetPiecePositions(point);
                Debug.Log(positions.Count);
                
                DeactivateTiles();
                if (p.piecetype == Piece.Pieces.Rook&& Input.GetKey(KeyCode.Q) && GameManager.Instance.CanCastle(s))
                {
                    GameManager.Instance.Castle(p, point);
                    p.hasMovedBefore = true;
                    GameManager.Instance.halfmove++;
                }
                if (p.piecetype == Piece.Pieces.Peon)
                {
                    GameManager.Instance.isAboutToEP(p, point);
                    p.SetPiece(point);
                    p.hasMovedBefore = true;
                    GameManager.Instance.halfmove = 0;
                }
                else
                {
                    p.SetPiece(point);
                    p.hasMovedBefore = true;
                    if (enemypiece == null) { GameManager.Instance.halfmove++; }
                    
                }
                GameManager.Instance.ResetEP();
                Debug.Log(p.num_of_turns);
                p.num_of_moves++;

                //GameManager.Instance.ResetEP();
                BoardGenerator.Instance.SetPosition(GameManager.Instance.currentpos);
                List<GameObject> t = GameManager.Instance.IsKingInCheck();
                    if (t.Count != 0)
                    {
                      
                                GameManager.Instance.SetCheck(true);
                                Debug.Log("King in check " + "Player:" + GameManager.Instance.current_player);
                                GameManager.Instance.ScanForCheckMate(GameManager.Instance.current_player, t);
                                                  
                    }

                GameManager.Instance.SetCheck(false);
                GameManager.Instance.isScanComplete = false;
                p.SetStatusMaterial(Piece.Status.Free);
                if (GameManager.Instance.canBePromoted(p))
                {
                    Debug.Log(p.piecetype + " can be promoted!");
                    GameManager.Instance.ActivatePromotionMenu();
                    Time.timeScale = 0f;
                }
                Debug.Log(GameManager.Instance.current_player);

                if (!GameManager.Instance.IsPaused)
                {
                  GameManager.Instance.currentBoardStatus = GameManager.Status.FreeMove;
                    BoardGenerator.Instance.GenerateFEN();
                    GameManager.Instance.NextPlayer();
                }
            }

        }
        if (GameManager.Instance.currentBoardStatus == GameManager.Status.CheckMate)
        {

        }
        

    }

    bool isPointinPos(Vector3 pos)
    {
        foreach (Vector3 p in positions)
        {
            if (pos.x == p.x && pos.z == p.z)
            {
                return true;
            }
        }
        return false;
    }

    void GenerateMoveTiles()
    {
        movetiles = new List<GameObject>();
        for (int i = (int)transform.position.x; i < (int)transform.position.x + 8; i++)
        {
            for (int j = (int)transform.position.z; j < (int)transform.position.z + 8; j++)
            {
                GameObject d = Instantiate(m, new Vector3(i, transform.position.y, j), Quaternion.identity);
                d.SetActive(false);
                movetiles.Add(d);
            }
        }
    }

    void MatchTileWithPos(Vector3 pos) 
    {
        foreach (GameObject d in movetiles)
        {
            if (pos.x == d.transform.position.x && pos.z == d.transform.position.z)
            {
                d.SetActive(true);
               
            }
        }
    }

    void DeactivateTiles()
    {
        foreach (GameObject d in movetiles)
        {
            if (d.activeSelf)
            {
                SwitchMaterial sm = d.GetComponent<SwitchMaterial>();
                sm.SetNormalState();
                d.SetActive(false);
            }
        }
    }

}
