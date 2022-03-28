using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCarro : MonoBehaviour
{
    public bool Habilitado = false;
    
    [SerializeField]
    private float _forcaDeMovimento;
    [SerializeField]
    private float _forcadeDeRotacao;

    [SerializeField]
    private float _valorDeRotacaoDoVisual;

    [SerializeField]
    private LayerMask _layerMaskChao;
    
    private Rigidbody _rigidbody;
    private Transform _visual;
    private Animator _animator;
    
    private float _moverParaFrente;
    private float _girar;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _visual = transform.Find("Visual");
        _animator = GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _animator.SetTrigger("Ligar");
    }

    // Update is called once per frame
    void Update()
    {
        if (Habilitado)
        {
            // Pega Input Do Teclado
            _moverParaFrente = Input.GetAxis("Vertical");
            _girar = Input.GetAxis("Horizontal");
        
            // Rotaciona O Visual Da Nave
            _visual.localRotation = Quaternion.Euler(new Vector3(_visual.localRotation.eulerAngles.x,_visual.localRotation.eulerAngles.y,-_girar * _valorDeRotacaoDoVisual));
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
        
        // Rotaciona A Nave
        _rigidbody.AddTorque(Vector3.up * _girar * _forcadeDeRotacao * (_moverParaFrente < 0f?-1f:1f));
        
        // Elimina Velocidade Lateral
        Vector3 velocidadeLocal = transform.InverseTransformDirection(_rigidbody.velocity);
        velocidadeLocal.x = 0;
        _rigidbody.velocity = transform.TransformDirection(velocidadeLocal);
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