using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject DarkTile;
    public GameObject WhiteTile;
    public int gridwidth;
    public int gridheight;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCheckeredGrid(gridwidth, gridheight, DarkTile, WhiteTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCheckeredGrid(int w, int h, GameObject a, GameObject b)
    {
        Debug.Log("yOOOO");
        bool getDarktile = true;
        for (int i = (int)transform.position.x; i < w+ (int)transform.position.x; i++)
        {
            getDarktile = !getDarktile;
            for (int j = (int)transform.position.z; j < h+ (int)transform.position.z; j++)
            {
                getDarktile = !getDarktile;
               
                if (getDarktile)
                { GameObject g = Instantiate(a, new Vector3Int(i, 0, j), Quaternion.identity); }
                else
                { GameObject g = Instantiate(b, new Vector3Int(i, 0, j), Quaternion.identity); }


            }
        }
    }
}
