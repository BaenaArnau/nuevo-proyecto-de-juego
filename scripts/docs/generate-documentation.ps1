<#
Regenerar la documentación del proyecto y abrir el índice HTML.

Uso:
  .\scripts\docs\generate-documentation.ps1

Requisitos:
  - Doxygen instalado y accesible en PATH
  - (Opcional) Graphviz instalado y `dot` en PATH
#>

Set-StrictMode -Version Latest

$doxyfile = Resolve-Path "docs/Doxyfile"
Write-Host "Generando documentación con Doxygen..." -ForegroundColor Cyan
doxygen $doxyfile

$index = Resolve-Path "docs/html/index.html" -ErrorAction SilentlyContinue
if ($index) {
    Write-Host "Apertura de docs/html/index.html en el navegador..." -ForegroundColor Green
    Start-Process $index.Path
} else {
    Write-Host "No se ha generado docs/html/index.html. Comprueba la salida de Doxygen." -ForegroundColor Yellow
}
