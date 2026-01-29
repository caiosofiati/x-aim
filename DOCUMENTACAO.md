# 📖 Documentação Completa - X-Aim

## Visão Geral

O **X-Aim** é um aplicativo para Windows que exibe uma mira (crosshair) personalizável sobre qualquer aplicativo ou jogo. A mira é uma sobreposição transparente que permanece sempre visível no centro da tela.

---

## 🖥️ Telas do Aplicativo

### 1. Janela de Sobreposição (Overlay Window)

Esta é a janela invisível que contém a mira. Suas características:

- **Transparente**: Não bloqueia cliques do mouse
- **Sempre no topo**: Fica sobreposta a qualquer aplicativo
- **Centralizada**: Posicionada automaticamente no centro da tela
- **Sem bordas**: Apenas a mira é visível

A mira pode incluir:
- **Linhas horizontais** (esquerda e direita)
- **Linhas verticais** (cima e baixo)
- **Círculo** ao redor do centro
- **Ponto central** (dot)

---

### 2. Painel de Controle (Control Panel)

Janela principal com interface moderna em tema escuro, dividida em 3 abas:

---

#### 📐 Aba "Shape" (Formato)

Controla a aparência e dimensões da mira:

**Botões de Forma:**
- **Circle**: Liga/desliga o círculo ao redor da mira
- **Dot**: Liga/desliga o ponto central
- **T-Shape**: Formato T (remove a linha superior)

**Visibilidade das Linhas:**
- **Horizontal Lines**: Toggle para mostrar/ocultar linhas laterais
- **Vertical Lines**: Toggle para mostrar/ocultar linhas de cima/baixo

**Configurações de Tamanho:**
- **Length**: Comprimento das linhas (10-200 pixels)
- **Thickness**: Espessura das linhas (1-20 pixels)
- **Gap**: Espaço entre o centro e as linhas (0-50 pixels)
- **Opacity**: Transparência da mira (0-100%)
- **Dot Size**: Tamanho do ponto central (quando ativado)
- **Circle Radius**: Raio do círculo (quando ativado)

---

#### 🎨 Aba "Color" (Cor)

Controla a cor da mira:

**Seletor de Cor HSV:**
- **Quadrado de Saturação/Brilho**: Clique ou arraste para escolher a intensidade
- **Barra de Matiz (Hue)**: Barra vertical colorida para escolher a cor base
- **Preview**: Quadrado mostrando a cor selecionada

**Sliders HSV:**
- **H (Hue)**: Matiz em graus (0-360°)
- **S (Saturation)**: Saturação em porcentagem (0-100%)
- **V (Value)**: Brilho em porcentagem (0-100%)

**Entrada HEX:**
- Campo para digitar código de cor hexadecimal (ex: #FF8C00)

**Cores Rápidas:**
- Botões com cores pré-definidas para seleção rápida

---

#### 💾 Aba "Profiles" (Perfis)

Gerenciamento de configurações salvas:

**Carregar Perfil:**
- Dropdown com lista de perfis salvos
- Botão "✓ Load" para carregar o perfil selecionado
- Botão "🗑 Delete" para excluir o perfil

**Salvar Novo Perfil:**
- Campo de texto para nome do perfil
- Botão "💾 Save" para salvar as configurações atuais

**Dica:**
Os perfis salvam todas as configurações: formato, tamanho, cor e visibilidade.

---

### 3. Rodapé (Footer)

Presente em todas as abas:

- **Enable Crosshair**: Toggle para ativar/desativar a visibilidade da mira
- **Reset**: Botão para restaurar configurações padrão

---

## 🔧 Como Funciona Internamente

### Arquitetura MVVM

O projeto segue o padrão **Model-View-ViewModel**:

- **Models**: Classes de dados (ex: `CrosshairPreset`)
- **ViewModels**: Lógica de apresentação (ex: `CrosshairViewModel`)
- **Views**: Interface do usuário (XAML)

### Principais Componentes

| Componente | Função |
|------------|--------|
| `OverlayWindow` | Janela transparente com a mira |
| `ControlPanelWindow` | Painel de controle |
| `ColorPickerControl` | Seletor de cores HSV |
| `CrosshairViewModel` | Gerencia estado e propriedades |
| `SettingsService` | Salva/carrega configurações |

### Bindings

Todas as configurações usam data binding bidirecional, ou seja, qualquer alteração na interface atualiza automaticamente a mira em tempo real.

---

## 📌 Dicas de Uso

1. **Para jogos**: Execute o app antes de abrir o jogo
2. **Perfis para jogos diferentes**: Crie perfis específicos para cada jogo
3. **Mira discreta**: Use baixa opacidade e cores sutis
4. **Mira visível**: Use cores vibrantes (laranja, verde) e maior espessura

---

## 🎮 Casos de Uso

- Jogos FPS sem mira nativa
- Jogos com mira difícil de ver
- Prática de mira (aim training)
- Acessibilidade visual

---

*Documentação criada para o projeto X-Aim*


## Visão Geral

O **Crosshair Overlay** é um aplicativo para Windows que exibe uma mira (crosshair) personalizável sobre qualquer aplicativo ou jogo. A mira é uma sobreposição transparente que permanece sempre visível no centro da tela.

---

## 🖥️ Telas do Aplicativo

### 1. Janela de Sobreposição (Overlay Window)

Esta é a janela invisível que contém a mira. Suas características:

- **Transparente**: Não bloqueia cliques do mouse
- **Sempre no topo**: Fica sobreposta a qualquer aplicativo
- **Centralizada**: Posicionada automaticamente no centro da tela
- **Sem bordas**: Apenas a mira é visível

A mira pode incluir:
- **Linhas horizontais** (esquerda e direita)
- **Linhas verticais** (cima e baixo)
- **Círculo** ao redor do centro
- **Ponto central** (dot)

---

### 2. Painel de Controle (Control Panel)

Janela principal com interface moderna em tema escuro, dividida em 3 abas:

---

#### 📐 Aba "Shape" (Formato)

Controla a aparência e dimensões da mira:

**Botões de Forma:**
- **Circle**: Liga/desliga o círculo ao redor da mira
- **Dot**: Liga/desliga o ponto central
- **T-Shape**: Formato T (remove a linha superior)

**Visibilidade das Linhas:**
- **Horizontal Lines**: Toggle para mostrar/ocultar linhas laterais
- **Vertical Lines**: Toggle para mostrar/ocultar linhas de cima/baixo

**Configurações de Tamanho:**
- **Length**: Comprimento das linhas (10-200 pixels)
- **Thickness**: Espessura das linhas (1-20 pixels)
- **Gap**: Espaço entre o centro e as linhas (0-50 pixels)
- **Opacity**: Transparência da mira (0-100%)
- **Dot Size**: Tamanho do ponto central (quando ativado)
- **Circle Radius**: Raio do círculo (quando ativado)

---

#### 🎨 Aba "Color" (Cor)

Controla a cor da mira:

**Seletor de Cor HSV:**
- **Quadrado de Saturação/Brilho**: Clique ou arraste para escolher a intensidade
- **Barra de Matiz (Hue)**: Barra vertical colorida para escolher a cor base
- **Preview**: Quadrado mostrando a cor selecionada

**Sliders HSV:**
- **H (Hue)**: Matiz em graus (0-360°)
- **S (Saturation)**: Saturação em porcentagem (0-100%)
- **V (Value)**: Brilho em porcentagem (0-100%)

**Entrada HEX:**
- Campo para digitar código de cor hexadecimal (ex: #FF8C00)

**Cores Rápidas:**
- Botões com cores pré-definidas para seleção rápida

---

#### 💾 Aba "Profiles" (Perfis)

Gerenciamento de configurações salvas:

**Carregar Perfil:**
- Dropdown com lista de perfis salvos
- Botão "✓ Load" para carregar o perfil selecionado
- Botão "🗑 Delete" para excluir o perfil

**Salvar Novo Perfil:**
- Campo de texto para nome do perfil
- Botão "💾 Save" para salvar as configurações atuais

**Dica:**
Os perfis salvam todas as configurações: formato, tamanho, cor e visibilidade.

---

### 3. Rodapé (Footer)

Presente em todas as abas:

- **Enable Crosshair**: Toggle para ativar/desativar a visibilidade da mira
- **Reset**: Botão para restaurar configurações padrão

---

## 🔧 Como Funciona Internamente

### Arquitetura MVVM

O projeto segue o padrão **Model-View-ViewModel**:

- **Models**: Classes de dados (ex: `CrosshairPreset`)
- **ViewModels**: Lógica de apresentação (ex: `CrosshairViewModel`)
- **Views**: Interface do usuário (XAML)

### Principais Componentes

| Componente | Função |
|------------|--------|
| `OverlayWindow` | Janela transparente com a mira |
| `ControlPanelWindow` | Painel de controle |
| `ColorPickerControl` | Seletor de cores HSV |
| `CrosshairViewModel` | Gerencia estado e propriedades |
| `SettingsService` | Salva/carrega configurações |

### Bindings

Todas as configurações usam data binding bidirecional, ou seja, qualquer alteração na interface atualiza automaticamente a mira em tempo real.

---

## 📌 Dicas de Uso

1. **Para jogos**: Execute o app antes de abrir o jogo
2. **Perfis para jogos diferentes**: Crie perfis específicos para cada jogo
3. **Mira discreta**: Use baixa opacidade e cores sutis
4. **Mira visível**: Use cores vibrantes (laranja, verde) e maior espessura

---

## 🎮 Casos de Uso

- Jogos FPS sem mira nativa
- Jogos com mira difícil de ver
- Prática de mira (aim training)
- Acessibilidade visual

---

*Documentação criada para o projeto Crosshair Overlay*
