using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointGerenciador : MonoBehaviour
{
    private TextMeshProUGUI _contadorDeVolta;
    private TextMeshProUGUI _contadorDeTempo;
    
    private int _idCheckPointAtual = 0;
    private int _voltaAtual = 1;

    private bool estaHabilitado = false;

    private float _tempoDeComeco;
    private List<float> _tempoDeCadaVolta = new List<float>();

    private void Update()
    {
        if (!estaHabilitado && SistemaDaCorrida.Instancia.corredoresEstaoHabilitados)
        {
            _tempoDeComeco = Time.time;
            estaHabilitado = true;
        }
        
        // Atualiza O UI
        _contadorDeVolta.text = _voltaAtual.ToString();
        _contadorDeTempo.text = CalcularSegundosParaHorasEMinutos(Time.time - _tempoDeComeco);
    }

    public void Init(TextMeshProUGUI contadorDeVolta,TextMeshProUGUI contadorDeTempo)
    {
        _contadorDeVolta = contadorDeVolta;
        _contadorDeTempo = contadorDeTempo;
    }

    private void Start()
    {
        _tempoDeComeco = Time.time;
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
                    
                    _tempoDeCadaVolta.Add(Time.time - _tempoDeComeco);

                    var tempoTotal = 0f;
                    foreach (float tempoDeCadaVolta in _tempoDeCadaVolta)
                        tempoTotal += tempoDeCadaVolta;
                    SistemaDaCorrida.Instancia.CorridaAcabou(this, _voltaAtual,tempoTotal);
                }
                // Se Não, Passa O Check Point 
                else
                {
                    _idCheckPointAtual = checkPoint.id;
                }
        }
    }

    String CalcularSegundosParaHorasEMinutos(float tempoAtual)
    {
        float segundos = tempoAtual % 60f;
        float minutos = Mathf.Floor(tempoAtual / 60f);
        float horas = Mathf.Floor(minutos / 60f);
        minutos %= 60f;

        String textoHoras = (Mathf.FloorToInt(horas) < 10
            ? "0" + Mathf.FloorToInt(horas).ToString()
            : Mathf.FloorToInt(horas).ToString());

        String textoMinutos = (Mathf.FloorToInt(minutos) < 10
            ? "0" + Mathf.FloorToInt(minutos).ToString()
            : Mathf.FloorToInt(minutos).ToString());

        String textoSegundos = (Mathf.FloorToInt(segundos) < 10
            ? "0" + Mathf.FloorToInt(segundos).ToString()
            : Mathf.FloorToInt(segundos).ToString());

        return textoHoras + "'" + textoMinutos + "''" + textoSegundos;
    }
}
