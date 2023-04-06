using UnityEngine;
using System.Collections.Generic;


public class Villages : MonoBehaviour
{
    [SerializeField] private GameObject _ship;
    private List<Dock> _docks = new List<Dock>();

    public Vector3 GetRandomDock
    {
        get
        {
            Vector3 check = _docks[Random.Range(0, _docks.Count)].Pos;
            return check;
        }
    }

    private struct Dock
    {
        public Vector3 Pos;
        public GameObject AttachedShip;

        public Dock(Vector3 pos, GameObject ship)
        {
            Pos = pos;
            AttachedShip = ship;
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform dockPos = transform.GetChild(i).Find("Dock");
            GameObject ship = Instantiate(_ship);
            ship.transform.position = dockPos.position;
            ship.transform.rotation = dockPos.rotation;
            Dock dock = new Dock(dockPos.position, ship);
            _docks.Add(dock);
        }
    }
}
