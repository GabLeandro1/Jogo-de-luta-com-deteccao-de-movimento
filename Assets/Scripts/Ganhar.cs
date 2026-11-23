using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ganhar : MonoBehaviour
{
    public Text TextoPontuacao;

    private personagem p;
    private bool jaExibiu = false;

    private void Start()
    {
        // Encontrar o jogador pela tag
        GameObject jogador = GameObject.FindGameObjectWithTag("DavRibs");

        if (jogador != null)
        {
            p = jogador.GetComponent<personagem>();

            if (p == null)
                p = jogador.GetComponentInParent<personagem>();

            if (p == null)
                p = jogador.GetComponentInChildren<personagem>(true);
        }

        Debug.Log("Jogador encontrado: " + (jogador != null));
        Debug.Log("Script personagem encontrado: " + (p != null));

        // Esconder a tela de vitória ao iniciar
        gameObject.SetActive(false);
    }

    // Exibir tela de vitória
    public void Exibir()
    {
        // Impedir chamadas repetidas
        if (jaExibiu)
            return;

        jaExibiu = true;

        Debug.Log("Exibindo tela de vitória.");

        // Ativar painel
        gameObject.SetActive(true);

        // Atualizar pontuação
        AtualizarPontuacao();

        // Pausar o jogo
        Time.timeScale = 0f;
    }

    // Esconder tela de vitória
    public void Esconder()
    {
        jaExibiu = false;

        gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    // Atualizar pontuação
    private void AtualizarPontuacao()
    {
        if (TextoPontuacao == null)
        {
            Debug.LogError("TextoPontuacao não foi atribuído no Inspector!");
            return;
        }

        // Procurar jogador novamente, caso não tenha encontrado no Start
        if (p == null)
        {
            GameObject jogador = GameObject.FindGameObjectWithTag("DavRibs");

            if (jogador != null)
                p = jogador.GetComponent<personagem>();
        }

        if (p != null)
        {
            TextoPontuacao.text = "Pontuação total = " + p.totalPontos;

            Debug.Log("Pontuação atualizada: " + p.totalPontos);
        }
        else
        {
            TextoPontuacao.text = "Vitória!";

            Debug.LogWarning("Personagem não encontrado. Exibindo mensagem padrão.");
        }
    }

    // Botão tentar novamente
    public void TentarNovamente()
    {
        Time.timeScale = 1f;

        if (SocketManager.Instance != null)
        {
            SocketManager.Instance.ReiniciarSocket();
        }

        SceneManager.LoadScene("tela de titulo");
    }
}