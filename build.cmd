:: the windows shell, so amazing

:: options
@echo Off
cd %~dp0
setlocal

:: determine cache dir
set NUGET_CACHE_DIR=%LocalAppData%\.nuget\v3.5.0-rc1

:: download nuget to cache dir
set NUGET_URL=https://dist.nuget.org/win-x86-commandline/v3.5.0-rc1/NuGet.exe
if not exist %NUGET_CACHE_DIR%\NuGet.exe (
  if not exist %NUGET_CACHE_DIR% md %NUGET_CACHE_DIR%
  echo Downloading '%NUGET_URL%'' to '%NUGET_CACHE_DIR%\NuGet.exe'...
  @powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%NUGET_URL%' -OutFile '%NUGET_CACHE_DIR%\NuGet.exe'"
)

:: copy nuget locally
if not exist .nuget\NuGet.exe (
  if not exist .nuget md .nuget
  copy %NUGET_CACHE_DIR%\NuGet.exe .nuget\NuGet.exe > nul
)

:: restore packages
.nuget\NuGet.exe restore .\ConfigR.sln

:: run script
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Roslyn\csi.exe" .\build.csx %*
