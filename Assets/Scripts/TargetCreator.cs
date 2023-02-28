using System.Collections.Generic;
using UnityEngine;

public class TargetCreator : MonoBehaviour
{
    [SerializeField] private GameObject _newTarget;
    private GameObject _mainLeftTarget;
    private GameObject _mainRightTarget;

    private void OnEnable()
    {
        _mainLeftTarget = transform.parent.Find("MainLeftTarget").gameObject;
        _mainRightTarget = transform.parent.Find("MainRightTarget").gameObject;

        List<GameObject> cannons = new List<GameObject>();
        Transform cannonsParent = transform.parent.Find("Cannons");
        for (int i = 0; i < cannonsParent.childCount; i++)
            cannons.Add(cannonsParent.GetChild(i).gameObject);

        for (int i = 0; i < cannons.Count; i++)
        {
            GameObject target;

            if (cannons[i].transform.position.x < 0)
            {
                target = Instantiate(_newTarget, _mainLeftTarget.transform);
            }
            else
                target = Instantiate(_newTarget, _mainRightTarget.transform);

            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, cannons[i].transform.position.z);

            cannons[i].GetComponent<Cannon>().SetTarget(target);

        }
    }
}
