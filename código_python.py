
import cv2
import mediapipe as mp
import psutil
import socket
import subprocess
import threading
import math
import time

# =========================
# CONFIGURAÇÕES
# =========================

UNITY_PATH = r"C:\Program Files\Unity\Editor\Unity.exe"
PROJECT_PATH = r"C:\Users\gabri\Downloads\jogo de box (definitivo)\jogo de box"

UDP_ADDRESS = ("localhost", 12345)

PROXIMITY_THRESHOLD = -0.1
DEFENSE_DISTANCE = 0.2
DEFENSE_HEIGHT = 0.5

PUNCH_COOLDOWN = 0.5


# =========================
# COMUNICAÇÃO UDP
# =========================

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)


def enviar_comando(comando):
    try:
        sock.sendto(comando.encode(), UDP_ADDRESS)
    except Exception as e:
        print(f"Erro ao enviar comando: {e}")


def send_key_E_press():
    enviar_comando("E_PRESSED")


def send_key_Q_press():
    enviar_comando("Q_PRESSED")


def send_defesa():
    enviar_comando("Defesa")


def send_neutro():
    enviar_comando("NEUTRO")


# =========================
# VERIFICAR UNITY
# =========================

def is_unity_running():
    for process in psutil.process_iter(['name']):
        try:
            if process.info['name'] == "Unity.exe":
                return True
        except (psutil.NoSuchProcess, psutil.AccessDenied):
            pass
    return False


# =========================
# DISTÂNCIA
# =========================

def calcular_distancia(p1, p2):
    return math.sqrt(
        (p1[0] - p2[0]) ** 2 +
        (p1[1] - p2[1]) ** 2
    )


# =========================
# DEFESA
# =========================

def is_defending(hand_positions):
    if len(hand_positions) != 2:
        return False

    wrist_left = hand_positions[0]
    wrist_right = hand_positions[1]

    distancia = calcular_distancia(wrist_left, wrist_right)

    return (
        distancia < DEFENSE_DISTANCE
        and wrist_left[1] < DEFENSE_HEIGHT
        and wrist_right[1] < DEFENSE_HEIGHT
    )


# =========================
# CÂMERA / MEDIAPIPE
# =========================

def processar_camera():

    mp_hands = mp.solutions.hands
    mp_drawing = mp.solutions.drawing_utils

    hands = mp_hands.Hands(
        max_num_hands=2,
        min_detection_confidence=0.6,
        min_tracking_confidence=0.6
    )

    cap = cv2.VideoCapture(0)

    defesa_ativa = False

    soco_esquerdo_ativo = False
    soco_direito_ativo = False

    ultimo_soco_esquerdo = 0
    ultimo_soco_direito = 0

    while cap.isOpened():

        ret, frame = cap.read()

        if not ret:
            break

        frame = cv2.flip(frame, 1)

        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = hands.process(image)

        hand_positions = []

        esquerda_z = None
        direita_z = None

        if results.multi_hand_landmarks:

            for hand_landmarks in results.multi_hand_landmarks:

                mp_drawing.draw_landmarks(
                    frame,
                    hand_landmarks,
                    mp_hands.HAND_CONNECTIONS
                )

                wrist = hand_landmarks.landmark[0]

                wrist_x = wrist.x
                wrist_y = wrist.y

                hand_center_z = hand_landmarks.landmark[-4].z

                hand_positions.append((wrist_x, wrist_y))

                # Identificação pela posição horizontal da mão
                if wrist_x < 0.5:

                    esquerda_z = hand_center_z

                else:

                    direita_z = hand_center_z

        # =========================
        # DETECTAR DEFESA
        # =========================

        defending = is_defending(hand_positions)

        if defending:

            # Só envia quando a defesa começa
            if not defesa_ativa:
                send_defesa()

            defesa_ativa = True

            # Impede golpes durante a defesa
            soco_esquerdo_ativo = False
            soco_direito_ativo = False

            cv2.putText(
                frame,
                "DEFESA ATIVA",
                (20, 40),
                cv2.FONT_HERSHEY_SIMPLEX,
                1,
                (0, 255, 0),
                2
            )

        else:

            # Defesa acabou
            if defesa_ativa:
                send_neutro()

            defesa_ativa = False

            cv2.putText(
                frame,
                "ESTADO NORMAL",
                (20, 40),
                cv2.FONT_HERSHEY_SIMPLEX,
                1,
                (255, 255, 255),
                2
            )

            tempo_atual = time.time()

            # =========================
            # SOCO ESQUERDO
            # =========================

            if esquerda_z is not None:

                if esquerda_z < PROXIMITY_THRESHOLD:

                    if (
                        not soco_esquerdo_ativo
                        and tempo_atual - ultimo_soco_esquerdo >= PUNCH_COOLDOWN
                    ):

                        send_key_Q_press()

                        soco_esquerdo_ativo = True
                        ultimo_soco_esquerdo = tempo_atual

                else:
                    soco_esquerdo_ativo = False

            else:
                soco_esquerdo_ativo = False

            # =========================
            # SOCO DIREITO
            # =========================

            if direita_z is not None:

                if direita_z < PROXIMITY_THRESHOLD:

                    if (
                        not soco_direito_ativo
                        and tempo_atual - ultimo_soco_direito >= PUNCH_COOLDOWN
                    ):

                        send_key_E_press()

                        soco_direito_ativo = True
                        ultimo_soco_direito = tempo_atual

                else:
                    soco_direito_ativo = False

            else:
                soco_direito_ativo = False

        # =========================
        # EXIBIR CÂMERA
        # =========================

        cv2.imshow("Controle do jogo de boxe", frame)

        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    # =========================
    # ENCERRAR
    # =========================

    if defesa_ativa:
        send_neutro()

    cap.release()
    hands.close()
    cv2.destroyAllWindows()


# =========================
# INICIAR UNITY
# =========================

if not is_unity_running():

    subprocess.Popen([
        UNITY_PATH,
        "-projectPath", PROJECT_PATH,
        "-executeMethod", "PlayModeInitializer.PlayGame"
    ])

    print("Iniciando o Unity...")

else:

    print("Unity já está em execução.")


# =========================
# INICIAR CÂMERA
# =========================

camera_thread = threading.Thread(target=processar_camera)
camera_thread.start()

camera_thread.join()

sock.close()
