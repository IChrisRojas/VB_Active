# Stay Awake Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Create a C# WinForms System Tray application that simulates F15 key presses to keep the PC active.

**Architecture:** Formless `ApplicationContext` with a `NotifyIcon` and a `System.Windows.Forms.Timer`. Uses P/Invoke `SendInput` for hardware-level key simulation.

**Tech Stack:** .NET 6.0, WinForms, P/Invoke (user32.dll).

---

### Task 1: Initialize Repository and Project

**Files:**
- Create: `StayAwake.csproj`
- Create: `.gitignore`

**Step 1: Create GitHub Repository**
Run: `gh repo create VB_Active --public --description "Utility to keep PC active using F15 simulation" --confirm` (Simulated via tool)

**Step 2: Create .NET project**
Run: `dotnet new winforms -n StayAwake -o .`

**Step 3: Clean up default files**
Run: `rm Form1.cs, Form1.Designer.cs`

**Step 4: Commit**
```bash
git init
git add .
git commit -m "chore: initial project structure"
```

---

### Task 2: Implement Native Methods (P/Invoke)

**Files:**
- Create: `NativeMethods.cs`

**Step 1: Implement NativeMethods class**
Write the `INPUT` structure and `SendInput` declaration for F15.

**Step 2: Commit**
```bash
git add NativeMethods.cs
git commit -m "feat: add P/Invoke for key simulation"
```

---

### Task 3: Implement TrayApplicationContext

**Files:**
- Create: `TrayApplicationContext.cs`

**Step 1: Implement TrayApplicationContext**
- Setup `NotifyIcon`.
- Setup `ContextMenuStrip` (Activar, Desactivar, Salir).
- Setup `Timer` (4 minutes).
- Implement `ToggleStatus` logic.

**Step 2: Commit**
```bash
git add TrayApplicationContext.cs
git commit -m "feat: implement tray logic and timer"
```

---

### Task 4: Main Entry Point

**Files:**
- Modify: `Program.cs`

**Step 1: Update Program.cs**
Replace default code with `Application.Run(new TrayApplicationContext());`.

**Step 2: Commit**
```bash
git add Program.cs
git commit -m "feat: update entry point to use TrayApplicationContext"
```

---

### Task 5: Documentation and Assets

**Files:**
- Create: `README.md`

**Step 1: Create README.md**
Write instructions and technical details.

**Step 2: Commit**
```bash
git add README.md
git commit -m "docs: add project documentation"
```

---

### Task 6: Final Build and Push

**Step 1: Build Single File Executable**
Run: `dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true`

**Step 2: Push to GitHub**
Run: `git push -u origin main`
