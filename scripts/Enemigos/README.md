# Pato (enemigo)

Descripción breve de lo implementado en `Pato.cs`:

- Tipo: nodo con física — hereda de `RigidBody2D`.
- Comportamiento principal:
  - Reacciona al jugador cuando este salta cerca: detecta cambios en la velocidad Y del jugador y salta (impulso vertical).
  - Tiene una detección por área (trigger) para saber si el jugador está cercano y activar la reacción.
- Movimiento:
  - Velocidad de movimiento configurable desde el inspector (`Export`/propiedad `Speed`).
  - Lógica de salto controlada por flags para evitar saltos múltiples en el aire.
- Interacción con el jugador:
  - Si el jugador colisiona con el área de daño del pato, el pato muere y el jugador recibe un rebote.
  - Si el jugador choca con el pato por otro lado, puede recibir daño (dependiendo de la lógica del jugador).
- Animaciones y nodos:
  - Usa un `AnimatedSprite2D` para las animaciones (`idle`, `run`, `hit`, `die`, etc.).
  - El script obtiene nodos con accesos seguros y emite mensajes mediante llamadas tipadas.
- Seguridad y compatibilidad:
  - Accesos a nodos realizados con `GetNodeOrNull` y castings seguros para evitar NullReference.
  - La muerte y efectos usan métodos asíncronos (`async Task`) donde procede para no bloquear el hilo principal.

Notas de uso

- Escena de ejemplo: `scenes/pato.tscn` (instanciada en `scenes/node_2d.tscn`).
- Para probar: arrastra la escena `pato.tscn` a una escena de prueba y ejecuta el juego. Ajusta `Speed` y `JumpForce` en el inspector del pato.

Si quieres, puedo resumir esto en la documentación generada por Doxygen o añadir ejemplos de configuración (valores recomendados para `Speed` y `JumpForce`).
