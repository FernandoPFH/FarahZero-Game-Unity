using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AlinharBoostComAPista : MonoBehaviour
{
    public bool Alinhar = false;
    public LayerMask LayerMaskParaAlinhar;
    
    public float LocalRotationOffSet = 0f;
    private float _ultimoValor_LocalRotationOffSet = 0f;

    private void OnDrawGizmos()
    {
        if (Alinhar)
            CalcularPosicaoERotacaoDoBoost();

        if (LocalRotationOffSet != _ultimoValor_LocalRotationOffSet)
        {
            CalcularPosicaoERotacaoDoBoost();

            _ultimoValor_LocalRotationOffSet = LocalRotationOffSet;
        }
    }

    private void CalcularPosicaoERotacaoDoBoost()
    {
        if (Physics.Raycast(transform.position + transform.up, -transform.up, out RaycastHit hitInfo, 30f,LayerMaskParaAlinhar))
        {
            transform.up = hitInfo.normal;

            transform.RotateAround(transform.position, transform.up, 180f);

            transform.RotateAround(transform.position, transform.up, LocalRotationOffSet);

            transform.position = hitInfo.point;
        }
    }
}
