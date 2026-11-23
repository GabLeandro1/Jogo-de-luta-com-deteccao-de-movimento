# 🎮 Jogo 2D de Luta com Detecção de Movimento

## 📌 Sobre o projeto

Este projeto consiste no desenvolvimento de um jogo de luta em 2D utilizando **Unity**, integrado a um sistema de detecção de movimentos desenvolvido em **Python**.

O jogador interage com o jogo por meio de movimentos físicos capturados pela câmera. O sistema desenvolvido em Python processa esses movimentos, permitindo que sejam utilizados como comandos para controlar o personagem dentro do ambiente virtual.

O projeto combina desenvolvimento de jogos, programação e visão computacional, explorando uma forma alternativa e interativa de jogar, sem depender exclusivamente dos controles tradicionais.

---

## 🎯 Objetivo

Desenvolver um jogo de luta 2D com controle baseado em movimentos físicos, integrando Unity e Python para proporcionar uma experiência de jogo interativa e dinâmica.

---

## 🛠️ Tecnologias utilizadas

- **Unity** — Desenvolvimento do ambiente e das mecânicas do jogo.
- **C#** — Programação dos scripts, movimentação e lógica do jogo.
- **Python** — Captura e detecção de movimentos.
- **Git e GitHub** — Versionamento e gerenciamento do projeto.

---

## 📷 Demonstração

### 🎮 Tela de início

<img width="903" height="411" alt="Tela de início" src="https://github.com/user-attachments/assets/35826827-bcf8-4e8f-9d58-0aa3f082e819" />

### 🕹️ Gameplay

<img width="901" height="412" alt="Gameplay" src="https://github.com/user-attachments/assets/51629564-65b1-413b-8612-7d3caf1016a7" />

### 💀 Tela de derrota

<img width="900" height="410" alt="Tela de derrota" src="https://github.com/user-attachments/assets/44dd45f9-bda3-4103-9816-89da27137135" />

### 🏆 Tela de vitória

<img width="903" height="414" alt="Tela de vitória" src="https://github.com/user-attachments/assets/63534d47-6ce5-42b8-bfc6-07962cc15c10" />

---

## 📂 Arquitetura do projeto

```text
meu-projeto/
│
├── Assets/
│   ├── Animations/          # Animações
│   ├── Animators/           # Controladores de animação
│   ├── Scenes/              # Cenas do jogo
│   ├── Scripts/             # Scripts C# e lógica
│   └── Sprites/             # Imagens e sprites 2D
│
├── Packages/                 # Pacotes e dependências
├── ProjectSettings/          # Configurações da Unity
├── UnityPackageManager/      # Gerenciamento de pacotes
│
├── codigo_python.py         # Sistema de detecção de movimentos
├── .gitattributes           # Configurações de atributos do Git
├── .gitignore               # Arquivos ignorados pelo Git
└── README.md                # Documentação do projeto
```

> A estrutura apresentada representa a organização principal do repositório.

---

## ⚙️ Funcionamento

O funcionamento do projeto ocorre por meio da integração entre a câmera, Python e Unity:

1. A câmera captura os movimentos físicos do jogador.
2. O Python processa as informações recebidas da câmera.
3. O sistema identifica os movimentos realizados.
4. Os movimentos detectados são convertidos em comandos.
5. Os comandos são utilizados para controlar o personagem.
6. A Unity executa as ações correspondentes dentro do jogo.

---

## 🎮 Funcionalidades

- Jogo de luta em ambiente 2D.
- Controle do personagem por detecção de movimentos.
- Integração entre Unity e Python.
- Captura de movimentos utilizando câmera.
- Mecânicas de combate.
- Sistema de pontuação.
- Tela de vitória e derrota.
- Interface inicial para iniciar o jogo.

---

## 🚀 Como executar o projeto

### ⚠️ Configuração necessária

Antes de executar o projeto, é necessário atualizar os **caminhos absolutos** presentes nos scripts ou arquivos de configuração que utilizam diretórios específicos do computador do desenvolvedor.

Substitua os caminhos existentes pelos caminhos correspondentes à localização do projeto no seu computador.

Caso essa alteração não seja realizada, funcionalidades como carregamento de cenas, inicialização de scripts ou comunicação entre os componentes poderão apresentar erros.

### 🎮 Executando pela Unity

1. Clone o repositório:

```bash
git clone https://github.com/GabLeandro1/NOME-DO-REPOSITORIO.git
```

2. Abra o Unity Hub.
3. Clique em **Add Project from Disk**.
4. Selecione a pasta do projeto clonado.
5. Abra o projeto utilizando uma versão compatível da Unity.
6. Verifique se as cenas necessárias estão incluídas no Build Profile.
7. Abra a cena inicial.
8. Execute o projeto pelo botão **Play**.

### 🐍 Executando o Python

1. Certifique-se de que o Python está instalado.
2. Abra um terminal na pasta do projeto.
3. Execute o script de detecção de movimentos:

```bash
python codigo_python.py
```

4. Permita o acesso à câmera, caso solicitado.
5. Mantenha o script Python executando enquanto utiliza o jogo, caso a integração dependa dele.

> Caso o projeto utilize bibliotecas externas, elas deverão ser instaladas previamente.

---

## 📚 Aprendizados e desenvolvimento

O desenvolvimento deste projeto envolve o estudo e a aplicação de diferentes conceitos, como:

- Desenvolvimento de jogos 2D.
- Programação em C#.
- Programação em Python.
- Visão computacional e detecção de movimentos.
- Integração entre diferentes linguagens e tecnologias.
- Desenvolvimento de interfaces e mecânicas de jogo.
- Lógica de programação.
- Versionamento de código com Git.
- Organização e documentação de projetos.

---

<p align="center">
  Desenvolvido como projeto da feira técnica de 2024, ocorrida no Colégios Univap unidade: centro
</p>
