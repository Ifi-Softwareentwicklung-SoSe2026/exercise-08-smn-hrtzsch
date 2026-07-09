# Task Briefing fuer AI-Agenten

Diese Datei dient der Middleware als strukturierte Grundlage fuer die automatisch erzeugten Issues von Maria. Die fachlichen Erklaerungen, Beispiele und Reflexionsfragen stehen in `README.md`.

## Ziel der Uebung

Diese Uebung fuehrt schrittweise von linearer, eng gekoppelter Prozesssimulation zu einer entkoppelten Architektur mit Delegates und Events.

## Rahmenbedingungen

- Sprache: C#
- Projekt: `src/ProcessSimulator/ProcessSimulator.csproj`
- Ausgabe: Konsole
- Stil: klare Trennung von Prozesslogik und UI/Ausgabe
- Arbeitsweise: ein Part pro Branch und Pull Request, PR-Body mit `Closes #<Issue-Nummer>`

# Part 1 -- Lineare Implementierung verstehen

<!-- agent-assignment-part:{"kind":"generic","required_checks":[]} -->

## Aufgabe

Analysiere die lineare Ausgangsloesung, bevor du Code refaktorisierst.

## To-dos

- Oeffne `src/ProcessSimulator/Program.cs` und fuehre das Programm lokal aus.
- Beantworte die vier Analysefragen direkt in `README.md` an den vorgesehenen Stellen.
- Unterscheide konkret zwischen Prozesslogik, Konsolenvisualisierung, Warnlogik und Abschlussmeldung.
- Beschreibe kurz, was in einer GUI- oder Webanwendung schwer wiederverwendbar waere.
- Beschreibe kurz, was fuer zusaetzliches Logging am linearen Code geaendert werden muesste.
- Passe den LiaScript-Badge-Link am Anfang der `README.md` auf dein Repository an.

## Akzeptanzkriterien

- Alle vier Antwortfelder in `README.md` sind konkret ausgefuellt.
- Die Antworten beziehen sich sichtbar auf den vorhandenen Code in `Program.cs`.
- Der LiaScript-Badge-Link zeigt nicht mehr auf das Template-Repository.
- Fuer diesen Part sind noch keine C#-Refactorings erforderlich.

# Part 2 -- Delegate einfuehren

<!-- agent-assignment-part:{"kind":"csharp","required_checks":["dotnet-build"]} -->

## Aufgabe

Fuehre einen Delegate ein, damit die Prozesssimulation Fortschritt meldet, ohne die konkrete Konsolenausgabe selbst zu kennen.

## To-dos

- Definiere einen Delegate mit der vorgeschlagenen Signatur oder einer fachlich gleichwertigen Signatur: `public delegate void ProgressReporter(string stepName, int percent);`.
- Lagere die Simulation eines Prozessschritts so aus, dass der Fortschritt ueber diesen Delegate gemeldet wird.
- Verschiebe die Fortschrittsbalken-Ausgabe in eine Methode, die zur Delegate-Signatur passt.
- Die Prozesslogik soll den Delegate aufrufen, aber nicht mehr direkt den Fortschrittsbalken zeichnen.
- Stelle sicher, dass der sichtbare Ablauf im Terminal weiterhin funktioniert.

## Akzeptanzkriterien

- Der Delegate-Typ existiert und wird tatsaechlich verwendet.
- Die Simulationsschleife ruft den Delegate fuer Fortschrittsaenderungen auf.
- Fortschrittsausgabe und Prozessablauf sind mindestens erkennbar getrennt.
- `dotnet build src/ProcessSimulator/ProcessSimulator.csproj --configuration Release` ist erfolgreich.
- `dotnet run --project src/ProcessSimulator/ProcessSimulator.csproj` zeigt weiterhin die Prozesssimulation.

# Part 3 -- Mehrere Subscriber anbinden

<!-- agent-assignment-part:{"kind":"csharp","required_checks":["dotnet-build"]} -->

## Aufgabe

Nutze den Delegate als Multicast-Delegate, damit mehrere unabhaengige Reaktionen auf denselben Fortschrittsstand erfolgen.

## To-dos

- Binde mindestens zwei Subscriber an den Progress-Delegate an.
- Ein Subscriber zeichnet weiterhin den Fortschrittsbalken.
- Ein zweiter Subscriber gibt pro Prozessschritt bei 50 Prozent eine Warnung aus.
- Optional: Ergaenze einen Logging-Subscriber fuer Fortschrittsmeldungen.
- Achte darauf, dass Warnung, Logging und Visualisierung nicht in die Kernschleife zurueckwandern.
- Optional: Stabilisiere das Konsolenlayout mit `Console.SetCursorPosition(left, top)` und `Console.Clear()`.

## Akzeptanzkriterien

- Der Delegate hat mehrere Subscriber, nicht nur eine Methode mit mehreren Aufgaben.
- Die Prozesslogik kennt die konkreten Subscriber nicht.
- Bei 50 Prozent erscheint pro Schritt eine nachvollziehbare Warnung.
- Der Fortschrittsbalken funktioniert weiterhin.
- `dotnet build src/ProcessSimulator/ProcessSimulator.csproj --configuration Release` ist erfolgreich.

# Part 4 -- Auf Events refaktorieren

<!-- agent-assignment-part:{"kind":"csharp","required_checks":["dotnet-build"]} -->

## Aufgabe

Refaktoriere die Delegate-Verkabelung zu einer event-basierten Struktur mit typisierten EventArgs und sauber getrennten Verantwortlichkeiten.

## To-dos

- Erstelle eine Klasse `ProcessRunner`, die den simulierten Prozess steuert.
- `ProcessRunner` stellt Events bereit, z. B. `ProgressChanged`, `StepStarted`, `StepCompleted` und `ProcessCompleted`.
- Verwende typisierte EventArgs-Klassen, z. B. `ProgressChangedEventArgs` und `ProcessStepEventArgs`.
- `ProcessRunner` darf keine direkte Konsolen-UI kennen und soll nicht selbst `Console.WriteLine` fuer Visualisierung/Warnung nutzen.
- Schließe Fortschrittsbalken, Warnung und optional Logging als Event-Subscriber an.
- Events sollen innerhalb der besitzenden Klasse ausgeloest werden; von aussen wird nur abonniert (`+=`) oder abgemeldet (`-=`).

## Akzeptanzkriterien

- `ProcessRunner` kapselt den Prozessablauf und loest Events aus.
- Event-Daten werden typisiert ueber `EventArgs` uebergeben.
- Prozesslogik und Konsolen-UI sind klar getrennt.
- Fortschrittsbalken, Warnung und optional Logging reagieren ueber Event-Subscriptions.
- Neues Verhalten koennte hinzugefuegt werden, ohne die Kernprozesslogik zu aendern.
- `dotnet build src/ProcessSimulator/ProcessSimulator.csproj --configuration Release` ist erfolgreich.
- `dotnet run --project src/ProcessSimulator/ProcessSimulator.csproj` zeigt weiterhin einen vollstaendigen simulierten Prozess.
