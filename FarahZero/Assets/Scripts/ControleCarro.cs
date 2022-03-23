using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCarro : MonoBehaviour
{
    public float VelociadadeDeMovimento = 450000f;
    public float VelociadadeDeGiro = 40000f;
    
    
    private Rigidbody _rigidbody;

    private float _moverParaFrente;
    private float _girar;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _moverParaFrente = Input.GetAxis("Vertical");
        _girar = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Movimentacao();
    }

    void Movimentacao()
    {
        // Acerela E Freia A Nave
        _rigidbody.AddRelativeForce(Vector3.forward * _moverParaFrente * VelociadadeDeMovimento);
        
        // Gira A Nave
        _rigidbody.AddTorque(Vector3.up * _girar * VelociadadeDeGiro);

        // Tira O Efeito De Deslizar Quando Vira
        Vector3 velocidadeLocal = transform.InverseTransformDirection(_rigidbody.velocity);
        velocidadeLocal.x = 0;
        _rigidbody.velocity = transform.TransformDirection(velocidadeLocal);
    }
}
