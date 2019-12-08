using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    bool HasTile = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetTileStatus()
    {
        return HasTile;
    }

    public void SetTileStatus(bool status)
    {
        HasTile = status;
    }

    public Vector3 GetTilePos()
    {
        return gameObject.transform.position;
    }


}
