@echo off

echo Building dependencies...
cd Assets
cd ArkGameFramework

REM ######################################################################################################
echo Building GameFramework...

if exist GameFramework (rd GameFramework /q /s)
git clone https://github.com/EllanJiang/GameFramework.git

echo Building GameFramework Success.
