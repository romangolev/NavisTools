# AGENTS.md

## Repository Notes for OpenCode

- CI workflow: `.github/workflows/build.yml`
- On push to `main`, GitHub Actions runs the build and calls `PublishGitHubRelease`.
- This publishes a GitHub Release automatically (not a draft) when successful.
- The release/tag name is `v<VersionPrefix>` from `Directory.Build.props`.
- If that release/tag already exists, release publishing fails.
- Before a new release, bump `VersionPrefix` and update `CHANGELOG.md`.

## Local Build

- Bundle: `./build.cmd CreateBundle`
- Installer + release target: `./build.cmd PublishGitHubRelease --GitHubToken <token>`

## App Structure

- `NavisTools/` is the main plugin project for Autodesk Navisworks.
- `NavisTools/Commands/` contains command entry points and command wiring.
- `NavisTools/Services/` contains business logic and external integrations.
- `NavisTools/ViewModels/` and `NavisTools/UI/` contain presentation logic and views.
- `NavisTools/Models/` contains domain/data models.
- `NavisTools/Interfaces/` defines contracts used for dependency inversion and testability.
- `NavisTools/Helpers/` and `NavisTools/Tools/` contain reusable utilities.
- `Installer/` contains installer generation (`InstallerCreator.cs`, `.iss` script).
- `Build/` contains NUKE build targets for bundle, installer, and release automation.

## Engineering Guidelines

- Follow SOLID principles for all new code and refactors.
- Prefer interface-driven design and dependency inversion over hard-coded dependencies.
- Keep classes focused on a single responsibility; split large classes by concern.
- Extend behavior via composition/abstractions instead of modifying stable core flows.
- Keep UI, domain logic, and infrastructure concerns separated.
