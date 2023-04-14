using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimExit : StateMachineBehaviour
{
    [SerializeField] private float _waitInSec = 2f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject, stateInfo.length + _waitInSec);
    }
}
