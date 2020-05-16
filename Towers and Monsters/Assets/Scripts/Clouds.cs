using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{

    public GameObject[] _cloudsFirst;
    public GameObject[] _cloudsSecond;

    public float[] _speeds;

    private int index = 0;

    void Update()
    {
        index = 0;
        foreach (GameObject cloud in _cloudsFirst)
        {
            cloud.transform.Translate(new Vector3(_speeds[index], 0, 0));
            if (cloud.transform.position.x >= 1300)
                cloud.transform.position = new Vector3(-1300.0f, cloud.transform.position.y, cloud.transform.position.z);
            index += 1;
        }
        index = 0;
        foreach (GameObject cloud in _cloudsSecond)
        {
            cloud.transform.Translate(new Vector3(_speeds[index], 0, 0));
            if (cloud.transform.position.x >= 1300)
                cloud.transform.position = new Vector3(-1300.0f, cloud.transform.position.y, cloud.transform.position.z);
            index += 1;
        }
    }
}
