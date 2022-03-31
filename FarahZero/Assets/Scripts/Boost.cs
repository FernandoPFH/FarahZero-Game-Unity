using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] private float _forcaBoost = 100f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            EfeitosDeCamera.Instancia.EfeitoDeVelocidade();
            rigidbody.AddForce(transform.forward * _forcaBoost);
        }
    }
}
