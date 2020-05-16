using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour
{
    public GameObject[] _borders;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_borders[0].transform.position, _borders[1].transform.position);
        Gizmos.DrawLine(_borders[1].transform.position, _borders[2].transform.position);
        Gizmos.DrawLine(_borders[2].transform.position, _borders[3].transform.position);
        Gizmos.DrawLine(_borders[3].transform.position, _borders[0].transform.position);

        Gizmos.DrawLine(_borders[4].transform.position, _borders[5].transform.position);
        Gizmos.DrawLine(_borders[5].transform.position, _borders[6].transform.position);
        Gizmos.DrawLine(_borders[6].transform.position, _borders[7].transform.position);
        Gizmos.DrawLine(_borders[7].transform.position, _borders[4].transform.position);

        Gizmos.DrawLine(_borders[0].transform.position, _borders[4].transform.position);
        Gizmos.DrawLine(_borders[1].transform.position, _borders[5].transform.position);
        Gizmos.DrawLine(_borders[2].transform.position, _borders[6].transform.position);
        Gizmos.DrawLine(_borders[3].transform.position, _borders[7].transform.position);
    }
}
