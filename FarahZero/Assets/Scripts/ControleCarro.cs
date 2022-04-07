using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControleCarro : MonoBehaviour
{
    [HideInInspector]
    public bool Habilitado = false;
    
    [SerializeField]
    private float _forcaDeMovimento;
    [SerializeField]
    private float _forcadeDeRotacao;
    [SerializeField]
    private float _forcadeDeRotacaoShift;

    [SerializeField]
    private float _valorDeRotacaoDoVisual;
    [SerializeField]
    private float _valorDeRotacaoShiftDoVisual;

    [SerializeField]
    private float _valorDeIncrementoDeFOV;
    [SerializeField]
    private float _valorDeVelocidadeMaxima;

    [SerializeField]
    private LayerMask _layerMaskChao;
    
    private Rigidbody _rigidbody;
    private Transform _visual;
    private Animator _animator;
    private Camera _camera;

    private float _fovIncial;

    private float? _tempoDoInicioDoDrift = null;
    
    private float _moverParaFrente;
    private float _girar;
    private bool _shiftEstaSendoPrecionado;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _visual = transform.Find("Visual");
        _animator = GetComponent<Animator>();
        _camera = transform.Find("Camera").GetComponent<Camera>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _animator.SetTrigger("Ligar");

        _fovIncial = _camera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (Habilitado)
        {
            // Pega Input Do Teclado
            _moverParaFrente = Input.GetAxis("Vertical");
            _girar = Input.GetAxis("Horizontal");
            _shiftEstaSendoPrecionado = Input.GetKey(KeyCode.LeftShift);

            ControleDoVisual();

            ControleCamera();

            ControleDoDrift();
        }
    }

    void FixedUpdate()
    {
        var estaProximoDoChao = ControleDeGravidade();
        
        if (Habilitado)
        {
            Movimentacao(estaProximoDoChao);
        }
    }

    void ControleDoVisual()
    {
        if (_shiftEstaSendoPrecionado)
        {
            // Rotaciona O Visual Da Nave
            _visual.localRotation = Quaternion.Euler(new Vector3(_visual.localRotation.eulerAngles.x, _girar * _valorDeRotacaoShiftDoVisual, _visual.localRotation.eulerAngles.z));
        }
        else
        {
            _visual.localRotation = Quaternion.Euler(Vector3.zero);
            // Rotaciona O Visual Da Nave
            _visual.localRotation = Quaternion.Euler(new Vector3(_visual.localRotation.eulerAngles.x, _visual.localRotation.eulerAngles.y, -_girar * _valorDeRotacaoDoVisual));
        }
    }

    void ControleCamera()
    {
        var fovAtual = Mathf.Lerp(_fovIncial, _fovIncial + _valorDeIncrementoDeFOV,
            _rigidbody.velocity.magnitude / _valorDeVelocidadeMaxima);

        _camera.fieldOfView = fovAtual;
    }

    void ControleDoDrift()
    {
        if (_tempoDoInicioDoDrift == null && _shiftEstaSendoPrecionado)
            _tempoDoInicioDoDrift = Time.time;
        
        if (_tempoDoInicioDoDrift != null)
        {
            float tempoDeDrift = (float) (Time.time - _tempoDoInicioDoDrift!);

            float[] trashHolds = {1, 3};

            if (tempoDeDrift >= trashHolds[1])
            {
                // TODO Efeito Do Drift Nivel 2
                
                if (!_shiftEstaSendoPrecionado)
                {
                    EfeitosDeCamera.Instancia.EfeitoDeVelocidade();
                    _rigidbody.AddForce(transform.forward * Constantes.BoostNivel2);

                    _tempoDoInicioDoDrift = null;
                }
            }
            else if (tempoDeDrift >= trashHolds[0])
            {
                // TODO Efeito Do Drift Nivel 1
                
                if (!_shiftEstaSendoPrecionado)
                {
                    EfeitosDeCamera.Instancia.EfeitoDeVelocidade();
                    _rigidbody.AddForce(transform.forward * Constantes.BoostNivel1);

                    _tempoDoInicioDoDrift = null;
                }
            }
        }
            
    }

    // Controla A Gravidade E Checa Se Está Próximo Do Chão
    bool ControleDeGravidade()
    {
        // Checar Chão
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, 3f, _layerMaskChao))
        {
            Vector3 interpolatedNormal = BarycentricCoordinateInterpolator.GetInterpolatedNormal(hitInfo);
 
            // Rotaciona A Nave Baseado Na Angulação Do Chão
            _rigidbody.MoveRotation(Quaternion.FromToRotation(transform.up, interpolatedNormal) * _rigidbody.rotation);

            // Mantem A Nave Há Uma Distancia Continua Do Chão
            if (hitInfo.distance != 2f)
                _rigidbody.MovePosition(hitInfo.point + hitInfo.normal * 2f);

            return true;
        }

        return false;
    }

    void Movimentacao(bool estaProximoDoChao)
    {
        // Checar Chão
        if (estaProximoDoChao)
        {
            // Acelera A Nave Baseado Na Rotação Da Nave
            _rigidbody.AddForce(transform.forward * _moverParaFrente * _forcaDeMovimento);
        }
        else
        {
            // Adiciona Gravidade Se Não Tiver Chão
            _rigidbody.AddForce(Vector3.down * 30f);
        
            // Acelera A Nave Para Frente
            _rigidbody.AddForce(Vector3.forward * _moverParaFrente * _forcaDeMovimento);
        }

        if (_shiftEstaSendoPrecionado)
        {
            // Rotaciona A Nave
            _rigidbody.AddTorque(transform.up * _girar * _forcadeDeRotacaoShift * (_moverParaFrente < 0f?-1f:1f));
        }
        else
        {
            // Rotaciona A Nave
            _rigidbody.AddTorque(transform.up * _girar * _forcadeDeRotacao * (_moverParaFrente < 0f?-1f:1f));
        
            // Elimina Velocidade Lateral
            Vector3 velocidadeLocal = transform.InverseTransformDirection(_rigidbody.velocity);
            velocidadeLocal.x = 0;
            _rigidbody.velocity = transform.TransformDirection(velocidadeLocal);
        }
    }
}

public static class BarycentricCoordinateInterpolator
{
    public static Vector3 GetInterpolatedNormal(RaycastHit hit)
    {
        MeshCollider meshCollider = hit.collider as MeshCollider;
 
        if (!meshCollider || !meshCollider.sharedMesh)
        {
            Debug.LogWarning("No MeshCollider attached to to the mesh!", hit.collider);
            return Vector3.up;
        }
 
        Mesh mesh = meshCollider.sharedMesh;
        Vector3 normal = CalculateInterpolatedNormal(mesh, hit);
     
        return normal;
    }
 
    private static Vector3 CalculateInterpolatedNormal(Mesh mesh, RaycastHit hit)
    {
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;
 
        // Extract local space normals of the triangle we hit
        Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];
 
        // interpolate using the barycentric coordinate of the hitpoint
        Vector3 baryCenter = hit.barycentricCoordinate;
 
        // Use barycentric coordinate to interpolate normal
        Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
        // normalize the interpolated normal
        interpolatedNormal = interpolatedNormal.normalized;
 
        // Transform local space normals to world space
        Transform hitTransform = hit.collider.transform;
        interpolatedNormal = hitTransform.TransformDirection(interpolatedNormal);
 
        // Display with Debug.DrawLine
        Debug.DrawRay(hit.point, interpolatedNormal);
 
        return interpolatedNormal;
    }
}