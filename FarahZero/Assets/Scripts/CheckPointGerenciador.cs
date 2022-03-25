using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointGerenciador : MonoBehaviour
{
    public TextMeshProUGUI volta;
    public TextMeshProUGUI cp;
    
    private int _idCheckPointAtual = 0;
    private int _voltaAtual = 1;

    private void Update()
    {
        // Atualiza O UI
        volta.text = _voltaAtual.ToString();
        cp.text = _idCheckPointAtual.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checa Se O Colisor É Um Check Point
        if (other.TryGetComponent<CheckPoint>(out CheckPoint checkPoint))
        {
            // Checa Se É O Próximo Check Point
            if (checkPoint.id - 1 == _idCheckPointAtual) 
                // Se For O Último Check Point Da Volta, Passa A Volta
                if (checkPoint.ultimoDaVolta)
                {
                    _idCheckPointAtual = 0;
                    _voltaAtual++;
                }
                // Se Não, Passa O Check Point 
                else
                {
                    _idCheckPointAtual = checkPoint.id;
                }
        }
    }
}
