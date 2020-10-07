# SilentClean UAC bypass via binary planting

This project implements a DLL planting technique to bypass UAC Always Notify and execute code in a high integrity process. 

When SilentCleanup task is launched, `dismhost` searches for the non existing DLL `api-ms-win-core-kernel32-legacy-l1.dll` under: 

```
C:\Users\USER\Appdata\Local\Microsoft\WindowsApps
```

The above path exists by default in the PATH of the user.

By crafting a malicious DLL and placing it in the above directory, it will be loaded by `dismhost.exe` and executed with High Integrity privileges.

## Implementation

The project consists of:

* **SilentClean .NET project** - Launching SilentClean scheduled task with the use of the TaskScheduler library

* **DLLmain_template.c** - A DLL skeleton which will spawn a process and inject the shellcode of our choice. Sample provided implements a simple CreateRemoteThread injector.

* **Cobalt strike aggressor script responsible for**:
	* Generating the shellcode byte array
	* Replacing dllmain_template.c with the above shellcode
	* Compile the dll with mingw
	* Upload the dll to the required path
	* Execute .NET binary SilentClean.exe through Execute-Assembly to launch the scheduled task

## Configuration

* Feel free to replace injection method in RunMe function of `dllmain_template.c`. **This is just a POC** 
* Current spawned process to inject to is `cmd.exe`. 
* No shellcode encryption / compression has been baked in. As such the DLL generated will probably be **flagged by an AV**
* x86_64-w64-mingw32 and headers are required to be installed on the building system
* Compile SilentClean .NET project and place executable in the same folder as the CNA script


## Versions tested

* Microsoft Windows 10 - 1909 18363.1110
* Microsoft Windows 10 - 1909 18363.1082
* Microsoft Windows 10 - 1809 17763.1457

## Author

* [@leftp](https://github.com/leftp)
* [@cirrusj](https://github.com/cirrusj)