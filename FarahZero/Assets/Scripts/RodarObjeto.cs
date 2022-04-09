using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarObjeto : MonoBehaviour
{
    public Transform objetoASeguir;
    
    public float _velocidade = 1f;
    
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(objetoASeguir.position,Vector3.up, _velocidade);
    }
}
