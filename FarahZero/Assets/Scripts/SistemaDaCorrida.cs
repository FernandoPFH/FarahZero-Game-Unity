using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SistemaDaCorrida : MonoBehaviour
{
    static public SistemaDaCorrida Instancia;

    [HideInInspector] public bool corredoresEstaoHabilitados = false;
    
    public int NumeroDeCorredores = 4;
    public int NumeroDeVoltas = 3;

    public List<Mesh> naves;

    private bool _eCorrida;

    [SerializeField]
    private TextMeshProUGUI _contadorDeVolta;

    [SerializeField]
    private TextMeshProUGUI _contadorDeTempo;

    public GameObject NaveJogador;

    private BoxCollider _colisor;
    private List<GameObject> _corredores = new List<GameObject>();

    void Awake()
    {
        Instancia = this;
        
        _colisor = GetComponent<BoxCollider>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _eCorrida = PlayerPrefs.GetInt("ModoEscolhido", 0) == 1;

        if (_eCorrida)
        {
            NumeroDeVoltas = 3;
            NumeroDeCorredores = 4;
        }
        else
        {
            NumeroDeVoltas = 1;
            NumeroDeCorredores = 1;
        }
        
        
        // Calcula Posição Inicial Dos Corredores
        var posicaoInicialCorredores = CalcularPosicoesDosCorredores();

        // Instancia O Jogador
        var jogador = Instantiate(NaveJogador, posicaoInicialCorredores[0], transform.rotation);

        // Passa As Informações Do UI
        jogador.GetComponent<CheckPointGerenciador>().Init(_contadorDeVolta,_contadorDeTempo);
        jogador.transform.Find("Visual").GetComponent<MeshFilter>().mesh = naves[PlayerPrefs.GetInt("NaveEscolhida", 0)];
        jogador.transform.Find("Visual").GetComponent<MeshCollider>().sharedMesh = naves[PlayerPrefs.GetInt("NaveEscolhida", 0)];

        // Adiciona O Jogador A Lista De Corredores
        _corredores.Add(jogador);
        
        for (int i = 1; i < posicaoInicialCorredores.Count; i++)
        {
            // TODO Posicionar Inimigos
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Calcula As Posições Inicias Dos Corredores
    List<Vector3> CalcularPosicoesDosCorredores()
    {
        // Pega O Centro
        var centro = transform.position;

        // Pega O Espaço Para Colocar Os Corredores
        var espaco = _colisor.bounds.size.x;

        // Corrige A Posição Do Centro
        centro.y -= _colisor.bounds.size.y / 2;
        centro.x -= espaco / 2;

        // Pega A Distância Entre Os Corredores
        var distanciaEntreCorredores = espaco / (NumeroDeCorredores + 1f);

        // Inicia A Lista Das Posições Inicias Dos Corredores
        var posicoesDosCorredores = new List<Vector3>();
        
        // Roda Entre As Posicções Iguinorando Os Cantos
        for (int i = 1; i <= NumeroDeCorredores; i++)
        {
            var posicaoDoCorredor = centro;
            posicaoDoCorredor.x += distanciaEntreCorredores * i;
            
            posicoesDosCorredores.Add(posicaoDoCorredor);
        }

        // Retorna As Posicões Inicias Dos Corredores
        return posicoesDosCorredores;
    }

    public void HabilitarCorredores()
    {
        corredoresEstaoHabilitados = true;
        
        foreach (GameObject corredor in _corredores)
        {
            corredor.GetComponent<ControleCarro>().Habilitado = true;
        }
    }

    public void DesabilitarCorredores()
    {
        corredoresEstaoHabilitados = false;
        
        foreach (GameObject corredor in _corredores)
        {
            corredor.GetComponent<ControleCarro>().Habilitado = false;
        }
    }

    public void CorridaAcabou(CheckPointGerenciador instanciaDoCorredor,int numeroDeVoltas,float tempoTotalDaCorrida)
    {
        if (instanciaDoCorredor.transform.TryGetComponent<ControleCarro>(out ControleCarro controleCarro))
        {
            if (numeroDeVoltas > NumeroDeVoltas)
            {
                DesabilitarCorredores();

                if (PlayerPrefs.GetFloat("MenorTempo_"+SceneManager.GetActiveScene().name, Mathf.Infinity) > tempoTotalDaCorrida)
                    PlayerPrefs.SetFloat("MenorTempo_" + SceneManager.GetActiveScene().name, tempoTotalDaCorrida);

                FuncoesUI.Intancia.HabilitarTelaFinalDaCorrida(tempoTotalDaCorrida);
            }
        }
    }
}