# 📖 Documentação Completa do Código - CrosshairOverlay

Este documento explica **cada arquivo e função** do projeto de forma simples para quem está começando com C#.

---

## 📁 Estrutura do Projeto

```
CrosshairOverlay/
├── App.xaml.cs                          # Ponto de entrada da aplicação
├── Controls/
│   └── ColorPickerControl.xaml.cs       # Seletor de cores personalizado
├── Converters/
│   └── ColorToBrushConverter.cs         # Conversores para a interface
├── Interop/
│   └── NativeMethods.cs                 # Funções do Windows para transparência
├── Models/
│   ├── CrosshairSettings.cs             # Configurações do crosshair
│   └── Preset.cs                        # Presets salvos
├── Services/
│   └── SettingsService.cs               # Salvar/carregar configurações
├── ViewModels/
│   ├── CrosshairViewModel.cs            # Lógica principal da aplicação
│   └── RelayCommand.cs                  # Comandos para botões
└── Windows/
    ├── ControlPanelWindow.xaml.cs       # Painel de controle
    └── OverlayWindow.xaml.cs            # Janela do crosshair
```

---

## 🚀 App.xaml.cs - Início da Aplicação

Este é o **ponto de entrada** do programa. Quando você abre o .exe, este arquivo é executado primeiro.

### Variáveis Principais
```csharp
private CrosshairViewModel? _viewModel;      // Cérebro da aplicação
private SettingsService? _settingsService;   // Serviço para salvar configurações
private OverlayWindow? _overlayWindow;       // Janela do crosshair (transparente)
private ControlPanelWindow? _controlPanelWindow; // Painel de controle
```

### Funções

#### `Application_Startup()`
**O que faz:** Inicia a aplicação quando você abre o programa.

**Passo a passo:**
1. Cria o serviço de configurações
2. Carrega as configurações salvas (ou usa padrão)
3. Carrega os presets salvos
4. Cria o "cérebro" (ViewModel) compartilhado
5. Cria e mostra a janela do crosshair
6. Cria e mostra o painel de controle

```csharp
private void Application_Startup(object sender, StartupEventArgs e)
{
    // Passo 1: Criar serviço para salvar/carregar
    _settingsService = new SettingsService();
    
    // Passo 2 e 3: Carregar configurações e presets
    var settings = _settingsService.LoadSettings();
    var presets = _settingsService.LoadPresets();
    
    // Passo 4: Criar o "cérebro" da aplicação
    _viewModel = new CrosshairViewModel(settings, presets, _settingsService);
    
    // Passo 5: Criar janela do crosshair e mostrar
    _overlayWindow = new OverlayWindow { DataContext = _viewModel };
    _overlayWindow.Show();
    
    // Passo 6: Criar painel de controle e mostrar
    _controlPanelWindow = new ControlPanelWindow { DataContext = _viewModel };
    _controlPanelWindow.Show();
}
```

#### `Application_Exit()`
**O que faz:** Salva as configurações quando você fecha o programa.

```csharp
private void Application_Exit(object sender, ExitEventArgs e)
{
    // Se existem dados para salvar, salva
    if (_viewModel != null && _settingsService != null)
    {
        _settingsService.SaveSettings(_viewModel.GetCurrentSettings());
    }
}
```

---

## 🎨 ColorPickerControl.xaml.cs - Seletor de Cores HSV

Controle personalizado para escolher cores usando o modelo HSV (Matiz, Saturação, Brilho).

### Variáveis Principais
```csharp
private double _hue = 0;        // Matiz (0-360): Qual cor (vermelho, azul, verde...)
private double _saturation = 1; // Saturação (0-1): Quão "pura" é a cor
private double _value = 1;      // Brilho (0-1): Quão clara é a cor
private bool _isUpdating = false;      // Evita atualização em loop
private bool _isSliderUpdating = false; // Evita atualização em loop dos sliders
```

### Funções Principais

#### `UpdateFromColor(Color color)`
**O que faz:** Quando a cor muda externamente, atualiza todos os controles visuais.

```csharp
private void UpdateFromColor(Color color)
{
    // Converte RGB para HSV
    RgbToHsv(color.R, color.G, color.B, out _hue, out _saturation, out _value);
    
    // Atualiza cada parte visual
    UpdateSatValGradient();  // Atualiza o quadrado de cores
    UpdateIndicators();       // Move os marcadores
    UpdateSliderTracks();     // Atualiza as barras dos sliders
    UpdateSliderValues();     // Atualiza os valores numéricos
    UpdatePreviewColor();     // Atualiza o círculo de preview
}
```

#### `UpdateSatValGradient()`
**O que faz:** Atualiza a cor base do quadrado de saturação/brilho baseado na matiz.

```csharp
private void UpdateSatValGradient()
{
    // Pega a cor pura da matiz atual (saturação e brilho máximos)
    var hueColor = HsvToColor(_hue, 1, 1);
    // Aplica como cor base do quadrado
    HueColorBrush.Color = hueColor;
}
```

#### `UpdateSliderTracks()`
**O que faz:** Atualiza as cores das barras de slider para refletir a cor atual.

#### `UpdateSliderValues()`
**O que faz:** Atualiza os valores numéricos dos sliders H/S/V.

#### `UpdatePreviewColor()`
**O que faz:** Atualiza o círculo grande que mostra a cor selecionada.

#### `UpdateIndicators()`
**O que faz:** Move os marcadores visuais (o ponto no quadrado e a barra no slider de matiz).

```csharp
private void UpdateIndicators()
{
    // Posição do indicador de matiz (barra vertical)
    double hueY = (_hue / 360.0) * HueSlider.ActualHeight;
    Canvas.SetTop(HueIndicator, hueY - 5);

    // Posição do indicador no quadrado (círculo)
    double satX = _saturation * SatValSquare.ActualWidth;
    double valY = (1 - _value) * SatValSquare.ActualHeight;
    Canvas.SetLeft(SatValIndicator, satX - 8);
    Canvas.SetTop(SatValIndicator, valY - 8);
}
```

### Handlers de Slider

#### `HueSliderControl_ValueChanged()`
**O que faz:** Quando você move o slider de Matiz, atualiza a cor.

#### `SaturationSliderControl_ValueChanged()`
**O que faz:** Quando você move o slider de Saturação, atualiza a cor.

#### `ValueSliderControl_ValueChanged()`
**O que faz:** Quando você move o slider de Brilho, atualiza a cor.

### Handlers de Mouse

#### `SatValSquare_MouseLeftButtonDown()`
**O que faz:** Quando você clica no quadrado de cores, começa a capturar o mouse.

#### `SatValSquare_MouseMove()`
**O que faz:** Quando você arrasta o mouse no quadrado, atualiza a cor.

#### `UpdateSatValFromMouse(Point pos)`
**O que faz:** Calcula a saturação e brilho baseado na posição do mouse.

```csharp
private void UpdateSatValFromMouse(Point pos)
{
    // X = saturação (0 à esquerda, 1 à direita)
    _saturation = Math.Clamp(pos.X / SatValSquare.ActualWidth, 0, 1);
    // Y = brilho invertido (1 em cima, 0 embaixo)
    _value = Math.Clamp(1 - (pos.Y / SatValSquare.ActualHeight), 0, 1);
    
    // Atualiza tudo
    UpdateIndicators();
    UpdateSliderTracks();
    UpdateSliderValues();
    UpdateSelectedColor();
}
```

### Conversão de Cores

#### `RgbToHsv(byte r, byte g, byte b, out double h, out double s, out double v)`
**O que faz:** Converte uma cor RGB (Vermelho, Verde, Azul) para HSV (Matiz, Saturação, Brilho).

**Explicação simples:**
- **RGB**: Valor de 0-255 para cada componente (vermelho, verde, azul)
- **HSV**: Matiz (0-360°), Saturação (0-1), Brilho (0-1)

#### `HsvToColor(double h, double s, double v)`
**O que faz:** Converte HSV de volta para uma cor RGB usável pelo sistema.

---

## 🔄 ColorToBrushConverter.cs - Conversores

Conversores são "tradutores" que o sistema usa para conectar dados com a interface.

### `ColorToBrushConverter`
**O que faz:** Converte uma string hexadecimal (ex: "#FF0000") para um pincel de cor.

```csharp
public object Convert(object value, ...)
{
    if (value is string hexColor)
    {
        // Tenta converter "#FF0000" para uma cor
        var color = (Color)ColorConverter.ConvertFromString(hexColor);
        return new SolidColorBrush(color);
    }
    // Se falhar, retorna verde limão
    return new SolidColorBrush(Colors.Lime);
}
```

### `BoolToVisibilityConverter`
**O que faz:** Converte `true`/`false` para `Visível`/`Invisível`.

```csharp
// true  → Visibility.Visible (visível)
// false → Visibility.Collapsed (invisível)
```

---

## 🖥️ NativeMethods.cs - Funções do Windows

Este arquivo usa funções especiais do Windows para fazer a janela do crosshair "invisível" para cliques.

### Constantes
```csharp
public const int GWL_EXSTYLE = -20;        // Índice para estilos estendidos
public const int WS_EX_TRANSPARENT = 0x00000020; // Clique atravessa a janela
public const int WS_EX_TOOLWINDOW = 0x00000080;  // Esconde da barra de tarefas
public const int WS_EX_LAYERED = 0x00080000;     // Permite transparência
```

### Funções

#### `MakeWindowClickThrough(IntPtr hwnd)`
**O que faz:** Faz com que cliques do mouse passem ATRAVÉS da janela, como se ela não existisse.

```csharp
public static void MakeWindowClickThrough(IntPtr hwnd)
{
    // Pega o estilo atual da janela
    int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
    // Adiciona o estilo "transparente para cliques"
    SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
}
```

#### `MakeWindowToolWindow(IntPtr hwnd)`
**O que faz:** Esconde a janela da barra de tarefas e do Alt+Tab.

#### `ApplyOverlayStyles(IntPtr hwnd)`
**O que faz:** Aplica TODOS os estilos necessários de uma vez (transparente + escondida).

---

## ⚙️ CrosshairSettings.cs - Modelo de Configurações

Classe simples que armazena todas as configurações do crosshair.

### Propriedades
```csharp
public string Color { get; set; } = "#00FF00";     // Cor em hexadecimal
public double Size { get; set; } = 20;              // Tamanho das linhas
public double Thickness { get; set; } = 2;          // Espessura das linhas
public double Gap { get; set; } = 4;                // Espaço do centro
public double Opacity { get; set; } = 1.0;          // Transparência
public bool ShowDot { get; set; } = true;           // Mostrar ponto central
public bool ShowCircle { get; set; } = false;       // Mostrar círculo
public double CircleRadius { get; set; } = 15;      // Raio do círculo
public double DotSize { get; set; } = 4;            // Tamanho do ponto
```

### Funções

#### `Clone()`
**O que faz:** Cria uma cópia exata das configurações (útil para presets).

```csharp
public CrosshairSettings Clone()
{
    return new CrosshairSettings
    {
        Color = this.Color,
        Size = this.Size,
        // ... copia todas as propriedades
    };
}
```

---

## 📋 Preset.cs - Presets Salvos

Representa um preset (configuração salva com um nome).

### Propriedades
```csharp
public string Name { get; set; }                    // Nome do preset
public CrosshairSettings Settings { get; set; }    // As configurações
```

### Construtores
```csharp
// Construtor vazio (para deserialização JSON)
public Preset() { }

// Construtor com nome e configurações
public Preset(string name, CrosshairSettings settings)
{
    Name = name;
    Settings = settings.Clone(); // Faz uma cópia para não afetar o original
}
```

---

## 💾 SettingsService.cs - Salvar e Carregar

Serviço responsável por salvar e carregar configurações em arquivos JSON.

### Variáveis
```csharp
private readonly string _settingsPath;  // Caminho do arquivo settings.json
private readonly string _presetsPath;   // Caminho do arquivo presets.json
private readonly JsonSerializerOptions _jsonOptions; // Opções de formatação
```

### Construtor
```csharp
public SettingsService()
{
    // Pega o diretório onde o .exe está
    var appDir = AppDomain.CurrentDomain.BaseDirectory;
    
    // Define os caminhos dos arquivos
    _settingsPath = Path.Combine(appDir, "settings.json");
    _presetsPath = Path.Combine(appDir, "presets.json");
    
    // Configura para JSON formatado e insensível a maiúsculas
    _jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
}
```

### Funções

#### `SaveSettings(CrosshairSettings settings)`
**O que faz:** Salva as configurações atuais em `settings.json`.

```csharp
public void SaveSettings(CrosshairSettings settings)
{
    try
    {
        // Converte para JSON
        var json = JsonSerializer.Serialize(settings, _jsonOptions);
        // Escreve no arquivo
        File.WriteAllText(_settingsPath, json);
    }
    catch (Exception ex)
    {
        // Se falhar, apenas registra o erro (não trava o programa)
        System.Diagnostics.Debug.WriteLine($"Falha ao salvar: {ex.Message}");
    }
}
```

#### `LoadSettings()`
**O que faz:** Carrega as configurações de `settings.json`, ou retorna padrão se não existir.

#### `SavePresets(List<Preset> presets)`
**O que faz:** Salva todos os presets em `presets.json`.

#### `LoadPresets()`
**O que faz:** Carrega todos os presets de `presets.json`.

---

## 🧠 CrosshairViewModel.cs - O Cérebro

Este é o arquivo mais importante! É o "cérebro" que controla toda a lógica da aplicação.

### Campos Privados
```csharp
private string _colorHex = "#00FF00";    // Cor atual
private double _size = 20;                // Tamanho
private double _thickness = 2;            // Espessura
private double _gap = 4;                  // Espaço central
private double _opacity = 1.0;            // Opacidade
private bool _showDot = true;             // Mostrar ponto
private bool _showCircle = false;         // Mostrar círculo
private double _circleRadius = 15;        // Raio do círculo
private double _dotSize = 4;              // Tamanho do ponto
private bool _crosshairEnabled = true;    // Crosshair ativo
private bool _showTShape = false;         // Formato T
private bool _showHorizontalLines = true; // Linhas horizontais
private bool _showVerticalLines = true;   // Linhas verticais
```

### Propriedades Principais

#### `ColorHex`
**O que faz:** Cor do crosshair em formato hexadecimal.
**Quando muda:** Notifica a interface para atualizar e sincroniza os valores RGB.

#### `CrosshairBrush`
**O que faz:** Retorna um "pincel" de cor para uso na interface.
**Tipo:** Propriedade calculada (não armazena, calcula quando perguntado).

#### Propriedades RGB (`ColorRed`, `ColorGreen`, `ColorBlue`)
**O que fazem:** Componentes individuais da cor (0-255 cada).
**Quando mudam:** Atualizam o ColorHex automaticamente.

### Funções de Sincronização de Cores

#### `UpdateHexFromRgb()`
**O que faz:** Quando você muda R, G ou B, atualiza a cor hexadecimal.

```csharp
private void UpdateHexFromRgb()
{
    if (_isUpdatingRgb) return; // Evita loop infinito
    
    // Formata como #RRGGBB
    _colorHex = $"#{_colorRed:X2}{_colorGreen:X2}{_colorBlue:X2}";
    
    // Notifica a interface
    OnPropertyChanged(nameof(ColorHex));
    OnPropertyChanged(nameof(CrosshairBrush));
    OnPropertyChanged(nameof(PreviewColor));
}
```

#### `UpdateRgbFromHex()`
**O que faz:** Quando a cor hexadecimal muda, atualiza os valores R, G, B.

### Propriedades de Posição (Calculadas)

```csharp
private const double CanvasCenter = 100; // Centro do canvas 200x200

// Linha de cima
public double TopLineY1 => CanvasCenter - Gap;
public double TopLineY2 => CanvasCenter - Gap - Size;

// Linha de baixo
public double BottomLineY1 => CanvasCenter + Gap;
public double BottomLineY2 => CanvasCenter + Gap + Size;

// Linha da esquerda
public double LeftLineX1 => CanvasCenter - Gap;
public double LeftLineX2 => CanvasCenter - Gap - Size;

// Linha da direita
public double RightLineX1 => CanvasCenter + Gap;
public double RightLineX2 => CanvasCenter + Gap + Size;

// Posição do ponto central
public double DotLeft => CanvasCenter - DotSize / 2;
public double DotTop => CanvasCenter - DotSize / 2;

// Posição do círculo
public double CircleLeft => CanvasCenter - CircleRadius;
public double CircleTop => CanvasCenter - CircleRadius;
public double CircleDiameter => CircleRadius * 2;
```

### Comandos (Para Botões)

```csharp
public RelayCommand SavePresetCommand { get; }   // Botão "Salvar Preset"
public RelayCommand LoadPresetCommand { get; }   // Botão "Carregar Preset"
public RelayCommand DeletePresetCommand { get; } // Botão "Excluir Preset"
```

### Funções de Preset

#### `SavePreset()`
**O que faz:** Salva as configurações atuais como um novo preset.

```csharp
private void SavePreset()
{
    // Não salva se o nome está vazio
    if (string.IsNullOrWhiteSpace(NewPresetName)) return;

    // Cria o preset
    var preset = new Preset(NewPresetName.Trim(), GetCurrentSettings());
    
    // Remove preset antigo com mesmo nome (se existir)
    var existing = Presets.FirstOrDefault(p => p.Name == preset.Name);
    if (existing != null) Presets.Remove(existing);

    // Adiciona e salva
    Presets.Add(preset);
    _settingsService.SavePresets(Presets.ToList());
    
    // Seleciona o novo preset e limpa o campo
    SelectedPreset = preset;
    NewPresetName = string.Empty;
}
```

#### `LoadPreset()`
**O que faz:** Aplica as configurações do preset selecionado.

#### `DeletePreset()`
**O que faz:** Remove o preset selecionado.

### Funções Principais

#### `ApplySettings(CrosshairSettings settings)`
**O que faz:** Aplica um conjunto de configurações ao ViewModel.

#### `GetCurrentSettings()`
**O que faz:** Retorna as configurações atuais como um objeto `CrosshairSettings`.

### INotifyPropertyChanged

#### `OnPropertyChanged(string? propertyName)`
**O que faz:** Avisa a interface que uma propriedade mudou e precisa ser atualizada.

```csharp
protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
{
    // Dispara o evento para todos que estão "ouvindo"
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

---

## 🎮 RelayCommand.cs - Comandos

Implementação do padrão Command para conectar botões com funções.

### Campos
```csharp
private readonly Action<object?> _execute;      // O que fazer quando executar
private readonly Predicate<object?>? _canExecute; // Pode executar?
```

### Construtores
```csharp
// Com parâmetro
public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)

// Sem parâmetro (mais simples)
public RelayCommand(Action execute, Func<bool>? canExecute = null)
```

### Funções

#### `CanExecute(object? parameter)`
**O que faz:** Verifica se o comando pode ser executado (botão ativo/inativo).

#### `Execute(object? parameter)`
**O que faz:** Executa a ação do comando.

#### `RaiseCanExecuteChanged()`
**O que faz:** Força a interface a re-verificar se o comando pode ser executado.

---

## 🖼️ ControlPanelWindow.xaml.cs - Painel de Controle

Janela principal de configuração com abas.

### Construtor
```csharp
public ControlPanelWindow()
{
    InitializeComponent();

    // Fechar esta janela fecha o programa
    Closed += ControlPanelWindow_Closed;

    // Configura as abas
    TabShape.Checked += (s, e) => SwitchTab(0);    // Aba Forma
    TabColor.Checked += (s, e) => SwitchTab(1);    // Aba Cor
    TabProfiles.Checked += (s, e) => SwitchTab(2); // Aba Perfis
}
```

### Funções

#### `SwitchTab(int tabIndex)`
**O que faz:** Muda a aba visível.

```csharp
private void SwitchTab(int tabIndex)
{
    // Esconde todos os painéis
    PanelShape.Visibility = Visibility.Collapsed;
    PanelColor.Visibility = Visibility.Collapsed;
    PanelProfiles.Visibility = Visibility.Collapsed;

    // Mostra o painel selecionado
    switch (tabIndex)
    {
        case 0: PanelShape.Visibility = Visibility.Visible; break;
        case 1: PanelColor.Visibility = Visibility.Visible; break;
        case 2: PanelProfiles.Visibility = Visibility.Visible; break;
    }
}
```

#### `ControlPanelWindow_Closed()`
**O que faz:** Fecha todo o aplicativo quando a janela é fechada.

#### `ColorButton_Click()`
**O que faz:** Quando clica em um botão de cor rápida, aplica aquela cor.

#### `ColorPicker_SelectedColorChanged()`
**O que faz:** Quando a cor do seletor muda, atualiza o ViewModel.

#### `ResetButton_Click()`
**O que faz:** Restaura todas as configurações para o padrão.

---

## 🎯 OverlayWindow.xaml.cs - Janela do Crosshair

Janela transparente que exibe o crosshair na tela.

### Construtor
```csharp
public OverlayWindow()
{
    InitializeComponent();

    // Aplica "click-through" depois que a janela é criada
    SourceInitialized += OverlayWindow_SourceInitialized;
    
    // Centraliza na tela depois de carregar
    Loaded += OverlayWindow_Loaded;
}
```

### Funções

#### `OverlayWindow_Loaded()`
**O que faz:** Chamada quando a janela termina de carregar. Centraliza na tela.

#### `CenterOnScreen()`
**O que faz:** Calcula e aplica a posição para centralizar a janela na tela.

```csharp
private void CenterOnScreen()
{
    // Pega dimensões da tela
    var screenWidth = SystemParameters.PrimaryScreenWidth;
    var screenHeight = SystemParameters.PrimaryScreenHeight;
    
    // Calcula posição central
    // A janela é 200x200, então subtrai metade de cada dimensão
    Left = (screenWidth / 2) - (Width / 2);
    Top = (screenHeight / 2) - (Height / 2);
}
```

#### `OverlayWindow_SourceInitialized()`
**O que faz:** Aplica os estilos do Windows para tornar a janela transparente para cliques.

```csharp
private void OverlayWindow_SourceInitialized(object? sender, EventArgs e)
{
    // Pega o "handle" da janela (identificador do Windows)
    var hwnd = new WindowInteropHelper(this).Handle;

    // Aplica os estilos de overlay (transparente + ferramenta)
    NativeMethods.ApplyOverlayStyles(hwnd);
}
```

---

## 📚 Glossário de Termos

| Termo | Significado |
|-------|-------------|
| **ViewModel** | Classe que contém a lógica e dados, conectando interface com código |
| **Binding** | Conexão automática entre dados e interface |
| **DataContext** | O objeto que fornece dados para uma janela |
| **PropertyChanged** | Evento que avisa quando um dado muda |
| **Command** | Objeto que representa uma ação (como clicar um botão) |
| **Handle (hwnd)** | Identificador único de uma janela no Windows |
| **HSV** | Sistema de cores: Hue (matiz), Saturation (saturação), Value (brilho) |
| **RGB** | Sistema de cores: Red (vermelho), Green (verde), Blue (azul) |
| **JSON** | Formato de texto para salvar dados estruturados |
| **Serializar** | Converter objeto para texto (JSON) |
| **Deserializar** | Converter texto (JSON) para objeto |

---

## 🔄 Fluxo de Execução

```
1. Usuário abre CrosshairOverlay.exe
   ↓
2. App.xaml.cs → Application_Startup()
   ↓
3. Carrega configurações (settings.json) e presets (presets.json)
   ↓
4. Cria CrosshairViewModel (o "cérebro")
   ↓
5. Cria OverlayWindow (crosshair) e ControlPanelWindow (controles)
   ↓
6. Ambas as janelas usam o MESMO ViewModel
   ↓
7. Usuário muda configurações no painel
   ↓
8. ViewModel notifica → Interface atualiza → Crosshair muda em tempo real
   ↓
9. Usuário fecha o painel
   ↓
10. App.xaml.cs → Application_Exit() → Salva configurações
```

---

*Documentação gerada para o projeto CrosshairOverlay*
