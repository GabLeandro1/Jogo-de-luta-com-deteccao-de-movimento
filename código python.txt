import cv2
import mediapipe as mp
import psutil
import socket
import subprocess
import customtkinter as ctk
import threading

# Função para salvar o nome
def salvar_nome():
    nome = entry.get()
    with open("F:/Projetos ou coisas do técnico/jogo de box/nome_usuario.txt", "w") as arquivo:
        arquivo.write(f"{nome}\n")
    janela.destroy()

# Verifica se o Unity está em execução
def is_unity_running():
    unity_process_name = "Unity.exe"
    for process in psutil.process_iter(['name']):
        if process.info['name'] == unity_process_name:
            return True
    return False

# Função para obter a cena ativa via socket
def get_active_scene():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    server_address = ('localhost', 12346)
    scene_name = None
    try:
        message = "GET_SCENE"
        sock.sendto(message.encode(), server_address)
        data, _ = sock.recvfrom(4096)
        scene_name = data.decode()
    except Exception as e:
        print("Erro ao obter a cena ativa:", e)
    finally:
        sock.close()
    return scene_name

# Enviar comando para pressionar tecla 'E'
def send_key_E_press():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    server_address = ('localhost', 12345)
    try:
        message = "E_PRESSED"
        sock.sendto(message.encode(), server_address)
    finally:
        sock.close()

# Enviar comando para pressionar tecla 'Q'
def send_key_Q_press():
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    server_address = ('localhost', 12345)
    try:
        message = "Q_PRESSED"
        sock.sendto(message.encode(), server_address)
    finally:
        sock.close()

# Função para processar a captura da câmera e detecção de mãos em um thread separado
def processar_camera():
    # Inicializar MediaPipe e OpenCV
    mp_hands = mp.solutions.hands
    hands = mp_hands.Hands()
    mp_drawing = mp.solutions.drawing_utils
    cap = cv2.VideoCapture(0)

    # Configurações de proximidade
    proximity_threshold = -0.1

    # Loop principal
    while cap.isOpened():
        # Captura e processa a imagem da câmera
        ret, frame = cap.read()
        if not ret:
            break
        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = hands.process(image)

        if results.multi_hand_landmarks:
            for idx, hand_landmarks in enumerate(results.multi_hand_landmarks):
                mp_drawing.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS)

                hand_center_z = hand_landmarks.landmark[-4].z
                wrist_x = hand_landmarks.landmark[0].x

                if wrist_x < 0.5:
                    hand_label_text = "Mão Esquerda"
                    if hand_center_z < proximity_threshold:
                        send_key_Q_press()
                else:
                    hand_label_text = "Mão Direita"
                    if hand_center_z < proximity_threshold:
                        send_key_E_press()

                cv2.putText(frame, hand_label_text, (10, 30 * (idx + 1)), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2, cv2.LINE_AA)

        # Exibe a imagem processada
        cv2.imshow('Hand Detection', frame)

        if cv2.waitKey(10) & 0xFF == ord('q'):
            break

    # Libere recursos
    cap.release()
    cv2.destroyAllWindows()

# Função principal que verifica a cena e exibe a janela de entrada de nome
def verificar_cena():
    ultima_cena = None
    while True:
        if is_unity_running():
            cena_atual = get_active_scene()

            # Verifique se a cena mudou e se é a "tela de titulo"
            if cena_atual != ultima_cena:
                ultima_cena = cena_atual  # Atualize a última cena

                if cena_atual == "tela de titulo":
                    # Exiba a janela de entrada de nome no thread principal
                    janela_nome()

# Função para exibir a janela de entrada de nome
def janela_nome():
    ctk.set_appearance_mode("System")
    ctk.set_default_color_theme("blue")

    global janela, entry
    janela = ctk.CTk()
    janela.title("Nome")
    janela.geometry("300x200")

    label_instrucoes = ctk.CTkLabel(janela, text="Digite seu nome completo:")
    label_instrucoes.pack(pady=10)

    entry = ctk.CTkEntry(janela, width=200)
    entry.pack(pady=10)

    botao_salvar = ctk.CTkButton(janela, text="Salvar", command=salvar_nome)
    botao_salvar.pack(pady=10)

    label_resultado = ctk.CTkLabel(janela, text="")
    label_resultado.pack(pady=10)

    janela.mainloop()

# Iniciar Unity e abrir o projeto
unity_path = r"C:\Program Files\Unity\Hub\Editor\2017.4.40f1\Editor\Unity.exe"
project_path = r"F:\Projetos ou coisas do técnico\jogo de box"
subprocess.Popen([unity_path, "-projectPath", project_path, "-executeMethod", "PlayModeInitializer.PlayGame"])
print('Iniciando o Unity...')

# Iniciar a captura da câmera em um thread separado
camera_thread = threading.Thread(target=processar_camera)
camera_thread.start()

# Iniciar a verificação da cena no thread principal (onde roda o tkinter)
verificar_cena()
