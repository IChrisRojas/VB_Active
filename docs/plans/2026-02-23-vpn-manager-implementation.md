# VPN Manager Implementation Plan

**Goal:** Implement background VPN management with encrypted local storage and a configuration UI.

---

### Task 1: Security and Configuration Service
**Files:**
- Create: `ConfigService.cs`
- Modify: `.gitignore`

**Steps:**
1. Add `System.Security.Cryptography.Pkcs` (if needed) or use `System.Security.Cryptography.ProtectedData` (requires `System.Security.Cryptography.ProtectedData` NuGet package).
2. Implement `ConfigService` with `SaveConfig` and `LoadConfig` using DPAPI encryption.
3. Update `.gitignore` to exclude `vpn_config.json`.

---

### Task 2: VPN Logic (VpnManager)
**Files:**
- Create: `VpnManager.cs`

**Steps:**
1. Implement `Connect(profile, user, pass)` using `Process.Start("rasdial", ...)` with hidden window.
2. Implement `Disconnect(profile)` using `Process.Start("rasdial", profile + " /disconnect")`.
3. Add success/failure detection based on process exit codes.

---

### Task 3: Configuration UI (VpnConfigForm)
**Files:**
- Create: `VpnConfigForm.cs`
- Create: `VpnConfigForm.Designer.cs`

**Steps:**
1. Create a simple WinForm with fields for Profile Name, Username, and Password.
2. Implement the "Save" logic to persist data via `ConfigService`.

---

### Task 4: Update TrayApplicationContext
**Files:**
- Modify: `TrayApplicationContext.cs`

**Steps:**
1. Add menu items: "VPN: Conectar", "VPN: Conectar por 30 min", "VPN: Desconectar", "Configurar VPN".
2. Implement handlers for these items.
3. Add a `System.Windows.Forms.Timer` for the 30-minute disconnection logic.
4. Integrate `NotifyIcon.ShowBalloonTip` for status updates.

---

### Task 5: Testing and Validation
**Steps:**
1. Test saving and loading encrypted credentials.
2. Test connecting to the VPN in the background.
3. Verify the 30-minute timer auto-disconnects.
4. Confirm no plain-text passwords exist in the local file.
