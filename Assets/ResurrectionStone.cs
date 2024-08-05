using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionStone : MonoBehaviour
{
    [SerializeField]
    Collider ringCollider;
    bool animationRoutine;

    private void Start()
    {
        animationRoutine = false;
        EquipScript.horcruxHit += HitCheck;
    }

    private void Update()
    {
        if(animationRoutine)
            transform.Rotate(Vector3.up * 60 * Time.deltaTime);
    }

    void HitCheck(Collider other)
    {
        if (other.Equals(ringCollider))
        {
            animationRoutine = true;
            this.transform.SetParent(null);
        }
    }
}
