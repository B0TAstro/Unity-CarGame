#!/usr/bin/env bash
set -euo pipefail

# --- 1. Lire les arguments : version (obligatoire) + --dry-run (optionnel) ---
VERSION=""
DRY_RUN=false
for arg in "$@"; do
  case "$arg" in
    --dry-run) DRY_RUN=true ;;
    *)         VERSION="$arg" ;;
  esac
done
if [[ ! "$VERSION" =~ ^v[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
  echo "❌ Version manquante ou invalide."
  echo "   Usage : $0 vMAJEUR.MINEUR.PATCH [--dry-run]   (ex: $0 v1.0.0)"
  exit 1
fi

# --- 2. Se placer à la racine du repo (le script vit dans scripts/) ---
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

WIN_DIR="Builds/Windows"
MAC_DIR="Builds/macOS"
GUIDE="HOW_TO_PLAY.md"
WIN_ZIP="$ROOT/Builds/NeedForDrift-Windows.zip"
MAC_ZIP="$ROOT/Builds/NeedForDrift-macOS.zip"

# --- 3. Vérifier que les deux builds existent et ne sont pas vides ---
for d in "$WIN_DIR" "$MAC_DIR"; do
  if [[ ! -d "$d" || -z "$(ls -A "$d" 2>/dev/null)" ]]; then
    echo "❌ Build manquant : '$d' est absent ou vide."
    echo "   Dans Unity : File > Build Settings > Build, vers ce dossier."
    exit 1
  fi
done

# --- 4. Vérifs GitHub (ignorées en --dry-run) ---
if [[ "$DRY_RUN" == false ]]; then
  if ! gh auth status >/dev/null 2>&1; then
    echo "❌ gh n'est pas connecté. Lance d'abord : gh auth login"
    exit 1
  fi
  if gh release view "$VERSION" >/dev/null 2>&1; then
    echo "❌ La release '$VERSION' existe déjà. Choisis une autre version."
    exit 1
  fi
fi

# --- 6. Glisser le guide joueur dans chaque build, puis zipper ---
cp "$GUIDE" "$WIN_DIR/"
cp "$GUIDE" "$MAC_DIR/"

rm -f "$WIN_ZIP" "$MAC_ZIP"
echo "📦 Compression du build Windows…"
( cd "$WIN_DIR" && zip -rq "$WIN_ZIP" . -x "*.DS_Store" )
echo "📦 Compression du build macOS…"
( cd "$MAC_DIR" && zip -rq "$MAC_ZIP" . -x "*.DS_Store" )

# --- 7. Publier (ou s'arrêter là en --dry-run) ---
if [[ "$DRY_RUN" == true ]]; then
  echo "🧪 [dry-run] Zips prêts, aucune release créée :"
  ls -lh "$WIN_ZIP" "$MAC_ZIP"
  echo "   Pour publier pour de vrai : $0 $VERSION"
  exit 0
fi

echo "🚀 Publication de la release $VERSION…"
gh release create "$VERSION" \
  --title "Need for Drift $VERSION" \
  --generate-notes \
  "$WIN_ZIP" "$MAC_ZIP"

echo "✅ Release publiée. Page Releases :"
gh release view "$VERSION" --web >/dev/null 2>&1 || true
