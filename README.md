# 📅 Meine Erinnerungs-App

Eine Desktop-Anwendung zur Verwaltung von Terminen und Erinnerungen, entwickelt mit WPF und C#.

## 🎯 Über das Projekt

Diese Anwendung ermöglicht es Benutzern, wichtige Termine schnell und einfach zu erfassen. Die intuitive Benutzeroberfläche mit modernem Gradient-Design bietet eine angenehme Nutzererfahrung.

## ✨ Funktionen

- **📝 Termin-Erfassung**: Eingabe von Grund, Datum und Uhrzeit für Termine
- **⏱️ Intelligente Erinnerungen**: Automatische Benachrichtigungen zu verschiedenen Zeitpunkten vor dem Termin
- **🎨 Dynamische Farbcodierung**: Visuelle Statusanzeige durch farbliche Hinterlegung der Termine
  - 🟢 Grün: Mehr als 30 Minuten bis zum Termin
  - 🟡 Gelb: 30 Minuten oder weniger bis zum Termin (mit Benachrichtigungen bei 30, 15 und 5 Minuten)
  - 🔴 Rot: Termin ist fällig oder überschritten
- **🔔 Mehrfache Warnungen**: Popup-Benachrichtigungen bei 30, 15 und 5 Minuten vor dem Termin
- **📊 Terminübersicht**: Übersichtliche Darstellung aller gespeicherten Termine in einer ListBox
- **💡 Platzhalter-Text**: Intelligente Textfelder mit automatischen Platzhaltern

## 🛠️ Technologie-Stack

- **Framework**: .NET Framework 4.8
- **UI-Technologie**: Windows Presentation Foundation (WPF)
- **Programmiersprache**: C#
- **IDE**: Visual Studio

## 📋 Voraussetzungen

- Windows Betriebssystem
- .NET Framework 4.8 oder höher
- Visual Studio 2017 oder höher (für Entwicklung)

## 🚀 Installation

1. Laden Sie das Repository herunter oder klonen Sie es
2. Öffnen Sie die Lösung `Meine_Erinnerungs_app.sln` in Visual Studio
3. Erstellen Sie das Projekt über Build → Build Solution
4. Starten Sie die Anwendung über Debug → Start Debugging (F5)

## 💻 Verwendung

1. Starten Sie die Anwendung
2. Geben Sie den Grund für den Termin im ersten Textfeld ein
3. Wählen Sie ein Datum aus dem Kalender oder klicken Sie auf "Heute"
4. Stellen Sie die gewünschte Uhrzeit im Time Picker ein
5. Klicken Sie auf "TERMIN BESTÄTIGEN!", um den Termin zu speichern
6. Die Anwendung zeigt Ihnen alle Termine in der Liste an und benachrichtigt Sie automatisch zu den konfigurierten Zeitpunkten

**Terminverwaltung:**
- Wählen Sie einen Termin aus der Liste aus, um ihn zu markieren
- Klicken Sie auf "Termin entfernen", um einen ausgewählten Termin zu löschen

## 📱 Benutzeroberfläche

Die Anwendung verfügt über eine moderne Benutzeroberfläche mit:
- Farbverlauf-Hintergrund für ein professionelles Erscheinungsbild
- Zentrierte Eingabefelder für optimale Benutzerfreundlichkeit
- Schatteneffekte für visuelle Tiefe
- Interaktiver Time Picker für präzise Zeitauswahl
- Dynamische Animationen bei Benutzerinteraktionen
- Farbcodierte Terminliste für schnelle Statuserkennung

## 🎓 Technische Highlights

Dieses Projekt demonstriert Kenntnisse in:
- **WPF-Entwicklung**: Erstellung moderner Windows-Desktop-Anwendungen mit XAML
- **Asynchrone Programmierung**: Timer-basierte Event-Verarbeitung für Echtzeit-Benachrichtigungen
- **Data Binding**: Verwendung von ObservableCollection für reaktive UI-Updates
- **UI/UX-Design**: Implementierung ansprechender visueller Effekte und Animationen
- **Event-Handling**: Verwaltung komplexer Benutzerinteraktionen
- **C#-Programmierung**: Objektorientierte Programmierung im .NET Framework
