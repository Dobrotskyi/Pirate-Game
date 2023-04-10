using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarShower : MonoBehaviour
{
    [SerializeField] private float _showBarDistance = 40f;
    [SerializeField] private GameObject _bar;
    private Transform _player;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (_player == null)
        {
            _bar.SetActive(false);
            this.enabled = false;
            return;
        }

        if (Vector3.Distance(_player.position, transform.position) <= _showBarDistance)
        {
            if (_bar.activeSelf == false)
                _bar.SetActive(true);
        }
        else
        {
            if (_bar.activeSelf == true)
                _bar.SetActive(false);
        }
    }
}
