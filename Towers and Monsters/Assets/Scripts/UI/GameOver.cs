using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text _roundDone = null;
    [SerializeField] private Text _enemiesKilled = null;

    public void EndGame(int wave, int enemies)
    {
        _roundDone.text = "Round Done : " + wave.ToString();
        _enemiesKilled.text = "Killed         : " + enemies.ToString();
    }
}
