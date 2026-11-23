using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text TextoPontuacao;
    private personagem p; // Referência ao script do personagem

    private void Start()
    {
        // Encontrar o script do personagem
        p = GameObject.FindGameObjectWithTag("DavRibs")?.GetComponent<personagem>();

        // Certificar-se de que a tela de Game Over está desativada no início do jogo
        this.gameObject.SetActive(false);
    }

    // Exibir a tela de Game Over quando o personagem perder toda a vida
    public void Exibir()
    {
        this.gameObject.SetActive(true);
        TextoPontuacao.text = "Pontuação total = " + p.totalPontos.ToString();
        // Pausar o jogo
        Time.timeScale = 0;
    }
    
    public void TentarNovamente()
    {
        // Despausar o jogo
        Time.timeScale = 1;

        // Carregar a cena de título
        SceneManager.LoadScene("tela de titulo");

        // Reiniciar o socket do SocketManager
        SocketManager.Instance.ReiniciarSocket();
    }
}
