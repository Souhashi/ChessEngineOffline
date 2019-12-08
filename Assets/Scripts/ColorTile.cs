using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTile : MonoBehaviour
{
    // Start is called before the first frame update
    private Color startcolor;
    MeshRenderer renderer;
    void Start()
    {
        renderer =GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnMouseEnter()

    {
        startcolor = renderer.material.color;
        renderer.material.color = Color.red;
    }
    void OnMouseExit()
    {
        renderer.material.color = startcolor;
    }
}
