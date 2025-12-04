# Nuevo Proyecto de Juego

Proyecto de ejemplo creado con Godot y C# (mono/.NET). Este repositorio contiene los recursos, escenas y scripts de un pequeño juego de plataformas con enemigos y trampas; está pensado para aprendizaje y desarrollo.

## Contenido

- `Nuevo Proyecto de Juego.csproj` / `.sln` — archivos de proyecto .NET
- `project.godot` — archivo del proyecto Godot
- `assets/` — sprites y recursos gráficos
- `scenes/` — escenas de Godot (.tscn)
- `scripts/` — scripts en C# usados por las escenas (por ejemplo `Player.cs`, `Pato.cs`, `Sierra.cs`)

## Requisitos

- Godot Engine (versión compatible con .NET/Mono). Asegúrate de usar una versión de Godot con soporte C# (Mono) que funcione con tu SDK.
- .NET SDK instalado (recomendado .NET 6 o 7 según el proyecto). Verifica con `dotnet --version`.

## Cómo compilar

1. Abre una terminal en la raíz del proyecto.
2. Construir el proyecto C# (opcional si sólo vas a ejecutar desde Godot):

```
dotnet build
```

3. Abrir el proyecto en Godot: lanza el editor de Godot y selecciona la carpeta del proyecto (la que contiene `project.godot`).
4. Dentro de Godot, asegúrate de que las dependencias .NET/Mono están configuradas correctamente. Después, ejecuta la escena principal desde el editor.

Nota: Si Godot no detecta los assemblies C#, puede que necesites reconstruir desde el editor o ajustar la versión de Mono usada por Godot.

## Estructura destacada

- `assets/` — gráficos organizados por tipo (Enemies, Background, Terrain, etc.).
- `scenes/` — escenas principales: `player.tscn`, `pato.tscn`, `sierra.tscn`, `node_2d.tscn`.
- `scripts/` — código en C#:
  - `Player/Player.cs` — lógica del jugador
  - `Enemigos/Pato.cs` — ejemplo de enemigo "pato"
  - `Trampas/Sierra.cs` — ejemplo de trampa
  - `Maquinas de estados/State.cs` — base para máquinas de estado

## Buenas prácticas

- Mantén los assets con nombres claros y rutas relativas para que Godot los encuentre.
- Vincula scripts a nodos desde el editor de Godot para facilitar la depuración.
- Usa el control de versiones (git) para registrar cambios y crear ramas por característica.

## Licencia

Este proyecto se publica bajo la licencia GNU General Public License v3.0 (GPL-3.0). Consulta el archivo `LICENSE` para el texto completo.
