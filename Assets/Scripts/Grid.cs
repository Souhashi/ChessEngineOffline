using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector3 GetGridPoint(Vector3 point) {
        Vector3 grid_pos = new Vector3(point.x - transform.position.x, point.y, point.z - transform.position.z);
        return grid_pos;
    }


}
