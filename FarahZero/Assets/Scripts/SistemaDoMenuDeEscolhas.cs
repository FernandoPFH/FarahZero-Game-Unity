using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SistemaDoMenuDeEscolhas : MonoBehaviour
{
    public List<String> mapas; 
    
    private int _modoEscolhido = 0;
    private int _mapaEscolhido = 0;
    private int _naveEscolhida = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EscolherModo(int valorDoModo) {_modoEscolhido = valorDoModo;}
    
    public void EscolherMapa(int valorDoMapa) {_mapaEscolhido = valorDoMapa;}
    
    public void EscolherNave(int valorDaNave) {_naveEscolhida = valorDaNave;}

    public void EntrarNaCorrida()
    {
        PlayerPrefs.SetInt("ModoEscolhido",_modoEscolhido);
        
        PlayerPrefs.SetInt("NaveEscolhida",_naveEscolhida);
        
        SceneManager.LoadScene(mapas[_mapaEscolhido]);
    }
}