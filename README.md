# Erinnerungs-App 📅

Eine moderne Windows-Desktopanwendung zur Verwaltung von Erinnerungen und Terminen mit einer intuitiven Benutzeroberfläche.

## ✨ Features

- **Intuitive Benutzeroberfläche** 🎨: Moderne WPF-Oberfläche mit elegantem Gradient-Design
- **Termineingabe**: Einfache Erfassung von Ereignissen mit Grund, Datum und Uhrzeit
- **Erinnerungsfunktion** ⏱️: Aktivierung von Erinnerungen für geplante Termine
- **Verwaltungsfunktionen**: Löschen und Bearbeiten von Einträgen
- **Visuelles Feedback** 🔔: Bestätigungsdialoge für Benutzeraktionen

## 🛠️ Technologie-Stack

- **Framework**: .NET Framework 4.8
- **UI-Technologie**: Windows Presentation Foundation (WPF)
- **Sprache**: C#
- **Architektur**: XAML-basiertes UI-Design mit Code-Behind-Pattern
- **Entwicklungsumgebung**: Visual Studio

## 📋 Voraussetzungen

- Windows 10 oder höher
- .NET Framework 4.8 oder höher
- Visual Studio 2019 oder höher (für Entwicklung)

## 🚀 Installation

1. Öffnen Sie die Solution-Datei `Meine_Erinnerungs_app.sln` in Visual Studio
2. Stellen Sie sicher, dass alle NuGet-Pakete wiederhergestellt sind
3. Wählen Sie die Build-Konfiguration (Debug oder Release)
4. Erstellen Sie die Anwendung über Build → Build Solution

## 📖 Verwendung

### Neue Erinnerung erstellen

1. Geben Sie einen **Grund** für die Erinnerung ein
2. Erfassen Sie das **Datum** des Termins
3. Tragen Sie die **Uhrzeit** ein
4. Klicken Sie auf **GO!** um die Erinnerung zu aktivieren
5. Eine Bestätigung wird angezeigt

### Erinnerung löschen

- Nutzen Sie die **Löschen**-Schaltfläche um Einträge zu entfernen

## 📊 Projektstruktur

```
Meine_Erinnerungs_app/
│
├── App.xaml                    # Anwendungs-Ressourcen und Konfiguration
├── App.xaml.cs                 # Anwendungs-Startlogik
├── MainWindow.xaml             # Haupt-UI-Definition
├── MainWindow.xaml.cs          # UI-Logik und Event-Handler
├── Meine_Erinnerungs_app.csproj # Projektdatei
├── Meine_Erinnerungs_app.sln   # Solution-Datei
└── Properties/                 # Assembly-Informationen und Ressourcen
```

## 🎨 Design-Highlights

- **Farbschema**: Professionelles Blau-Gradient-Design
- **Benutzerführung**: Placeholder-Texte mit automatischer Fokus-Verwaltung
- **Visuelle Effekte**: Schatten-Effekte für erhöhte Tiefe der UI-Elemente

## 💡 Technische Besonderheiten

- **Event-gesteuerte Architektur**: Saubere Trennung von UI und Logik
- **Fokus-Management**: Intelligente Placeholder-Text-Verwaltung
- **Responsive Design**: Anpassungsfähiges Layout für verschiedene Fenstergrößen

---

Entwickelt mit ❤️ unter Verwendung moderner .NET-Technologien
