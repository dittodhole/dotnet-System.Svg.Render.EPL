@ECHO OFF
powershell -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "Invoke-WebRequest https://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1"
powershell -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "%~dpn0.ps1 %*"
