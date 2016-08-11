@ECHO OFF
pushd %~dp0
if "%1"=="" (
	SET CONFIGURATION=Debug
) else (
	SET CONFIGURATION=%1
)

SET VSMSBUILDCMD="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsMSBuildCmd.bat"
@call %VSMSBUILDCMD%
for %%f in (*.sln) do (
	echo msbuild /p:Configuration=%CONFIGURATION% /m:8 /v:minimal /nologo %%f
	msbuild /p:Configuration=%CONFIGURATION% /m:8 /v:minimal /nologo %%f
)