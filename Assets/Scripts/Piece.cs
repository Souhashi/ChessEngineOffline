using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    Material Piece_Material;
    public Material select_Material;
    MeshRenderer m_renderer;
    public enum Pieces { King, Queen, Bishop, Knight, Rook, Peon }
    public enum Color { White, Black};
    public Color pColor;
    public enum Status { Selected, Moving, Free};
    public Status c_status;
    public Pieces piecetype;
    bool isSelected = false;
    public bool hasMovedBefore = false;
    public bool EnPassantLeft = false;
    public bool EnPassantRight = false;
    public bool CanEnPassantLeft = true;
    public bool CanEnPassantRight = true;
    public int num_of_moves;
    public int num_of_turns = -1;
    public Vector3 startposition;
    List<Vector3> kingPositions;
    List<Vector3> knightPositions;
    List<Vector3> positions;
    List<Vector3> checkMatePositions;
    // Start is called before the first frame update
    void Start()
    {
        startposition = transform.position;
        InitialiseKing();
        InitialiseKnight();
        m_renderer = GetComponent<MeshRenderer>();
        Piece_Material = m_renderer.material;
        c_status = Status.Free;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPiece(Vector3 npos)
    {
        if (transform.position != npos)
        {

            transform.position = new Vector3(npos.x, transform.position.y, npos.z);
        }
        
        
    }

    

   public bool selected()
    {
        return isSelected;
    }

    public void ToggleSelection(bool s)
    {
        isSelected = s;
    }

    void InitialiseKing()
    {
        kingPositions = new List<Vector3>();
        kingPositions.Add(new Vector3(1, 0, 0));
        kingPositions.Add(new Vector3( - 1, 0, 0));
        kingPositions.Add(new Vector3(0, 0, 1));
        kingPositions.Add(new Vector3(0,0, - 1));
        kingPositions.Add(new Vector3( - 1,0, - 1));
        kingPositions.Add(new Vector3( 1, 0, - 1));
        kingPositions.Add(new Vector3(- 1, 0,  1));
        kingPositions.Add(new Vector3(1, 0, 1));

    }

    void InitialiseKnight()
    {
        knightPositions = new List<Vector3>();
        knightPositions.Add(new Vector3(2, 0, 1));
        knightPositions.Add(new Vector3(-2, 0, 1));
        knightPositions.Add(new Vector3(-2, 0, -1));
        knightPositions.Add(new Vector3(2, 0, -1));
        knightPositions.Add(new Vector3(1, 0, 2));
        knightPositions.Add(new Vector3(-1, 0, 2));
        knightPositions.Add(new Vector3(-1, 0, -2));
        knightPositions.Add(new Vector3(1, 0, -2));

    }

    public void SetStatusMaterial(Status s)
    {
        c_status = s;
        switch (s)
        {
            case Status.Free:
                m_renderer.material = Piece_Material;
                break;
            case Status.Selected:
                m_renderer.material = select_Material;
                break;
            case Status.Moving:
                m_renderer.material = Piece_Material;
                break;
        }
    }

    public List<Vector3> GetThreats()
    {
        positions = new List<Vector3>();
        if (piecetype == Pieces.King)
        {
            GenerateUpPositions(transform.position);
            GenerateDownPositions(transform.position);
            GenerateLeftPositions(transform.position);
            GenerateRightPositions(transform.position);
            GenerateDiagonalUpRight(transform.position);
            GenerateDiagonalUpLeft(transform.position);
            GenerateDiagonalDownRight(transform.position);
            GenerateDiagonalUpLeft(transform.position);
            positions.RemoveAll(pos => pos.x < 1 || pos.x > 8 || pos.z < 11 || pos.z > 18);
            return positions;

        }
        return null;
    }

    void GenerateDiagonalUpRight(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z + i);

            
            if (!GameManager.Instance.HasPiece(up))
            {
               
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
                
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }
    }

    void GenerateDiagonalUpRightAIn(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z + i);


            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }
    }

    void GenerateDiagonalUpLeft(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x - i, GameManager.Instance.g.y, point.z + i);

            
            if (!GameManager.Instance.HasPiece(up))
            {
                
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
               
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }
    }

    void GenerateDiagonalUpLeftAIn(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x - i, GameManager.Instance.g.y, point.z + i);


            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }
    }
    void GenerateDiagonalDownRight(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z - i);

            if (!GameManager.Instance.HasPiece(up))
            {
                
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
                
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }
    }

    void GenerateDiagonalDownRightAIn(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z - i);

            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }
    }
    void GenerateDiagonalDownLeft(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x - i, GameManager.Instance.g.y, point.z - i);

           
            if (!GameManager.Instance.HasPiece(up))
            {
                
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
                
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }
    }

    void GenerateDiagonalDownLeftAIn(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x - i, GameManager.Instance.g.y, point.z - i);


            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }
    }

    void GenerateUpPositions(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x, GameManager.Instance.g.y, point.z + i);
           
           
            if (!GameManager.Instance.HasPiece(up))
            {
               
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
                
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }
    }

    void GenerateUpPositionsAIn(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x, GameManager.Instance.g.y, point.z + i);


            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }
    }

    void GenerateDownPositions(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x, GameManager.Instance.g.y, point.z - i);

            
            if (!GameManager.Instance.HasPiece(up))
            {
               
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
              
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }

    }

    void GenerateDownPositionsAIn(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x, GameManager.Instance.g.y, point.z - i);


            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }

    }

    void GenerateLeftPositions(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x-i, GameManager.Instance.g.y, point.z );

           
            if (!GameManager.Instance.HasPiece(up))
            {
                
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
                
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }
    }

    void GenerateLeftPositionsAIn(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x - i, GameManager.Instance.g.y, point.z);


            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }
    }

    void GenerateRightPositions(Vector3 point)
    {

        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z);

            //Debug.Log(up);
            if (!GameManager.Instance.HasPiece(up))
            {
               
                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {
               
                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                
                break;
            }

        }

    }

    void GenerateRightPositionsAIn(Vector3 point)
    {

        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z);

            //Debug.Log(up);
            if (!GameManager.Instance.HasPiece(up))
            {

                positions.Add(up);
            }
            if (GameManager.Instance.EnemyPieceAtLocation(up, pColor))
            {

                positions.Add(up);
                break;
            }
            if (GameManager.Instance.HasPiece(up))
            {
                positions.Add(up);
                break;
            }

        }

    }

    void GenXRAYDown(Vector3 point)
    {
        for (int i = 1; i <= 8; i++)
        {
            Vector3 up = new Vector3(point.x, GameManager.Instance.g.y, point.z - i);

            //Debug.Log(up);
           

                checkMatePositions.Add(up);
            

        }
    }

    void GenXRAYUp(Vector3 point)
    {
        for (int i = 1; i <= 8; i++)
        {
            Vector3 up = new Vector3(point.x, GameManager.Instance.g.y, point.z + i);

            //Debug.Log(up);
            

                checkMatePositions.Add(up);
            

        }
    }

    void GenXRAYLeft(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x-i, GameManager.Instance.g.y, point.z);

           // Debug.Log(up);
           
                
                checkMatePositions.Add(up);
            

        }
    }

    void GenXRAYRight(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z);

           // Debug.Log(up);
            
                
                checkMatePositions.Add(up);
           

        }
    }
    void GenXRAYDURight(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z+i);

            //Debug.Log(up);
            
               
                checkMatePositions.Add(up);
            
        }
    }
    void GenXRAYDULeft(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x - i, GameManager.Instance.g.y, point.z + i);

            //Debug.Log(up);
            
                
                checkMatePositions.Add(up);
           

        }
    }
    void GenXRAYDDLeft(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x - i, GameManager.Instance.g.y, point.z - i);

            //Debug.Log(up);
            
                
                checkMatePositions.Add(up);
           

        }
    }
    void GenXRAYDDRight(Vector3 point)
    {
        for (int i = 1; i <= 7; i++)
        {
            Vector3 up = new Vector3(point.x + i, GameManager.Instance.g.y, point.z - i);

            //Debug.Log(up);
            
                
                checkMatePositions.Add(up);
           

        }
    }

    

    public List<Vector3> GetOpKingsPositions(Vector3 point, GameManager.Player pl)
    {
        Debug.Log("King at: " + point);
        
        List<Vector3> positions = new List<Vector3>();
        foreach (Vector3 p in kingPositions)
        {
            Vector3 pos = p + point;
            if (GameManager.Instance.HasPiece(pos)==false || GameManager.Instance.EnemyPieceAtLocationOpPlayer(pos, pl) == false)
            {
                positions.Add(pos);

            }
        }
        Debug.Log("Before: " + positions.Count);
       positions.RemoveAll(pos => pos.x < 1 || pos.x > 8 || pos.z < 11 || pos.z > 18);
        Debug.Log("After: " + positions.Count);
        return positions;
    }
    
    public List<Vector3> GetPiecePositions(Vector3 point)
    {
        positions = new List<Vector3>();
        
        switch (piecetype)
        {
            case Pieces.Peon:
                if (pColor == Piece.Color.White)
                {
                    Vector3 pos = new Vector3(point.x, GameManager.Instance.g.y, point.z - 1);
                    Vector3 spos = new Vector3(point.x, GameManager.Instance.g.y, point.z - 2);
                    Vector3 dpos = new Vector3(point.x - 1, GameManager.Instance.g.y, point.z - 1);
                    Vector3 fdpos = new Vector3(point.x + 1, GameManager.Instance.g.y, point.z - 1);
                    if (!GameManager.Instance.HasPiece(pos))
                    {
                        positions.Add(pos);
                    }
                    if (GameManager.Instance.EnemyPieceAtLocation(dpos,pColor) || (EnPassantLeft && CanEnPassantLeft))
                    {
                        positions.Add(dpos);
                    }
                    if (GameManager.Instance.EnemyPieceAtLocation(fdpos, pColor) || (EnPassantRight && CanEnPassantRight))
                    {
                        positions.Add(fdpos);
                    }
                    if (!GameManager.Instance.HasPiece(spos)&& !GameManager.Instance.HasPiece(pos) && startposition.x == transform.position.x && startposition.z == transform.position.z)
                    {
                        
                        positions.Add(spos);
                        
                    }
                    
                }
                if (pColor == Piece.Color.Black)
                {
                    Vector3 pos = new Vector3(point.x, GameManager.Instance.g.y, point.z + 1);
                    Vector3 spos = new Vector3(point.x, GameManager.Instance.g.y, point.z + 2);
                    Vector3 dpos = new Vector3(point.x + 1, GameManager.Instance.g.y, point.z + 1);
                    Vector3 fdpos = new Vector3(point.x - 1, GameManager.Instance.g.y, point.z + 1);
                    if (!GameManager.Instance.HasPiece(pos))
                    {
                        positions.Add(pos);
                    }
                    if (GameManager.Instance.EnemyPieceAtLocation(dpos,pColor) || (EnPassantRight && CanEnPassantRight))
                    {
                        positions.Add(dpos);
                    }
                    if (GameManager.Instance.EnemyPieceAtLocation(fdpos,pColor) || (EnPassantLeft&& CanEnPassantLeft))
                    {
                        positions.Add(fdpos);
                    }
                    if (!GameManager.Instance.HasPiece(spos) &&  !GameManager.Instance.HasPiece(pos) && startposition.x == transform.position.x && startposition.z == transform.position.z)
                    {
                        positions.Add(spos);
                        
                    }
                }
                break;
            case Pieces.King:
                foreach (Vector3 p in kingPositions)
                {
                    Vector3 pos = p + point;
                    if (!GameManager.Instance.HasPiece(pos) || GameManager.Instance.EnemyPieceAtLocation(pos,pColor))
                    {
                        positions.Add(pos);
                        
                    }
                }
                //Debug.Log(positions.Count + "KING");
                break;
            case Pieces.Rook:
                GenerateUpPositions(point);
                GenerateDownPositions(point);
                GenerateLeftPositions(point);
                GenerateRightPositions(point);
                break;
            case Pieces.Bishop:
                GenerateDiagonalUpRight(point);
                GenerateDiagonalUpLeft(point);
                GenerateDiagonalDownRight(point);
                GenerateDiagonalDownLeft(point);
                break;
            case Pieces.Knight:
                foreach (Vector3 p in knightPositions)
                {
                    Vector3 pos = p + point;
                    if (!GameManager.Instance.HasPiece(pos) || GameManager.Instance.EnemyPieceAtLocation(pos,pColor))
                    {
                        positions.Add(pos);
                    }
                }
                break;
            case Pieces.Queen:
                GenerateUpPositions(point);
                GenerateDownPositions(point);
                GenerateLeftPositions(point);
                GenerateRightPositions(point);
                GenerateDiagonalUpRight(point);
                GenerateDiagonalUpLeft(point);
                GenerateDiagonalDownRight(point);
                GenerateDiagonalDownLeft(point);
                break;
        }
        positions.RemoveAll(pos => pos.x < 1 || pos.x > 8 || pos.z < 11 || pos.z > 18);
        //Debug.Log(positions.Count +": " +piecetype);
        return positions;
    }

    public List<Vector3> GenerateCheckMateMoves(Vector3 point)
    {
        //Debug.Log(piecetype + " " + pColor);
        checkMatePositions = new List<Vector3>();
        switch (piecetype)
        {
            case Pieces.Peon:
                if (pColor == Piece.Color.White)
                {
                    Vector3 pos = new Vector3(point.x, GameManager.Instance.g.y, point.z - 1);
                    Vector3 spos = new Vector3(point.x, GameManager.Instance.g.y, point.z - 2);
                    Vector3 dpos = new Vector3(point.x - 1, GameManager.Instance.g.y, point.z - 1);
                    Vector3 fdpos = new Vector3(point.x + 1, GameManager.Instance.g.y, point.z - 1);
                    if (!GameManager.Instance.HasPiece(pos))
                    {
                        checkMatePositions.Add(pos);
                    }
                    
                    if (!GameManager.Instance.HasPiece(spos) && !GameManager.Instance.HasPiece(pos) && startposition.x == transform.position.x && startposition.z == transform.position.z&& !hasMovedBefore)
                    {
                        checkMatePositions.Add(spos);
                        
                    }
                    checkMatePositions.Add(dpos);
                    checkMatePositions.Add(fdpos);

                }
                if (pColor == Piece.Color.Black)
                {
                    Vector3 pos = new Vector3(point.x, GameManager.Instance.g.y, point.z + 1);
                    Vector3 spos = new Vector3(point.x, GameManager.Instance.g.y, point.z + 2);
                    Vector3 dpos = new Vector3(point.x + 1, GameManager.Instance.g.y, point.z + 1);
                    Vector3 fdpos = new Vector3(point.x - 1, GameManager.Instance.g.y, point.z + 1);
                    if (!GameManager.Instance.HasPiece(pos))
                    {
                        checkMatePositions.Add(pos);
                    }
                    
                    if (!GameManager.Instance.HasPiece(spos) && !GameManager.Instance.HasPiece(pos) && startposition.x == transform.position.x && startposition.z == transform.position.z && !hasMovedBefore)
                    {
                       checkMatePositions.Add(spos);
                        
                    }
                    checkMatePositions.Add(dpos);
                    checkMatePositions.Add(fdpos);


                }
                break;
            case Pieces.King:
                foreach (Vector3 p in kingPositions)
                {
                    Vector3 pos = p + point;
                   
                    
                        positions.Add(pos);

                    
                }
                //Debug.Log(positions.Count + "KING");
                break;
            case Pieces.Knight:

                foreach (Vector3 p in knightPositions)
                {
                    Vector3 pos = p + point;
                   
                   
                        checkMatePositions.Add(pos);
                    
                }
                break;
            case Pieces.Bishop:
                GenXRAYDDLeft(point);
                GenXRAYDDRight(point);
                GenXRAYDULeft(point);
                GenXRAYDURight(point);
                break;
            case Pieces.Rook:
                 GenXRAYLeft(point);
                GenXRAYRight(point);
                GenXRAYUp(point);
                GenXRAYDown(point);
                break;
            case Pieces.Queen:
                GenXRAYDDLeft(point);
                GenXRAYDDRight(point);
                GenXRAYDULeft(point);
                GenXRAYDURight(point);
                GenXRAYLeft(point);
                GenXRAYRight(point);
                GenXRAYUp(point);
                GenXRAYDown(point);
                break;
        }
        checkMatePositions.RemoveAll(pos => pos.x < 1 || pos.x > 8 || pos.z < 11 || pos.z > 18);
        return checkMatePositions;
    }

    public List<Vector3> GetPositions(Vector3 point)
    {
        positions = new List<Vector3>();

        switch (piecetype)
        {
            case Pieces.Peon:
                if (pColor == Piece.Color.White)
                {
                    Vector3 pos = new Vector3(point.x, GameManager.Instance.g.y, point.z - 1);
                    Vector3 spos = new Vector3(point.x, GameManager.Instance.g.y, point.z - 2);
                    Vector3 dpos = new Vector3(point.x - 1, GameManager.Instance.g.y, point.z - 1);
                    Vector3 fdpos = new Vector3(point.x + 1, GameManager.Instance.g.y, point.z - 1);
                    if (!GameManager.Instance.HasPiece(pos))
                    {
                        positions.Add(pos);
                    }
                    
                    if (!GameManager.Instance.HasPiece(spos) && !GameManager.Instance.HasPiece(pos) && startposition.x == transform.position.x && startposition.z == transform.position.z && !hasMovedBefore)
                    {
                        positions.Add(spos);

                    }
                    positions.Add(dpos);
                    positions.Add(fdpos);

                }
                if (pColor == Piece.Color.Black)
                {
                    Vector3 pos = new Vector3(point.x, GameManager.Instance.g.y, point.z + 1);
                    Vector3 spos = new Vector3(point.x, GameManager.Instance.g.y, point.z + 2);
                    Vector3 dpos = new Vector3(point.x + 1, GameManager.Instance.g.y, point.z + 1);
                    Vector3 fdpos = new Vector3(point.x - 1, GameManager.Instance.g.y, point.z + 1);
                    if (!GameManager.Instance.HasPiece(pos))
                    {
                        positions.Add(pos);
                    }
                    
                    if (!GameManager.Instance.HasPiece(spos) && !GameManager.Instance.HasPiece(pos) && startposition.x == transform.position.x && startposition.z == transform.position.z && !hasMovedBefore)
                    {
                        positions.Add(spos);

                    }
                    positions.Add(dpos);
                    positions.Add(fdpos);


                }
                break;
            case Pieces.King:
                foreach (Vector3 p in kingPositions)
                {
                    Vector3 pos = p + point;
                    
                        positions.Add(pos);

                    
                }
                //Debug.Log(positions.Count + "KING");
                break;
            case Pieces.Rook:
                GenerateDownPositionsAIn(point);
                GenerateLeftPositionsAIn(point);
                GenerateUpPositionsAIn(point);
                GenerateDownPositionsAIn(point);

                break;
            case Pieces.Bishop:
                GenerateDiagonalUpRightAIn(point);
                GenerateDiagonalUpLeftAIn(point);
                GenerateDiagonalDownRightAIn(point);
                GenerateDiagonalDownLeftAIn(point);
                break;
            case Pieces.Knight:
                foreach (Vector3 p in knightPositions)
                {
                    Vector3 pos = p + point;
                   
                   
                        positions.Add(pos);
                    
                }
                break;
            case Pieces.Queen:
                GenerateDiagonalUpRightAIn(point);
                GenerateDiagonalUpLeftAIn(point);
                GenerateDiagonalDownRightAIn(point);
                GenerateDiagonalDownLeftAIn(point);
                GenerateDownPositionsAIn(point);
                GenerateLeftPositionsAIn(point);
                GenerateUpPositionsAIn(point);
                GenerateDownPositionsAIn(point);
                break;
        }
        positions.RemoveAll(pos => pos.x < 1 || pos.x > 8 || pos.z < 11 || pos.z > 18);
       // Debug.Log(positions.Count + ": " + piecetype);
        return positions;
    }
}
