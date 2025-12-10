<#
Instala (copia) `docs/html` desde la rama de documentación remota (por defecto `gh-pages`) en la rama actual.
- Por defecto restaura los archivos en el working tree para que puedas revisarlos.
- Opcionalmente puede commitear y pushear los cambios. Si quieres crear un PR automáticamente, el script lo intentará usando la CLI `gh` si está instalada.

Usage:
  pwsh .\scripts\docs\install-docs.ps1 [-DocsBranch gh-pages] [-DocsPath docs/html] [-Method checkout|worktree] [-Commit] [-Push] [-CreatePR]

Ejemplos:
  # Restaurar en working tree (no commitea)
  pwsh .\scripts\docs\install-docs.ps1

  # Restaurar y commitear/pushear en la rama actual
  pwsh .\scripts\docs\install-docs.ps1 -Commit -Push

  # Usar worktree (más seguro) y crear PR si `gh` está disponible
  pwsh .\scripts\docs\install-docs.ps1 -Method worktree -Commit -CreatePR
#>
param(
    [string]$DocsBranch = "gh-pages",
    [string]$DocsPath = "docs/html",
    [ValidateSet("checkout","worktree")][string]$Method = "checkout",
    [switch]$Commit,
    [switch]$Push,
    [switch]$CreatePR,
    [string]$PRBaseBranch = $null,
    [string]$PRTitle = $null
)

function ExitWithError($code, $message) {
    Write-Error $message
    exit $code
}

# Compruebo git
if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    ExitWithError 1 "git no está disponible en PATH. Instala git y vuelve a intentarlo."
}

# Variables
$cwd = (Get-Location).Path
$currentBranch = (git rev-parse --abbrev-ref HEAD).Trim()
if ($LASTEXITCODE -ne 0) { ExitWithError 2 "No se pudo determinar la rama actual." }

# Fetch remoto
Write-Host "Fetching origin/$DocsBranch..."
git fetch origin $DocsBranch
if ($LASTEXITCODE -ne 0) { ExitWithError 3 "Error al hacer 'git fetch origin/$DocsBranch'." }

# Si se usa worktree: crear un worktree temporal y copiar los archivos
$tempWorktree = Join-Path $env:TEMP "docs-worktree-$([guid]::NewGuid().ToString())"
try {
    if ($Method -eq 'worktree') {
        Write-Host "Usando worktree temporal: $tempWorktree"
        git worktree add --detach --checkout $tempWorktree origin/$DocsBranch
        if ($LASTEXITCODE -ne 0) { ExitWithError 4 "No se pudo crear el worktree temporal." }

        $sourcePath = Join-Path $tempWorktree $DocsPath
        if (-not (Test-Path $sourcePath)) {
            ExitWithError 5 "La ruta $DocsPath no existe en origin/$DocsBranch (revisa la rama de docs)."
        }

        # Borrar la carpeta local si existe, para evitar residuos
        if (Test-Path $DocsPath) {
            Write-Host "Limpiando carpeta local $DocsPath antes de copiar..."
            Remove-Item -Recurse -Force $DocsPath
        }

        Write-Host "Copiando $sourcePath -> $DocsPath"
        Copy-Item -Path $sourcePath -Destination $DocsPath -Recurse -Force

        # Limpieza del worktree
        Write-Host "Eliminando worktree temporal..."
        git -C $tempWorktree clean -fdx 2>$null | Out-Null
        git worktree remove $tempWorktree --force
    }
    else {
        # checkout directo desde la rama remota
        Write-Host "Restaurando $DocsPath desde origin/$DocsBranch (modo checkout)..."
        git checkout origin/$DocsBranch -- $DocsPath
        if ($LASTEXITCODE -ne 0) { ExitWithError 6 "Error al restaurar $DocsPath desde origin/$DocsBranch." }
    }
}
finally {
    # En caso de que el worktree exista por fallo, intentar removerlo
    if (Test-Path $tempWorktree) {
        try { git worktree remove $tempWorktree --force } catch { }
    }
}

# Añadir al index (forzando si está en .gitignore)
Write-Host "Añadiendo $DocsPath al index (git add -f)..."
git add -f $DocsPath
if ($LASTEXITCODE -ne 0) { ExitWithError 7 "Error en 'git add' para $DocsPath." }

if ($Commit) {
    if ([string]::IsNullOrWhiteSpace($PRBaseBranch)) { $PRBaseBranch = $currentBranch }
    $msg = "chore(docs): instalar docs desde $DocsBranch en $currentBranch"
    Write-Host "Creando commit: $msg"
    git commit -m $msg
    if ($LASTEXITCODE -ne 0) {
        Write-Host "No se creó commit (posible ausencia de cambios). Continuando..."
    } else {
        Write-Host "Commit creado."
    }

    if ($Push) {
        Write-Host "Pusheando la rama actual ($currentBranch) a origin..."
        git push origin HEAD
        if ($LASTEXITCODE -ne 0) { ExitWithError 8 "Error al hacer git push." }
        Write-Host "Push completado."
    }

    if ($CreatePR) {
        if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
            Write-Warning "La CLI 'gh' no está disponible. No puedo crear el PR automáticamente. Instala https://cli.github.com/ para habilitar esa funcionalidad."
        } else {
            # Crear rama temporal y PR
            $timestamp = Get-Date -Format "yyyyMMddHHmmss"
            $tempBranch = "docs-for-$($PRBaseBranch)-$timestamp"
            Write-Host "Creando rama temporal $tempBranch basada en $PRBaseBranch"
            git checkout -b $tempBranch origin/$PRBaseBranch
            if ($LASTEXITCODE -ne 0) { Write-Warning "No se pudo crear checkout de $PRBaseBranch; intentando crear desde HEAD." ; git checkout -b $tempBranch }

            # Añadir y commitear en la rama temporal
            git add -f $DocsPath
            git commit -m "chore(docs): añadir docs desde $DocsBranch para $PRBaseBranch"
            git push --set-upstream origin $tempBranch

            # Crear PR
            if ([string]::IsNullOrWhiteSpace($PRTitle)) { $PRTitle = "Actualizar docs desde $DocsBranch -> $PRBaseBranch" }
            gh pr create --base $PRBaseBranch --head $tempBranch --title "$PRTitle" --body "Actualiza la carpeta $DocsPath con los archivos generados en $DocsBranch."
            Write-Host "PR creado (siempre que gh esté autenticado y tenga permisos)."
        }
    }
}
else {
    Write-Host "Operación completada: los archivos de docs han sido restaurados en el working tree en '$DocsPath'."
    Write-Host "Si quieres commitearlos y empujarlos, ejecuta: pwsh .\scripts\docs\install-docs.ps1 -Commit -Push"
}

Write-Host "Hecho."