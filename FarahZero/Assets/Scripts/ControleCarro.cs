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
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, 10f))
        {
            if (hitInfo.distance != 1f)
            {
                transform.position = Vector3.Lerp(transform.position,hitInfo.point + hitInfo.normal * 3f, .8f) ;
            }

            Vector3 interpolatedNormal = BarycentricCoordinateInterpolator.GetInterpolatedNormal(hitInfo);

            _rigidbody.MoveRotation(Quaternion.FromToRotation(transform.up, interpolatedNormal) * _rigidbody.rotation);
        }

        
        //if (Physics.Raycast(transform.position,-transform.up,6f))
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


// Esse Código Não É Meu
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