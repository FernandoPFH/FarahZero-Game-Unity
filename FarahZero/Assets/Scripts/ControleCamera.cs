using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ControleCamera : MonoBehaviour
{
    public Transform Jogador;
    public float Smoothness;
    public float TurnSmoothness;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, Jogador.position,Smoothness);
        transform.rotation = Quaternion.Euler(
            Vector3.up * Mathf.LerpAngle(transform.rotation.eulerAngles.y,Jogador.rotation.eulerAngles.y,TurnSmoothness)
        );
    }
}
