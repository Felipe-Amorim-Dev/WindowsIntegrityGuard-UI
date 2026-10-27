<h1 align="center">🖥️ Windows Integrity Guard — UI</h1>

<p align="center"><em>WPF interface for monitoring integrity checks.</em></p>

<p align="center">🇧🇷 <a href="README.md">Português</a> | 🇺🇸 English</p>

<p align="center"><img src="https://img.shields.io/badge/license-MIT-006bed?style=flat-square" alt="MIT" /> <img src="https://img.shields.io/github/last-commit/Felipe-Amorim-Dev/WindowsIntegrityGuard-UI?style=flat-square&color=006bed" alt="Last commit" /></p>

<p align="center"><a href="#sobre">About</a> • <a href="#tecnologias">Technologies</a> • <a href="#como-rodar">Running locally</a> • <a href="#licenca">License</a> • <a href="#autor">Author</a></p>

---

<a id="sobre"></a>
## 📖 About

Windows desktop interface for Windows Integrity Guard, built with WPF and .NET 9. Contains application screens and controls designed to work alongside the separately maintained service.

## ✨ Features

- WPF desktop interface
- Monitoring and control screens

## 🛠️ Technologies

![C#](https://img.shields.io/badge/C%23-006bed?style=for-the-badge) ![.NET 9](https://img.shields.io/badge/.NET_9-006bed?style=for-the-badge) ![WPF](https://img.shields.io/badge/WPF-006bed?style=for-the-badge) ![Windows](https://img.shields.io/badge/Windows-006bed?style=for-the-badge)

## 📦 Prerequisites

Windows and .NET 9 SDK (.NET desktop workload for WPF).

<a id="como-rodar"></a>
## 🚀 Running locally

```bash
git clone https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard-UI.git
cd WindowsIntegrityGuard-UI
dotnet build WindowsIntegrityGuard.UI/WindowsIntegrityGuard.UI.sln
```

## 🔗 Related repositories

- [WindowsIntegrityGuard](https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard) — Repositório agregador
- [WindowsIntegrityGuard-Service](https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard-Service) — Serviço de integridade

## 🏗️ Architecture and components

- .NET 9 WPF desktop application using ViewModels.
- DashboardViewModel exposes ServiceStatus, IntegrityStatus, IntegrityMessage, LastScan and CurrentOperation.
- NamedPipeClient implements IIpcClient and sends JSON requests to the service.
- TrayIconService manages the notification-area icon.

### Technical workflow

1. UI → IIpcClient → NamedPipeClient → WindowsIntegrityGuard pipe → service.
2. The client uses a three-second timeout and parses JSON responses.
3. Dashboard properties expose the state for WPF data binding.

<a id="licenca"></a>
## 📄 License

Distributed under the [MIT license](LICENSE).

<a id="autor"></a>
## 👤 Author

**Felipe Figueiredo Amorim** — Full Stack Developer.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Felipe%20Amorim-0A66C2?style=for-the-badge&logo=linkedin)](https://www.linkedin.com/in/felipe-amorim-dev/) [![E-mail](https://img.shields.io/badge/E--mail-contato-006bed?style=for-the-badge)](mailto:felipe.f.amorim@outlook.com)

<p align="center"><strong>Obrigado por visitar! / Thanks for visiting!</strong></p>
