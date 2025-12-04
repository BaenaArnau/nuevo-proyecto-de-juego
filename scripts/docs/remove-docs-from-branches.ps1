<#
Remove generated docs/html from one or more branches safely.

Usage examples:
  # Dry run (default): show what would happen on 'main'
  pwsh .\scripts\docs\remove-docs-from-branches.ps1 -Branches main

  # Actually remove and push the commit to origin
  pwsh .\scripts\remove-docs-from-branches.ps1 -Branches main,develop -Force -Push

Parameters:
  -Branches: array of branch names to process (default: 'main')
  -DryRun: default true. If set, the script only prints actions.
  -Push: if set, the script pushes the resulting commit to origin.
  -Force: if set, performs destructive actions (checkout, commit, push). Without -Force the script stops before making changes.

Notes:
- The script does NOT re-write history. It creates a commit that removes docs/html from the specified branches.
- If you need to remove docs from history, use git-filter-repo or BFG (advanced, coordinate with team).
#>
param(
    [string[]]$Branches = @('main'),
    [switch]$DryRun = $true,
    [switch]$Push = $false,
    [switch]$Force = $false
)

function Run-Git([string]$cmd) {
    Write-Host "git $cmd"
    git $cmd
    return $LASTEXITCODE
}

# Confirm
if (-not $Force) {
    Write-Host "*** SAFE MODE ***"
    Write-Host "This will only simulate actions. Pass -Force to execute (and -Push to push)."
}

# Save current branch to return later
$originalBranch = (git rev-parse --abbrev-ref HEAD).Trim()
if ($LASTEXITCODE -ne 0) { Write-Warning "Cannot determine current branch. Aborting."; exit 1 }

foreach ($b in $Branches) {
    Write-Host "\n=== Processing branch: $b ==="

    # Fetch remote branch
    if (Run-Git "fetch origin $b --depth=1" -ne 0) { Write-Warning "Failed to fetch origin/$b. Skipping."; continue }

    # Checkout branch (create local tracking if needed)
    if ($DryRun) {
        Write-Host "DRY RUN: would checkout origin/$b into local branch $b (or update local)"
    } else {
        if ($Force) {
            Write-Host "Checking out $b"
            if (Run-Git "checkout $b" -ne 0) {
                Write-Host "Local branch $b doesn't exist; creating from origin/$b"
                Run-Git "checkout -b $b origin/$b"
            }
        } else {
            Write-Host "Not forced; skipping checkout for $b"
            continue
        }
    }

    # Determine if docs/html is tracked in this branch
    $isTracked = $false
    $ls = git ls-tree -r --name-only origin/$b | Where-Object { $_ -like 'docs/html*' }
    if ($ls) { $isTracked = $true }

    if ($isTracked) {
        Write-Host "docs/html appears tracked in origin/$b"
        if ($DryRun) {
            Write-Host "DRY RUN: would remove tracked docs/html from $b (git rm -r --cached docs/html) and commit"
        } else {
            # Remove from index (respect .gitignore: use --cached so file remains locally if present)
            Run-Git "rm -r --cached docs/html" | Out-Null

            # If files still exist in working tree, remove them too
            if (Test-Path docs/html) {
                try { Remove-Item -Recurse -Force docs/html } catch { }
            }

            # Commit if there are changes
            if (-not (git diff --quiet) -or -not (git diff --cached --quiet)) {
                Run-Git "add -A"
                Run-Git "commit -m \"chore(docs): remove generated docs/html from branch ($b)\""
                if ($Push) { Run-Git "push origin HEAD" }
            } else {
                Write-Host "No changes to commit on $b"
            }
        }
    } else {
        Write-Host "docs/html not tracked in origin/$b — nothing to remove."
    }
}

# Return to original branch if it still exists
if ($originalBranch) {
    Write-Host "\nReturning to original branch: $originalBranch"
    Run-Git "checkout $originalBranch" | Out-Null
}

Write-Host "\nDone."
