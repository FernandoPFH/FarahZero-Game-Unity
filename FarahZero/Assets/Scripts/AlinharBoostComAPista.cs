using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AlinharBoostComAPista : MonoBehaviour
{
    public bool Alinhar = false;
    public LayerMask LayerMaskParaAlinhar;

    private void OnDrawGizmos()
    {
        if (Alinhar)
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, 30f,LayerMaskParaAlinhar))
            {
                transform.up = hitInfo.normal;

                transform.RotateAround(transform.position, transform.up, 180f);

                transform.position = hitInfo.point;
            }
    }
}
