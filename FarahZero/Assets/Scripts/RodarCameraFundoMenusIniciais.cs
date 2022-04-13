using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodarCameraFundoMenusIniciais : MonoBehaviour
{
    public float VelocidadeDeRotacao = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position,Vector3.forward, VelocidadeDeRotacao);
    }
}
