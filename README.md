# webview-unity-android
Cargar un index.html local en un WebView dentro de Unity para Android
✅ Resumen final
Paso	Qué hacer
1	Crear carpeta Assets/StreamingAssets/
2	Copiar index.html ahí
3	Copiar Bootstrapper.cs a Assets/Scripts/
4	Añadir el script a un GameObject en la escena
5	Configurar Android en Build Settings
6	Compilar la APK
7	Crear repo en GitHub con .gitignore de Unity
8	Subir Scripts/, StreamingAssets/ y README.md
9	(Opcional) Activar GitHub Pages para el HTML

# Unity WebView con HTML Local

Script para Unity que carga un archivo `index.html` local dentro de un WebView nativo de Android, sin necesidad de conexión a internet.

## ✨ Características

- ✅ Carga un `index.html` local desde `StreamingAssets`
- ✅ Funciona sin conexión a internet
- ✅ Soporta JavaScript, localStorage y CSS
- ✅ Botón "Atrás" de Android integrado
- ✅ Pantalla completa automática
- ✅ Sin pantalla blanca (usa `loadDataWithBaseURL`)

## 📋 Requisitos

- Unity 2019.4 o superior
- Android 5.0 (API 21) o superior
- Módulo de Android Build Support instalado

## 🚀 Cómo usar

1. Crea la carpeta `Assets/StreamingAssets/` en tu proyecto
2. Coloca tu `index.html` dentro de esa carpeta
3. Copia el script `Bootstrapper.cs` a `Assets/Scripts/`
4. Crea un GameObject vacío en tu escena y añádele el componente `Bootstrapper`
5. Configura el proyecto para Android y compila

## 📁 Estructura del proyecto
Assets/
├── Scripts/
│ └── Bootstrapper.cs
└── StreamingAssets/
└── index.html
