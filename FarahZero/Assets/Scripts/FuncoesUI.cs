using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FuncoesUI : MonoBehaviour
{
    static public FuncoesUI Intancia;

    [SerializeField]
    private TextMeshProUGUI _textoDoMelhorTempo;

    [SerializeField]
    private TextMeshProUGUI _textoDoTempoAtual;

    private Animator _animator;

    void Awake()
    {
        Intancia = this;

        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.SetLayerWeight(1 , PlayerPrefs.GetInt("ModoEscolhido", 0)==0?1:0);
    }

    public void HabilitarCorredores()
    {
        SistemaDaCorrida.Instancia.HabilitarCorredores();
    }

    public void DesabilitarCorredores()
    {
        SistemaDaCorrida.Instancia.DesabilitarCorredores();
    }

    public void HabilitarTelaFinalDaCorrida(float tempoAtual)
    {
        _textoDoMelhorTempo.text = CalcularSegundosParaHorasEMinutos(PlayerPrefs.GetFloat("MenorTempo_"+SceneManager.GetActiveScene().name, 0));
        _textoDoTempoAtual.text = CalcularSegundosParaHorasEMinutos(tempoAtual);
        
        _animator.SetTrigger("AcabarACorrida");
    }

    public void Revanche()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VoltarParaTelaDeMenu()
    {
        SceneManager.LoadScene("MenusIniciais");
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
