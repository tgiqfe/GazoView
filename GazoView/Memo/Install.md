# GazoView

## インストールコマンド

※検証中

```dos
set execPath=C:\App\GazoView\GazoView.exe

reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Classes\GazoView\shell\open\command" /ve /d "\"%execPath%\" \"%1\"" /t REG_EXPAND_SZ /f

reg add "HKEY_LOCAL_MACHINE\SOFTWARE\RegisteredApplications" /v "GazoView" /d "Software\GazoView\Capabilities" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities" /v "ApplicationDescription" /d "Image Viewer" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities" /v "ApplicationName" /d "GazoView" /t REG_SZ /f

reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities\FileAssociations" /v ".bmp" /d "GazoView" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities\FileAssociations" /v ".jpg" /d "GazoView" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities\FileAssociations" /v ".jpeg" /d "GazoView" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities\FileAssociations" /v ".png" /d "GazoView" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities\FileAssociations" /v ".svg" /d "GazoView" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities\FileAssociations" /v ".tif" /d "GazoView" /t REG_SZ /f
reg add "HKEY_LOCAL_MACHINE\Software\GazoView\Capabilities\FileAssociations" /v ".tiff" /d "GazoView" /t REG_SZ /f
```


