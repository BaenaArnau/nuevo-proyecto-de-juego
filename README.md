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

## Estructura destacada (rama Pato)

- `assets/` — gráficos organizados por tipo (Enemies, Background, Terrain, etc.).
- `scenes/` — escenas implicadas en esta rama: `player.tscn`, `pato.tscn`, `sierra.tscn`.
- `scripts/` — código en C# relevante añadido/actualizado en esta rama:
  - `Enemigos/Pato.cs` — añadido un enemigo "Pato" que:
    - hereda de `RigidBody2D` y maneja física y animaciones.
    - detecta al jugador mediante áreas y reacciona con saltos cuando el jugador salta cerca.
    - tiene área de daño: si el jugador impacta correctamente, el pato muere y el jugador recibe un rebote.
  - `Player/Player.cs` — ajustado para soportar muerte y rebotes, y para interoperar con enemigos (colisiones/daño).
  - `Trampas/Sierra.cs` — añadida/ajustada la sierra (trampa) que puede causar la muerte del jugador al contacto.
  - `Maquinas de estados/` — pequeños ajustes en estados y `State` para mantener compatibilidad con los cambios de scripts.

## Buenas prácticas

- Mantén los assets con nombres claros y rutas relativas para que Godot los encuentre.
- Vincula scripts a nodos desde el editor de Godot para facilitar la depuración.
- Usa el control de versiones (git) para registrar cambios y crear ramas por característica.

## Cambios implementados en esta rama

En esta rama (PatoEnSierraActividades) se han implementado y/o integrado las siguientes características principales:

- Añadido el enemigo "Pato" (`scripts/Enemigos/Pato.cs`) con detección del jugador, salto reactivo y lógica de muerte.
- Añadida la trampa "Sierra" (`scripts/Trampas/Sierra.cs`) que puede matar al jugador al colisionar.
- Adaptaciones en `Player.cs` para soportar la muerte del jugador y el rebote al interactuar con enemigos.

Estos cambios están pensados para pruebas y desarrollo; la documentación generada por Doxygen y la página publicada reflejarán la versión oficial cuando se haga merge a `main`.

## Licencia

Este proyecto se publica bajo la licencia GNU General Public License v3.0 (GPL-3.0). Consulta el archivo `LICENSE` para el texto completo.
