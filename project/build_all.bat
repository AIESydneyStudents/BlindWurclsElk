@echo off

REM : Default install directory for unity via UnityHub
IF exist "C:\Program Files\Unity\Hub\Editor\2019.3.6f1\Editor\Unity.exe" (
	echo "Unity Found, Please wait while we attempt to build, this may take a while"
	"C:\Program Files\Unity\Hub\Editor\2019.3.6f1\Editor\Unity.exe" -batchmode -quit -projectPath "%cd%" -executeMethod Builder.Build	
	echo "Build Complete"
	goto :endofscript
)

REM : Default install directory for unity on school pc's
IF exist "C:\Program Files\Unity 2019.3.6f1\Editor\Unity.exe" (
	echo "Unity Found, Please wait while we attempt to build, this may take a while"
	"C:\Program Files\Unity 2019.3.6f1\Editor\Unity.exe" -batchmode -quit -projectPath "%cd%" -executeMethod Builder.Build
	echo "Build Complete"
	goto :endofscript
)

REM : Unity has been installed in a different location, or it does not exist
echo "Unity Not found, please update this build script"


:endofscript
pause