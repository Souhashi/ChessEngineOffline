using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterial : MonoBehaviour
{
    Material material;
    public Material attack_state;
    MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        material = mesh.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttackState()
    {
        mesh.material = attack_state;
    }
    public void SetNormalState()
    {
        mesh.material = material;
    }
}
