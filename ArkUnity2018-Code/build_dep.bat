@echo off

echo Building dependencies...
cd Assets

REM ######################################################################################################
echo Building GameFramework...

if exist GameFramework (rd GameFramework /q /s)
git clone https://github.com/EllanJiang/UnityGameFramework.git

echo Building GameFramework Success.
echo Building ArkGameClient Success.