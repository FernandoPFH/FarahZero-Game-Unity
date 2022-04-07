using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EfeitosDeCamera : MonoBehaviour
{
    public static EfeitosDeCamera Instancia;

    private Camera _cameraCache;
    
    private bool _efeitoDeVelocidadeEstaEmExecucao = false;

    void Awake()
    {
        _cameraCache = Camera.main;

        Instancia = this;
    }

    async public void EfeitoDeVelocidade()
    {
        if (_efeitoDeVelocidadeEstaEmExecucao)
            return;
        
        _efeitoDeVelocidadeEstaEmExecucao = true;
        
        float tempoDeReferencia = 0f;

        float fovInicial = _cameraCache.fieldOfView;
        
        float fovDesejado = fovInicial + 20;

        while (tempoDeReferencia <= 1)
        {
            _cameraCache.fieldOfView = Mathf.Lerp(fovInicial, fovDesejado, tempoDeReferencia);
            
            await Task.Delay(10);

            tempoDeReferencia += 0.01f;
        }

        await Task.Delay(500);

        while (tempoDeReferencia >= 0)
        {
            _cameraCache.fieldOfView = Mathf.Lerp(fovInicial, fovDesejado, tempoDeReferencia);
            
            await Task.Delay(10);

            tempoDeReferencia -= 0.01f;
        }

        _efeitoDeVelocidadeEstaEmExecucao = false;
    }
}
