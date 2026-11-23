
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class personagem : MonoBehaviour
{
    private GameOver telagameOver;

    public float totalPontos;
    private int qtdGolpesOponente;
    public float health;

    private UdpClient udpClient;
    private Animator personagemAnimator;
    private SpriteRenderer personagemSpriteRenderer;

    private Text uiTextPontosEsquerda;
    private Text uiTextPontosDireita;

    private int qtdPontosEsquerda;
    private int qtdPontosDireita;

    private bool verificarpunhoesquerdo;
    private bool verificarpunhodireito;

    private float tempocor;

    public GameObject punhoL;
    public GameObject punhoR;

    public float deslocamentoY = 0.5f;
    public float duracaoMovimento = 0.1f;

    private bool podeMoverEsquerdo = true;
    private bool podeMoverDireito = true;

    public float cooldownTempo = 0.5f;

    private bool estaBloqueado;
    private bool estaDefendendo;

    private Color corOriginal;

    void Start()
    {
        estaBloqueado = false;
        estaDefendendo = false;

        totalPontos = 0;
        qtdGolpesOponente = 0;
        health = 100f;

        telagameOver = GameObject.FindGameObjectWithTag("GameOver")
            ?.GetComponent<GameOver>();

        InicializarUdpClient();

        uiTextPontosEsquerda = GameObject.FindGameObjectWithTag("esquerda")
            ?.GetComponent<Text>();

        uiTextPontosDireita = GameObject.FindGameObjectWithTag("direita")
            ?.GetComponent<Text>();

        tempocor = 0.3f;

        verificarpunhoesquerdo = false;
        verificarpunhodireito = false;

        personagemSpriteRenderer = GetComponent<SpriteRenderer>();
        personagemAnimator = GetComponent<Animator>();

        if (personagemSpriteRenderer != null)
            corOriginal = personagemSpriteRenderer.color;

        if (punhoL != null)
            punhoL.SetActive(false);

        if (punhoR != null)
            punhoR.SetActive(false);
    }

    void Update()
    {
        verificarVidaPersonagem();

        SalvarArquivoFloat("totalPontos.txt", totalPontos);

        verificarPunhoEsquerdo();
        verificarPunhoDireito();
    }

    void OnDestroy()
    {
        FecharUdpClient();
    }

    // =========================
    // UDP
    // =========================

    private void InicializarUdpClient()
    {
        FecharUdpClient();

        try
        {
            udpClient = new UdpClient(12345);
            udpClient.BeginReceive(OnReceive, null);
        }
        catch (SocketException ex)
        {
            Debug.LogError("Erro ao iniciar UDP: " + ex.Message);
        }
    }

    private void FecharUdpClient()
    {
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
    }

    void OnReceive(IAsyncResult ar)
    {
        if (udpClient == null)
            return;

        try
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 12345);

            byte[] receivedBytes = udpClient.EndReceive(ar, ref endPoint);

            string message = Encoding.ASCII.GetString(receivedBytes);

            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                ProcessarPunhos(message);
            });

            if (udpClient != null)
                udpClient.BeginReceive(OnReceive, null);
        }
        catch (ObjectDisposedException)
        {
            // UDP foi encerrado
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Erro ao receber UDP: " + ex.Message);
        }
    }

    // =========================
    // PROCESSAR COMANDOS
    // =========================

    void ProcessarPunhos(string message)
    {
        Debug.Log("UDP RECEBIDO: " + message);

        if (message == "Defesa")
        {
            Debug.Log("ENTROU NA DEFESA");

            if (!estaDefendendo)
            {
                estaDefendendo = true;
                estaBloqueado = true;
                ExecutarDefesa();
            }

            return;
        }

        if (message == "NEUTRO")
        {
            Debug.Log("SAINDO DA DEFESA");

            PararDefesa();
            return;
        }

        if (estaDefendendo)
        {
            Debug.Log("DEFESA INTERROMPIDA POR OUTRO COMANDO");
            PararDefesa();
        }

        if (estaBloqueado)
            return;

        if (message == "Q_PRESSED" && podeMoverDireito)
        {
            ExecutarAtaqueDireito();
        }
        else if (message == "E_PRESSED" && podeMoverEsquerdo)
        {
            ExecutarAtaqueEsquerdo();
        }
    }

    // =========================
    // DEFESA
    // =========================

    void ExecutarDefesa()
    {
        if (personagemAnimator == null)
            return;

        // Desativa outros estados que possam interferir
        personagemAnimator.SetBool("AtacandoL", false);
        personagemAnimator.SetBool("AtacandoR", false);
        personagemAnimator.SetBool("estaParado", false);

        // Ativa a defesa
        personagemAnimator.SetBool("estaBloqueando", true);

        Debug.Log("DEFESA ATIVADA");
    }


    void PararDefesa()
    {
        estaDefendendo = false;
        estaBloqueado = false;

        if (personagemAnimator == null)
            return;

        // Desativa todos os parâmetros da defesa e ataques
        personagemAnimator.SetBool("estaBloqueando", false);
        personagemAnimator.SetBool("AtacandoL", false);
        personagemAnimator.SetBool("AtacandoR", false);

        // Retorna ao estado normal
        personagemAnimator.SetBool("estaParado", true);

        Debug.Log("DEFESA DESATIVADA");
        Debug.Log("estaBloqueando: " +
            personagemAnimator.GetBool("estaBloqueando"));

        Debug.Log("estaParado: " +
            personagemAnimator.GetBool("estaParado"));
    }

    // =========================
    // ATAQUE ESQUERDO
    // =========================

    void ExecutarAtaqueEsquerdo()
    {
        if (estaBloqueado || verificarpunhoesquerdo)
            return;

        verificarpunhoesquerdo = true;
        qtdPontosEsquerda++;

        if (personagemAnimator != null)
        {
            personagemAnimator.SetBool("AtacandoL", true);
            personagemAnimator.SetBool("estaParado", true);
        }

        if (uiTextPontosEsquerda != null)
        {
            uiTextPontosEsquerda.text =
                "Número de hits (esquerda): " + qtdPontosEsquerda;
        }

        if (punhoL != null)
        {
            punhoL.SetActive(true);
            StartCoroutine(MoverPunho(punhoL, true));
        }

        StartCoroutine(CooldownEsquerdo());
    }

    // =========================
    // ATAQUE DIREITO
    // =========================

    void ExecutarAtaqueDireito()
    {
        if (estaBloqueado || verificarpunhodireito)
            return;

        verificarpunhodireito = true;
        qtdPontosDireita++;

        if (personagemAnimator != null)
        {
            personagemAnimator.SetBool("AtacandoR", true);
            personagemAnimator.SetBool("estaParado", true);
        }

        if (uiTextPontosDireita != null)
        {
            uiTextPontosDireita.text =
                "Número de hits (direita): " + qtdPontosDireita;
        }

        if (punhoR != null)
        {
            punhoR.SetActive(true);
            StartCoroutine(MoverPunho(punhoR, false));
        }

        StartCoroutine(CooldownDireito());
    }

    // =========================
    // MOVIMENTO DOS PUNHOS
    // =========================

    IEnumerator MoverPunho(GameObject punho, bool esquerdo)
    {
        if (estaBloqueado || punho == null)
            yield break;

        Vector3 posicaoInicial = punho.transform.position;

        Vector3 posicaoFinal = posicaoInicial +
            new Vector3(0, deslocamentoY, 0);

        float tempo = 0;

        while (tempo < duracaoMovimento)
        {
            punho.transform.position = Vector3.Lerp(
                posicaoInicial,
                posicaoFinal,
            tempo / duracaoMovimento
            );

            tempo += Time.deltaTime;
            yield return null;
        }

        punho.transform.position = posicaoFinal;

        yield return new WaitForSeconds(0.1f);

        tempo = 0;

        while (tempo < duracaoMovimento)
        {
            punho.transform.position = Vector3.Lerp(
                posicaoFinal,
                posicaoInicial,
                tempo / duracaoMovimento
            );

            tempo += Time.deltaTime;
            yield return null;
        }

        punho.transform.position = posicaoInicial;
        punho.SetActive(false);

        if (personagemAnimator != null)
        {
            if (esquerdo)
                personagemAnimator.SetBool("AtacandoL", false);
            else
                personagemAnimator.SetBool("AtacandoR", false);

            personagemAnimator.SetBool("estaParado", true);
        }
    }

    // =========================
    // COOLDOWN
    // =========================

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

    // =========================
    // CONTROLE DOS GOLPES
    // =========================

    void verificarPunhoEsquerdo()
    {
        if (!verificarpunhoesquerdo || punhoL == null)
            return;

        if (tempocor >= 0)
        {
            tempocor -= Time.deltaTime;
        }
        else
        {
            verificarpunhoesquerdo = false;
            tempocor = 0.3f;
        }
    }

    void verificarPunhoDireito()
    {
        if (!verificarpunhodireito || punhoR == null)
            return;

        if (tempocor >= 0)
        {
            tempocor -= Time.deltaTime;
        }
        else
        {
            verificarpunhodireito = false;
            tempocor = 0.3f;
        }
    }

    // =========================
    // VIDA
    // =========================

    void verificarVidaPersonagem()
    {
        if (health <= 0)
        {
            telagameOver?.Exibir();
        }
    }

    public void AddDamage(float dano)
    {
        health -= dano;
    }

    void perderPontos()
    {
        totalPontos = Mathf.Max(0, totalPontos - 5);
    }

    // =========================
    // ARQUIVOS
    // =========================

    public void SalvarArquivoFloat(string nomeArquivo, float valor)
    {
        string caminho = Path.Combine(
            Application.persistentDataPath,
            nomeArquivo
        );

        File.WriteAllText(caminho, valor.ToString());
    }

    // =========================
    // COR DO PERSONAGEM
    // =========================

    IEnumerator TemporariamenteMudarCor(Color novaCor, float duracao)
    {
        if (personagemSpriteRenderer == null)
            yield break;

        personagemSpriteRenderer.color = novaCor;

        yield return new WaitForSeconds(duracao);

        personagemSpriteRenderer.color = corOriginal;
    }

    // =========================
    // COLISÕES
    // =========================

    void OnTriggerEnter2D(Collider2D objetoTriggerTocado)
    {
        string tagTocadaTrigger = objetoTriggerTocado.gameObject.tag;

        if (
            tagTocadaTrigger == "punhoEsquerdoOponente" ||
            tagTocadaTrigger == "punhoDireitoOponente"
        )
        {
            if (!estaBloqueado)
            {
                perderPontos();
                AddDamage(2f);
                qtdGolpesOponente++;

                StartCoroutine(
                    TemporariamenteMudarCor(
                        new Color(0.5f, 0, 0.5f),
                        0.3f
                    )
                );

                SalvarArquivoFloat(
                    "qtdGolpesOponente.txt",
                    qtdGolpesOponente
                );
            }
        }
    }
}