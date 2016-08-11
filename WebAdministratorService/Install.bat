%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\installutil.exe WebAdministratorService.exe
Net Start ServiceTest
sc config ServiceTest start=auto
