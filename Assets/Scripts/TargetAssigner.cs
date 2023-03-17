using System.Collections.Generic;
using UnityEngine;

public class TargetAssigner : MonoBehaviour
{
    [SerializeField] private GameObject _mainLeftTarget;
    [SerializeField] private GameObject _mainRightTarget;

    private void OnEnable()
    {
        List<GameObject> cannons = new List<GameObject>();
        Transform cannonsParent = transform.parent.Find("Cannons");
        for (int i = 0; i < cannonsParent.childCount; i++)
            cannons.Add(cannonsParent.GetChild(i).gameObject);

        for (int i = 0; i < cannons.Count; i++)
        {
            if (cannons[i].transform.position.x < 0)
                cannons[i].GetComponent<Cannon>().SetTarget(_mainLeftTarget);
            else
                cannons[i].GetComponent<Cannon>().SetTarget(_mainRightTarget);//Надо перенести в методы SetLeftCannons и SetRightCannons
        }
    }
}
