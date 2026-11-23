using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class oponente : MonoBehaviour
{
    Ganhar telaWin;
    int qtdGolpesPersonagem;
    personagem p;
    public float health;
    public GameObject enemyObject;
    Rigidbody2D CorpoRigido;
    BoxCollider2D Colisor;
    private Animator inimigoAnimator;
    SpriteRenderer Renderer;
    private bool verificarpunhoesquerdo;
    private bool verificarpunhodireito;
    private float tempocor;
    private float tempEntreSocos;
    public GameObject punhoL;
    public GameObject punhoR;
    public GameObject colisaopersonagem;
    public float deslocamentoY = 0f;
    public float deslocamentoX = 0f; // Altura do movimento
    public float duracaoMovimento = 0.1f; // Velocidade do movimento
    private bool podeMoverEsquerdo = true;
    private bool podeMoverDireito = true;
    public float cooldownTempo = 0.5f;
    bool podeSocar;
    float tempoSocar;

    // Variável para armazenar a cor original do oponente
    private Color corOriginal;

    void Start()
    {
        tempoSocar = 2f;
        podeSocar = true;
        telaWin = GameObject.FindGameObjectWithTag("Ganhar")?.GetComponent<Ganhar>();
        print(telaWin);
        p = FindObjectOfType<personagem>();
        qtdGolpesPersonagem = 0;
        inimigoAnimator = GetComponent<Animator>();
        tempEntreSocos = 3.5f;
        health = 100f;
        Colisor = GetComponent<BoxCollider2D>();
        CorpoRigido = GetComponent<Rigidbody2D>();
        enemyObject = GameObject.FindWithTag("oponente");
        Renderer = GetComponent<SpriteRenderer>();

        // Armazena a cor original do oponente
        corOriginal = Renderer.color;

        tempocor = 0.3f;
        verificarpunhoesquerdo = false;
        verificarpunhodireito = false;

        // Desabilita os punhos inicialmente
        punhoL = GameObject.FindWithTag("punhoEsquerdoOponente");
        punhoR = GameObject.FindWithTag("punhoDireitoOponente");
        if (punhoL != null) punhoL.SetActive(false);
        if (punhoR != null) punhoR.SetActive(false);
    }

    void Update()
    {
        verificarVidaOponente();
        verificaresquerda();
        verificardireita();
        VerificarSoco();
    }

    void VerificarSoco()
    {
        // Se o personagem pode socar
        if (podeSocar)
        {
            // Verifica se o tempo do soco é maior que 0
            if (tempoSocar > 0)
            {
                // Diminui o tempo do soco
                tempoSocar -= Time.deltaTime;

                // Define as animações de ataque e parada
                inimigoAnimator.SetBool("estaAtacando", false);
                inimigoAnimator.SetBool("estaParado", true);
            }
            else
            {
                // O tempo acabou, começa a socar
                inimigoAnimator.SetBool("estaAtacando", true);
                inimigoAnimator.SetBool("estaParado", false);
                Punhos(); // Chama a função para socar
                podeSocar = false; // Não pode socar até o cooldown
                tempoSocar = 2; // Reseta o tempo do soco
                StartCoroutine(CooldownSocar()); // Inicia cooldown
            }
        }
    }

    IEnumerator CooldownSocar()
    {
        yield return new WaitForSeconds(cooldownTempo);
        podeSocar = true; // Permite socar novamente após o cooldown
    }


    void Punhos()
    { 
        // Habilita os punhos antes de realizar o soco
        if (punhoR != null)
        {
            punhoR.SetActive(true);
            StartCoroutine(MoverQuadrado(punhoR));
            StartCoroutine(CooldownDireito());
        }
        if (punhoL != null)
        {
            punhoL.SetActive(true);
            StartCoroutine(MoverQuadrado(punhoL));
            StartCoroutine(CooldownEsquerdo());
        }
    }

    IEnumerator MoverQuadrado(GameObject quadrado)
    {
        print("Valor de deslocamentoY: " + deslocamentoY);
        Vector3 posicaoInicial = quadrado.transform.position;
        Vector3 posicaoFinal = posicaoInicial + new Vector3(0, deslocamentoY, 0);

        float tempo = 0;
        while (tempo < duracaoMovimento)
        {
            quadrado.transform.position = Vector3.Lerp(posicaoInicial, posicaoFinal, tempo / duracaoMovimento);
            tempo += Time.deltaTime;
            yield return null;
        }

        quadrado.transform.position = posicaoFinal;

        yield return new WaitForSeconds(0.1f);

        tempo = 0;
        while (tempo < duracaoMovimento)
        {
            quadrado.transform.position = Vector3.Lerp(posicaoFinal, posicaoInicial, tempo / duracaoMovimento);
            tempo += Time.deltaTime;
            yield return null;
        }

        quadrado.transform.position = posicaoInicial;

        // Desabilita o punho após o movimento
        quadrado.SetActive(false);
    }

    IEnumerator CooldownEsquerdo()
    {
        podeMoverEsquerdo = false;
        yield return new WaitForSeconds(cooldownTempo);
        podeMoverEsquerdo = true;
    }

    IEnumerator CooldownDireito()
    {
        podeMoverDireito = false;
        yield return new WaitForSeconds(cooldownTempo);
        podeMoverDireito = true;
    }

    void verificaresquerda()
    {
        if (verificarpunhoesquerdo)
        {
            if (punhoL == null)
            {
                punhoL = GameObject.FindWithTag("punhoEsquerdoOponente");
            }

            if (punhoL != null && tempocor >= 0)
            {
                tempocor -= Time.deltaTime;
            }
            else if (punhoL != null)
            {
                verificarpunhoesquerdo = false;
                tempocor = 0.3f;
                inimigoAnimator.SetBool("estaAtacando", true); // Aqui também mantemos a animação de ataque
                inimigoAnimator.SetBool("estaParado", false);
            }
        }
    }

    void verificardireita()
    {
        if (verificarpunhodireito)
        {
            if (punhoR == null)
            {
                punhoR = GameObject.FindWithTag("punhoDireitoOponente");
            }

            if (punhoR != null && tempocor >= 0)
            {
                tempocor -= Time.deltaTime;
            }
            else if (punhoR != null)
            {
                verificarpunhodireito = false;
                tempocor = 0.3f;
                inimigoAnimator.SetBool("estaAtacando", true); // Aqui também mantemos a animação de ataque
                inimigoAnimator.SetBool("estaParado", false);
            }
        }
    }

    void verificarVidaOponente()
    {
        if (health <= 0f)
        {
            telaWin?.Exibir();
        }
    }

    public void AddDamage()
    {
        health -= 1.5f;
    }

    public void somarPontosPersonagem()
    {
        p.totalPontos += 15;
    }

    void SalvarArquivoFloat(string nomeArquivo, float ValorGravar)
    {
        string textoValorGravar = ValorGravar.ToString();
        File.WriteAllText(nomeArquivo, textoValorGravar);
    }

    // Coroutine para mudar temporariamente a cor
    IEnumerator TemporariamenteMudarCor(Color novaCor, float duracao)
    {
        // Altera a cor para a cor roxa (ou qualquer cor especificada)
        Renderer.color = novaCor;

        // Aguarda o tempo especificado (por exemplo, 0.5 segundos)
        yield return new WaitForSeconds(duracao);

        // Restaura a cor original
        Renderer.color = corOriginal;
    }

    void OnTriggerEnter2D(Collider2D objetoTriggerTocado)
    {
        string tagTocadaTrigger = objetoTriggerTocado.gameObject.tag;

        // Verifica se o punho esquerdo ou direito do personagem tocou o oponente
        if (tagTocadaTrigger == "punhoEsquerdoPersonagem" || tagTocadaTrigger == "punhoDireitoPersonagem")
        {
            qtdGolpesPersonagem++;  // Incrementa o número de golpes
            print("Número de golpes recebidos: " + qtdGolpesPersonagem);  // Exibe a quantidade no console
            somarPontosPersonagem();
            AddDamage();

            // Inicia a coroutine para mudar a cor e restaurar
            StartCoroutine(TemporariamenteMudarCor(new Color(0.38f, 0.0f, 0.6f, 0.7f), 0.5f));

            // Salva o número de golpes em um arquivo
            SalvarArquivoFloat("qtdGolpesPersonagem.txt", qtdGolpesPersonagem);
        }
    }

    void OnTriggerExit2D(Collider2D objetoTriggerTirado)
    {
        string tagTocadaTrigger = objetoTriggerTirado.gameObject.tag;

        // Verifica se o punho esquerdo ou direito do personagem tocou o oponente
        if (tagTocadaTrigger == "punhoEsquerdoPersonagem" || tagTocadaTrigger == "punhoDireitoPersonagem")
        {
            // Decrementa a quantidade de golpes quando o punho sai
            qtdGolpesPersonagem--;
        }
    }
}
