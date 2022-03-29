using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncoesUI : MonoBehaviour
{
    static public FuncoesUI Intancia;

    private Animator _animator;

    void Awake()
    {
        Intancia = this;

        _animator = GetComponent<Animator>();
    }

    public void HabilitarCorredores()
    {
        SistemaDaCorrida.Instancia.HabilitarCorredores();
    }

    public void DesabilitarCorredores()
    {
        SistemaDaCorrida.Instancia.DesabilitarCorredores();
    }

    public void HabilitarTelaFinalDaCorrida()
    {
        _animator.SetTrigger("AcabarACorrida");
    }
}
