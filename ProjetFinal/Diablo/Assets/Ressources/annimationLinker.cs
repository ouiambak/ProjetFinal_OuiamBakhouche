using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class annimationLinker : MonoBehaviour
{
    public void ResetAttack()
    {
        GetComponentInParent<PlayerController>().ResetAttack();
    }
}
