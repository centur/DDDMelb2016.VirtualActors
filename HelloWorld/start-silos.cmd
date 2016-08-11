@ pushd %~dp0
@ call .\build.cmd
start .\ConsoleSilo\bin\Debug\ConsoleSilo.exe
start .\ConsoleSilo\bin\Debug\ConsoleSilo.exe 1
start .\ConsoleSilo\bin\Debug\ConsoleSilo.exe 2
@ popd