using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCarro : MonoBehaviour
{
    public float ForcaDeMovimento;
    public float ForcadeDeRotacao;

    public float ValorDeRotacao;

    public LayerMask LayerMaskChao;
    
    private Rigidbody _rigidbody;
    private Transform _visual;

    private float _moverParaFrente;
    private float _girar;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _visual = transform.Find("Visual");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Pega Input Do Teclado
        _moverParaFrente = Input.GetAxis("Vertical");
        _girar = Input.GetAxis("Horizontal");
        
        _visual.rotation = Quaternion.Euler(new Vector3(_visual.rotation.eulerAngles.x,_visual.rotation.eulerAngles.y,-_girar * ValorDeRotacao));
        //_visual.Rotate(transform.forward,-_girar * ValorDeRotacao * Time.deltaTime);
    }

    void FixedUpdate()
    {
        Movimentacao();
    }

    void Movimentacao()
    {
        // Checar Chão
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, 3f,LayerMaskChao))
        {
            // Rotaciona A Nave Baseado Na Angulação Do Chão
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation,Quaternion.FromToRotation(transform.up,hitInfo.normal) * _rigidbody.rotation,0.8f));

            // Mantem A Nave Há Uma Distancia Continua Do Chão
            if (hitInfo.distance != 2f)
                _rigidbody.MovePosition(hitInfo.point + hitInfo.normal * 2f);
        
            // Acelera A Nave Baseado Na Rotação Da Nave
            _rigidbody.AddForce(transform.forward * _moverParaFrente * ForcaDeMovimento);
        }
        else
        {
            // Adiciona Gravidade Se Não Tiver Chão
            _rigidbody.AddForce(Vector3.down * 30f);
        
            // Acelera A Nave Para Frente
            _rigidbody.AddForce(Vector3.forward * _moverParaFrente * ForcaDeMovimento);
        }
        
        // Rotaciona A Nave
        _rigidbody.AddTorque(Vector3.up * _girar * ForcadeDeRotacao * (_moverParaFrente < 0f?-1f:1f));
        
        // Elimina Velocidade Lateral
        Vector3 velocidadeLocal = transform.InverseTransformDirection(_rigidbody.velocity);
        velocidadeLocal.x = 0;
        _rigidbody.velocity = transform.TransformDirection(velocidadeLocal);
    }
}