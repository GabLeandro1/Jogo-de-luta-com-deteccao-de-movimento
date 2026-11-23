using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class PlayModeInitializer
{
    [MenuItem("Tools/Play Game")]
    public static void PlayGame()
    {
        // Carregar a cena "tela de título"
        EditorSceneManager.OpenScene("C:/Users/gabri/Downloads/jogo de box (definitivo)/jogo de box/Assets/tela de titulo.unity");

        // Verificar se o SocketManager já existe na cena
        if (GameObject.FindObjectOfType<SocketManager>() == null)
        {
            GameObject socketManagerObject = new GameObject("SocketManager");
            socketManagerObject.AddComponent<SocketManager>();
            Debug.Log("SocketManager criado.");
        }
        else
        {
            Debug.Log("SocketManager já existe.");
        }

        EditorApplication.isPlaying = true;
    }
}
