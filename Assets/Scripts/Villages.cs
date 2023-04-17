using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Villages : MonoBehaviour
{
    [SerializeField] private GameObject _ship;
    [SerializeField] private float _shipRespawnRate = 3f;
    private Vector2 _XZDissplacement = new Vector2(5, 5);

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

    private void OnDisable()
    {
        EnemyShipController.Destroyed -= RecalculateEnemies;
    }

    private void RecalculateEnemies()
    {
        StartCoroutine(ReplaceDestroyedShip());
    }

    private IEnumerator ReplaceDestroyedShip()
    {
        yield return new WaitForSeconds(_shipRespawnRate);
        List<Dock> docksNoShip = new List<Dock>();
        for (int i = 0; i < _docks.Count; i++)
        {
            if (_docks[i].AttachedShip == null)
                docksNoShip.Add(_docks[i]);
        }
        int index = new System.Random().Next(0, docksNoShip.Count);
        GameObject newShip = SpawnShip(docksNoShip[index].Transform.position, docksNoShip[index].Transform.rotation);
        docksNoShip[index].SetShip(newShip);
        yield break;
    }

    private GameObject SpawnShip(Vector3 position, Quaternion rotation)
    {
        GameObject ship = Instantiate(_ship);
        Vector3 spawnPosition = position;
        spawnPosition.y = 0.5f;
        spawnPosition.x += UnityEngine.Random.Range(-_XZDissplacement.x, _XZDissplacement.x);
        spawnPosition.z += UnityEngine.Random.Range(-_XZDissplacement.y, _XZDissplacement.y);
        ship.transform.position = spawnPosition;
        ship.transform.rotation = rotation;
        return ship;
    }
}
