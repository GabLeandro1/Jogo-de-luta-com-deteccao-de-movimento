
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Oponente : MonoBehaviour
{
    public Slider slider;
    public Ganhar telaWin;

    private oponente op;
    private bool winExibido = false;

    void Start()
    {
        op = FindObjectOfType<oponente>();

        Debug.Log("HealthBar_Oponente iniciado.");
        Debug.Log("Oponente encontrado: " + (op != null));
        Debug.Log("Painel Win atribuído: " + (telaWin != null));
    }

    void Update()
    {
        if (op == null || slider == null)
            return;

        slider.value = op.health;

        if (op.health <= 0 && !winExibido)
        {
            winExibido = true;

            Debug.Log("VIDA DO OPONENTE CHEGOU A ZERO!");

            if (telaWin != null)
                telaWin.Exibir();
            else
                Debug.LogError("A REFERÊNCIA GANHAR ESTÁ VAZIA!");
        }
    }
}
