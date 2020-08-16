@echo off

:: Copyright (c) Philipp Wagner. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

set JAVA_EXE="G:\Applications\Oracle\jdk-14.0.1\bin\java.exe"
set PHOTON_JAR="G:\Applications\Photon\photon-0.3.3.jar"
set DATA_DIR="G:\Data\Photon\photon-db-de-200809"


%JAVA_EXE% -jar %PHOTON_JAR% -data-dir %DATA_DIR% -listen-ip 0.0.0.0 -listen-port 2322 -cors-any

pause