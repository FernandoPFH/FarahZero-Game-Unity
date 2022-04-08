using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public class PreservarEntreCenas : MonoBehaviour
{
    [HideInInspector] public string objectID;
    private string _nomeCena;

    void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    private void Start()
    {
        _nomeCena = SceneManager.GetActiveScene().name;

        foreach (var objeto in FindObjectsOfType<PreservarEntreCenas>())
            if (objeto != this)
                if (objeto.objectID == objectID)
                    Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!SceneManager.GetActiveScene().name.Contains(_nomeCena))
            Destroy(gameObject);
    }
}
