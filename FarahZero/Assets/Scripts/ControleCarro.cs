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
        
        //_visual.rotation = Quaternion.Euler(new Vector3(_visual.rotation.eulerAngles.x,_visual.rotation.eulerAngles.y,-_girar * ValorDeRotacao));
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
            Vector3 interpolatedNormal = BarycentricCoordinateInterpolator.GetInterpolatedNormal(hitInfo);
 
            _rigidbody.MoveRotation(Quaternion.FromToRotation(transform.up, interpolatedNormal) * _rigidbody.rotation);
            // Rotaciona A Nave Baseado Na Angulação Do Chão
            //_rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation,Quaternion.FromToRotation(transform.up,hitInfo.normal) * _rigidbody.rotation,0.8f));

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