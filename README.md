# UShell

**A universal, modular shell.**

UShell is a modular, extensible shell built to provide a common foundation for a wide range of functionality without requiring everything to be built into the a single application.

UShell uses a module-based architecture that allows capabilities to be added through DLLs. Modules can extend the interactive shell with commands or provide background services hosted by UShell.

Need Perforce support? Add the [Perforce module](https://github.com/EntexInteractive/UShell.Perforce).

Need Unreal Engine support? Add the [Unreal module](https://github.com/EntexInteractive/UShell.Unreal).

## Core Concepts

UShell separates its core runtime from the functionality it hosts. This keeps the shell extensible and allows different installations to support different workflows.

Its architecture currently supports two types of modules.

### Command Modules

Command modules extend the interactive shell with additional commands and functionality.

They allow integrations and tools to be developed independently of the core application, while remaining accessible through the UShell command-line interface.

Examples include:

- Version control integrations
- Development and build tools
- Project-specific utilities
- Administrative commands
- Custom team workflows

For example, a UE5 command module can provide Unreal-related commands without making Unreal functionality a mandatory part of the core shell.

### Service Modules

Service modules provide functionality that runs in the background, hosted by UShell.

Rather than requiring every component to be an interactive command, service modules allow UShell to host long-running background functionality alongside its command-oriented capabilities.

Potential applications include:

- Background workers and automation
- Scheduled or continuous tasks
- Monitoring and health checks
- Integration services
- Other long-running processes

The exact lifecycle, configuration, and management capabilities depend on the service-hosting infrastructure implemented by UShell.

## Extensible Through DLLs

UShell is designed around independently loadable .NET assemblies.

Instead of bundling every feature, functionality can be packaged into modules and integrated with the shell.

For example:

```text
UShell/
├── UShell.exe
├── UShell.Perforce.dll
└── UShell.Unreal.dll
```

This illustrates the concept; the actual module directory and discovery mechanism depend on the implementation.

The result is a shell whose available functionality can vary depending on the modules installed, without requiring every integration to become part of the core application.

## One Platform, Multiple Roles

UShell brings together two complementary capabilities:

| Capability | Purpose |
|---|---|
| Interactive shell | Provides a common command-line environment |
| Command modules | Add commands and interactive functionality |
| Service modules | Host long-running background functionality |
| Modular architecture | Keeps extensions independent of the core |

This makes UShell useful as both an extensible command-line environment and a host for modular background components.

## Technology

UShell is built with the .NET ecosystem.

Current technologies include:

- **C# / .NET** — application and module implementation
- **Spectre.Console.Cli** — command-line command handling
- **Spectre.Console** — rich terminal output

## Project Status

UShell is under active development. Its core architecture supports command modules and service modules, with the goal of making it straightforward to add new functionality without coupling every feature to the core shell.
