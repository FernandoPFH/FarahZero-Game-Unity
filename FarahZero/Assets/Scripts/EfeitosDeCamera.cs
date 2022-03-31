using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EfeitosDeCamera : MonoBehaviour
{
    public static EfeitosDeCamera Instancia;

    void Awake()
    {
        Instancia = this;
    }

    async public void EfeitoDeVelocidade()
    {
        float tempoDeReferencia = 0f;

        Vector3 posicaoInicial = transform.localPosition;
        
        Vector3 posicaoDesejada = transform.localPosition - transform.forward;

        while (tempoDeReferencia <= 1)
        {
            transform.localPosition = Vector3.Lerp(posicaoInicial, posicaoDesejada, tempoDeReferencia);
            
            await Task.Delay(10);

            tempoDeReferencia += 0.01f;
        }

        await Task.Delay(500);

        while (tempoDeReferencia >= 0)
        {
            transform.localPosition = Vector3.Lerp(posicaoInicial, posicaoDesejada, tempoDeReferencia);
            
            await Task.Delay(10);

            tempoDeReferencia -= 0.01f;
        }
    }
}
