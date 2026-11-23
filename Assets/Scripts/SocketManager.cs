using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class SocketManager : MonoBehaviour
{
    private static SocketManager instance;
    private Socket socket;
    private const int PORT = 12346;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        OpenSocket(); // Abre o socket ao iniciar
    }

    public static SocketManager Instance
    {
        get { return instance; }
    }

    public void OpenSocket()
    {
        if (socket != null)
        {
            CloseSocket(); // Fecha o socket existente, se houver
        }

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        
        // Configurar para reutilizar o endereço
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        
        socket.Bind(new IPEndPoint(IPAddress.Any, PORT)); // Define a porta que você deseja usar
        Debug.Log("Socket aberto na porta " + PORT);
    }

    public void CloseSocket()
    {
        if (socket != null)
        {
            try
            {
                socket.Close();
                socket.Dispose();
                socket = null;
                Debug.Log("Socket fechado.");
            }
            catch (Exception e)
            {
                Debug.LogError("Erro ao fechar o socket: " + e.Message);
            }
        }
    }

    public void ReiniciarSocket()
    {
        CloseSocket(); // Fecha o socket atual
        OpenSocket();  // Abre um novo socket
    }

    private void OnDestroy()
    {
        CloseSocket(); // Fecha o socket quando o objeto é destruído
    }
}
