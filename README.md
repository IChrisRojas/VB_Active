# Stay Awake (VB_Active)

Una utilidad ligera para Windows que mantiene la PC activa simulando una pulsación de tecla invisible (F15) cada 4 minutos.

## Características

- **Sin Ventanas:** Se ejecuta exclusivamente en el área de notificación (System Tray).
- **Invisible:** Utiliza la tecla F15, que no interfiere con el trabajo diario ni con periféricos (como software de Logitech).
- **Control Total:** Activa o desactiva la funcionalidad con un doble clic o mediante el menú contextual.
- **Portable:** Ejecutable único (Single File) que no requiere instalación ni runtime de .NET previo.

## Uso

1. Ejecuta `StayAwake.exe`.
2. Verás un icono gris en el System Tray (Desactivado).
3. **Doble clic** en el icono o selecciona **Activar** en el menú contextual para iniciar. El icono cambiará a azul.
4. Para detenerlo, haz doble clic nuevamente o selecciona **Desactivar**.
5. Para cerrar la aplicación, selecciona **Salir**.

## Compilación (Build)

Para generar el ejecutable portable de un solo archivo:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true
```

El archivo generado se encontrará en: `bin\Release
et6.0-windows\win-x64\publish\StayAwake.exe`

## Detalles Técnicos

- **Framework:** .NET 6.0
- **UI:** NotifyIcon (WinForms)
- **Simulación:** P/Invoke `SendInput` para inyectar `VK_F15 (0x7E)`.
- **Intervalo:** 240,000ms (4 minutos).
