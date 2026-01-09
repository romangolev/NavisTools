# NavisTools Release Process

## Version Management

The version is centrally managed in `Directory.Build.props`:

```xml
<VersionPrefix>1.0.0</VersionPrefix>
```

This version is automatically applied to:
- All compiled DLLs (NavisTools.dll, _build.exe, Installer.exe)
- The installer package filename
- The About dialog
- GitHub releases

## Creating a Release

### 1. Update Version

Edit `Directory.Build.props` and update the `VersionPrefix`:

```xml
<VersionPrefix>1.1.0</VersionPrefix>
```

### 2. Update CHANGELOG.md

Add a new section for the version with changes:

```markdown
## [1.1.0] - 2026-01-15

### Added
- New feature description

### Fixed
- Bug fix description
```

### 3. Commit and Tag

```bash
git add Directory.Build.props CHANGELOG.md
git commit -m "Release v1.1.0"
git tag v1.1.0
git push origin main --tags
```

### 4. Automated Build

The GitHub Actions workflow will automatically:
1. Build all Navisworks versions (2022-2026)
2. Create the bundle
3. Generate the installer
4. Create a GitHub release with the installer attached
5. Extract release notes from CHANGELOG.md

## Manual Build

To build locally:

```bash
# Build bundle for all Navisworks versions
./build.cmd CreateBundle

# Create installer
dotnet run --project Installer/Installer.csproj
```

The outputs will be in:
- `output/NavisTools.bundle/` - Bundle for Navisworks
- `output/installer/` - Installer executable

## Workflow Details

The CI/CD pipeline runs on:
- **Push to main/develop**: Builds and uploads artifacts
- **Pull requests to main**: Builds only (no release)
- **Tags matching v***: Builds and creates GitHub release

See [.github/workflows/build.yml](.github/workflows/build.yml) for full details.
