# Design Doc: Stay Awake (WinForms Tray Utility)

## Overview
A lightweight C# WinForms application that lives exclusively in the System Tray. Its purpose is to prevent the PC from going to sleep or showing as "Away" by simulating a F15 key press every 4 minutes.

## Technical Specifications
- **Framework:** .NET 6.0 (WinForms)
- **Architecture:** Formless `ApplicationContext`
- **Activity Simulation:** P/Invoke `SendInput` (Key: F15)
- **UI:** `NotifyIcon` with `ContextMenuStrip`
- **Initial State:** Disabled

## Components
- `TrayApplicationContext`: Manages application lifecycle and UI components.
- `ActivityTimer`: `System.Windows.Forms.Timer` set to 240,000ms.
- `NativeMethods`: Static class for `user32.dll` P/Invoke declarations.

## User Interface
- **Context Menu:**
  - Activar (Enabled state)
  - Desactivar (Disabled state)
  - Salir (Exits application)
- **Interactions:**
  - Double-click: Toggle between Enabled/Disabled.
  - Hover: Shows current status.

## Deployment
- **Target:** Single File Executable (Portable)
- **Runtime:** Self-contained (No external .NET installation required)
