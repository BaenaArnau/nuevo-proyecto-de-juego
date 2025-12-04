# Documentación del proyecto

Esta carpeta contiene la documentación generada por Doxygen para el proyecto "Nuevo Proyecto de Juego".

- `Doxyfile` — configuración usada para generar la documentación.
- `html/` — salida HTML generada por Doxygen (si se ha ejecutado).

Para (re)generar la documentación en tu máquina (PowerShell):

```pwsh
doxygen docs/Doxyfile
```

Luego abre `docs/html/index.html` en tu navegador.

Clases útiles (enlaces rápidos):

- [Player](class_nuevo_proyectode_juego_1_1scripts_1_1_player_1_1_player.html)
- [Pato (enemigo)](class_nuevo_proyectode_juego_1_1scripts_1_1_enemigos_1_1_pato.html)
- [Sierra (trampa)](class_nuevo_proyectode_juego_1_1scripts_1_1_trampas_1_1_sierra.html)

Si quieres que aparezcan mini-previews (SVG) en la página principal, dímelo y los incluiré directamente en este fichero.
Si quieres que aparezcan mini-previews (SVG) en la página principal, dímelo y los incluiré directamente en este fichero.

Publicación automática (GitHub Pages)
-----------------------------------

He añadido un workflow de GitHub Actions en `.github/workflows/deploy-docs.yml` que genera la documentación con Doxygen y publica la carpeta `docs/html` en la rama `gh-pages`. El flujo funciona así:

- Al hacer push a `main` (o a la rama `PatoEnSierraActividades`) se ejecuta el workflow.
- El runner instala `doxygen` y `graphviz`, ejecuta `doxygen docs/Doxyfile` y despliega `docs/html` a la rama `gh-pages`.

Después de la primera ejecución revisa en la pestaña "Settings → Pages" de tu repositorio que la fuente de GitHub Pages está configurada a la rama `gh-pages` (en muchos casos GitHub la establece automáticamente cuando la rama existe). Si quieres puedo añadir un pequeño badge o configurar el workflow para ejecutarse sólo en `main`.

Opciones útiles:
- Activar `EXTRACT_PRIVATE = YES` en `Doxyfile` para incluir miembros privados.
- Integrar Doxygen en CI para publicar `docs/html` automáticamente.
