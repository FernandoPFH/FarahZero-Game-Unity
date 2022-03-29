using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDoMenuDeEscolhas : MonoBehaviour
{
    private int _mapaEscolhido = 99;
    private int _naveEscolhida = 99;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void EscolherMapa(int valorDoMapa) {_mapaEscolhido = valorDoMapa;}
    
    public void EscolherNave(int valorDaNave) {_naveEscolhida = valorDaNave;}
}
