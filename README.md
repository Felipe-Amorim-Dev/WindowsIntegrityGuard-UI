# Windows Integrity Guard — Interface WPF

Interface Windows do [Windows Integrity Guard](https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard). O objetivo é apresentar o andamento da verificação, os resultados e os controles permitidos ao usuário.

**Estado:** em desenvolvimento. A interface não deve ser tratada como instalador ou cliente de produção.

## Ambiente necessário

- Windows 10 ou 11;
- Visual Studio com a carga de trabalho de desenvolvimento desktop com .NET;
- SDK .NET compatível com o `.csproj`;
- repositório de [serviço](https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard-Service) para avaliar o fluxo completo.

## Abrir e compilar

```powershell
git clone https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard-UI.git
cd WindowsIntegrityGuard-UI
```

Abra o projeto WPF em `WindowsIntegrityGuard.UI/WindowsIntegrityGuard.UI/` no Visual Studio. Faça a compilação local antes de executar, e confira dependências com o Core e o serviço. A solução agregadora pode ser obtida com:

```powershell
git clone --recurse-submodules https://github.com/Felipe-Amorim-Dev/WindowsIntegrityGuard.git
```

## Limites atuais

O repositório contém uma implementação WPF, mas ainda não foi validado aqui um fluxo ponta a ponta com o serviço instalado. As telas e a comunicação devem ser conferidas no código; não se deve assumir que todas as operações já funcionam.

Não há captura de tela verificada anexada a este README. Imagens reais poderão ser incluídas após a conferência visual de uma versão compilada.

**Segurança:** nunca solicitar reparo do sistema sem autorização clara do usuário; valide o serviço em VM antes de utilizar em máquinas reais.
