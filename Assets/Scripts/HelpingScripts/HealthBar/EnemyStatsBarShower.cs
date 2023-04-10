using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsBarShower : MonoBehaviour
{
    [SerializeField] private float _showBarDistance = 40f;
    [SerializeField] private List<GameObject> _bars;
    private Transform _player;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (_player == null)
        {
            foreach(GameObject bar in _bars)
                bar.SetActive(false);
            this.enabled = false;
            return;
        }

        if (Vector3.Distance(_player.position, transform.position) <= _showBarDistance)
        {
            if (_bars[0].activeSelf == false)
                foreach(GameObject bar in _bars)
                    bar.SetActive(true);
        }
        else
        {
            if (_bars[0].activeSelf == true)
                foreach(GameObject bar in _bars)
                    bar.SetActive(false);
        }
    }
}
