@echo off

echo Building dependencies...

REM ######################################################################################################
echo Building ArkClient_Core...

if exist ArkClient_Core (rd ArkClient_Core /q /s)
git clone https://github.com/ArkGame/ArkClient_Core.git

REM ######################################################################################################
echo Building ArkClient_Net...

if exist ArkClient_Net (rd ArkClient_Net /q /s)
git clone https://github.com/ArkGame/ArkClient_Net.git