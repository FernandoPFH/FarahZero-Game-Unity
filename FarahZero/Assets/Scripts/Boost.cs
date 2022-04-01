using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            EfeitosDeCamera.Instancia.EfeitoDeVelocidade();
            rigidbody.AddForce(transform.forward * Constantes.BoostNivel1);
        }
    }
}
