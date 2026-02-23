# Design Doc: VPN Manager Integration

## Overview
Adds institutional VPN management capabilities to the Stay Awake utility. It allows connecting/disconnecting to a pre-configured Windows VPN profile in the background, with support for automatic disconnection after a timer.

## Technical Specifications
- **Connection Method:** `rasdial.exe` (Background process)
- **Credential Storage:** Local JSON file (`vpn_config.json`) encrypted using `ProtectedData` (DPAPI) for user-level security.
- **Notifications:** Windows Balloon Tips via `NotifyIcon`.
- **Timer:** `System.Windows.Forms.Timer` for the 30-minute auto-disconnect feature.

## Components
- `VpnManager`: Static class to handle `rasdial` execution and status checks.
- `ConfigService`: Manages encryption/decryption and persistence of `vpn_config.json`.
- `VpnConfigForm`: A simple dialog to input Profile Name, Username, and Password.
- `TrayApplicationContext (Update)`: New menu items and event handlers for VPN logic.

## User Interface
- **New Menu Items:**
  - VPN: Conectar (Background)
  - VPN: Conectar por 30 min (Timed)
  - VPN: Desconectar
  - Configurar VPN (Opens VpnConfigForm)
- **Configuration Form:**
  - TextBoxes for Profile, User, and Password (masked).
  - Save/Cancel buttons.

## Security
- Passwords will **never** be stored in plain text.
- `System.Security.Cryptography.ProtectedData` ensures only the current Windows user can decrypt the config file on the same machine.
- The `vpn_config.json` will be added to `.gitignore`.

## Error Handling
- Notify user if `rasdial` fails (e.g., wrong password or profile name).
- Notify user if the config file is missing or corrupted.
