using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Villages : MonoBehaviour
{
    [SerializeField] private GameObject _ship;
    [SerializeField] private float _shipRespawnRate = 3f;
    private List<Dock> _docks = new List<Dock>();

    public Vector3 GetRandomDock
    {
        get
        {
            Vector3 check = _docks[Random.Range(0, _docks.Count)].Transform.position;
            return check;
        }
    }

    private struct Dock
    {
        public Transform Transform;
        private GameObject _attachedShip;

        public Dock(Transform pos, GameObject ship)
        {
            Transform = pos;
            _attachedShip = ship;
        }

        public GameObject AttachedShip
        {
            get { return _attachedShip; }
        }

        public void SetShip(GameObject ship)
        {
            _attachedShip = ship;
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform dockPos = transform.GetChild(i).Find("Dock");
            GameObject ship = SpawnShip(dockPos.position, dockPos.rotation);
            Dock dock = new Dock(dockPos, ship);
            _docks.Add(dock);
        }

        EnemyShipController.Destroyed += RecalculateEnemies;
    }

    private GameObject SpawnShip(Vector3 position, Quaternion rotation)
    {
        GameObject ship = Instantiate(_ship);
        ship.transform.position = position;
        ship.transform.rotation = rotation;
        return ship;
    }

    private void OnDisable()
    {
        EnemyShipController.Destroyed -= RecalculateEnemies;
    }

    private void RecalculateEnemies()
    {
        StartCoroutine(ReplaceDestroyedShipShip());
    }

    private IEnumerator ReplaceDestroyedShipShip()
    {
        yield return new WaitForSeconds(_shipRespawnRate);
        for (int i = 0; i < _docks.Count; i++)
        {
            if (_docks[i].AttachedShip == null)
            {
                Debug.Log("Spawning new ship");
                GameObject ship = SpawnShip(_docks[i].Transform.position, _docks[i].Transform.rotation);
                _docks[i].SetShip(ship);
            }
        }
        yield break;
    }
}
