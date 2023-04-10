using UnityEngine;
using TMPro;
using System;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreCounter;

    private void OnEnable()
    {
        EnemyShipController.Destroyed += UpdateScore;
    }

    private void OnDisable()
    {
        EnemyShipController.Destroyed -= UpdateScore;
    }

    private void UpdateScore()
    {
        int score = Convert.ToInt32(_scoreCounter.text) + 1;
        _scoreCounter.text = score.ToString();
    }
}
