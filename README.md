# Windows Integrity Guard — WPF UI

**Status: in development / Em desenvolvimento.** Windows desktop UI for the [Windows Integrity Guard](https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard) project.

Built using C# and WPF, with the service implementation maintained separately in [WindowsIntegrityGuard-Service](https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard-Service).

## Development
Open the solution or project in `WindowsIntegrityGuard.UI/` using Visual Studio on Windows with a compatible .NET desktop SDK. Build and validate the UI in conjunction with the service version referenced by the main repository. Do not assume all planned screens and service integrations are complete.

## Safety
This interface belongs to an experimental Windows maintenance utility. Review actual service behavior before invoking system-repair functionality, and test in a disposable VM.

**PT-BR:** Interface WPF experimental, em evolução. A integração com o serviço e os fluxos finais ainda precisam de validação.
