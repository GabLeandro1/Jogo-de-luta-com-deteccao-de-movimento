
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Personagem : MonoBehaviour
{
    public Slider slider;
    public GameOver telaGameOver;

    personagem p;
    private bool gameOverExibido = false;

    void Start()
    {
        p = FindObjectOfType<personagem>();
    }

    void Update()
    {
        if (p == null || slider == null)
            return;

        slider.value = p.health;

        if (p.health <= 0 && !gameOverExibido)
        {
            gameOverExibido = true;

            if (telaGameOver != null)
            {
                telaGameOver.Exibir();
            }
            else
            {
                Debug.LogError("Painel Game Over não foi atribuído!");
            }
        }
    }
}