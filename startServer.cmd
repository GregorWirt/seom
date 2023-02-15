@echo off
rd /S /Q Source\.vs 2> nul
rd /S /Q Source\.vscode 2> nul
rd /S /Q Source\Seom.Application\bin 2> nul
rd /S /Q Source\Seom.Application\obj 2> nul
rd /S /Q Source\Seom.Webapp\bin 2> nul
rd /S /Q Source\Seom.Webapp\obj 2> nul

:start
dotnet watch run -c Debug --project Source\Seom.Webapp
GOTO start
