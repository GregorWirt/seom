# Diplomarbeit SEOM - Self Employed Order Management

Projektpartner: Taurus Pioneering e.U.  
Betreuer: Klaus Unger

## Voraussetzungen

Für die Ausführung wird die .NET 6 SDK benötigt.
Sie kann von https://dotnet.microsoft.com/en-us/download geladen werden.

## Starten des Servers

Start die Datei *startServer.cmd* (Windows) bzw. führe das Skript *startServer.sh* (macOS, Linux) aus.

## Deployment in Microsoft Azure

Voraussetzungen: Lade das [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli) Tool.
Führe danach in der Konsole `az login` aus und melde dich mit deinem Azure Account an.
Gehe danach in den Ordner *Source* und starte in der Bash (git bash, ...) das Skript *deploy_app.sh*.
