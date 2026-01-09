# Changelog

All notable changes to NavisTools will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-01-09

### Added
- Initial release of NavisTools
- Calculate Volume, Area, Length totals from selected Navisworks items
- Add parent names to selected items as parameters
- Selection info dockable panel with detailed property display
- Revit parameter ID support for locale-independent property lookup
- Configurable settings dialog for parent parameter names
- Property lookup mode: By display name or by Revit parameter ID
- About dialog with version information and GitHub link
- Centralized version management via Directory.Build.props
- Multi-version support for Navisworks 2022-2026

### Features
- **Total Sums**: Display combined volume, area, length, and count for selected items
- **Parent Name Tool**: Automatically add parent object names to selected items
- **Selection Info Panel**: Real-time display of selected item properties
- **Balloon Notifications**: Non-intrusive user feedback

### Technical
- .NET Framework 4.8
- Navisworks API via Speckle.Navisworks.API
- WinForms UI with MVVM patterns
- Service Locator pattern for dependency injection
- Command pattern for tool extensibility
