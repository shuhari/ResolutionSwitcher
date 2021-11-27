#include "pch.h"
#include "models.h"
#include <math.h>
#include <atlstr.h>
#include <SetupApi.h>
#include <cfgmgr32.h>


using namespace System::Collections::Generic;
using namespace ResolutionSwitcher;


static const DeviceScaleFactor DpiVals[] =
{
    DeviceScaleFactor::Scale100,
    DeviceScaleFactor::Scale125,
    DeviceScaleFactor::Scale150,
    DeviceScaleFactor::Scale175,
    DeviceScaleFactor::Scale200,
    DeviceScaleFactor::Scale225,
    DeviceScaleFactor::Scale250,
    DeviceScaleFactor::Scale300,
    DeviceScaleFactor::Scale350,
    DeviceScaleFactor::Scale400,
    DeviceScaleFactor::Scale450,
    DeviceScaleFactor::Scale500,
};

// Idea from:
// https://stackoverflow.com/questions/10073261/how-to-fetch-the-native-resolution-of-attached-monitor-from-edid-file-through-vb
// https://ofekshilon.com/2011/11/13/reading-monitor-physical-dimensions-or-getting-the-edid-the-right-way/
const GUID GUID_CLASS_MONITOR = { 0x4d36e96e, 0xe325, 0x11ce, 0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18 };


int GetDPIScalingIndex(DeviceScaleFactor value) {
    for (int i = 0; i < sizeof(DpiVals) / sizeof(DeviceScaleFactor); i++) {
        if (DpiVals[i] == value)
            return i;
    }
    return -1;
}


BOOL DisplayDeviceFromHMonitor(HMONITOR hMonitor, DISPLAY_DEVICE& ddMonOut);
CString Get2ndSlashBlock(const CString& sIn);
Size GetSizeForDevID(const CString& TargetDevID);
Size GetMonitorSizeFromEDID(const HKEY hEDIDRegKey);


DeviceScaleFactor DisplayApi::RecommendedDPIScaling::get() {
    int dpi = 0;
    auto retval = SystemParametersInfo(SPI_GETLOGICALDPIOVERRIDE, 0, (LPVOID)&dpi, 1);
    if (retval != 0 && abs(dpi) < sizeof(DpiVals) / sizeof(DeviceScaleFactor))
        return DpiVals[dpi * -1];
	return DeviceScaleFactor::Invalid;
}


void DisplayApi::RecommendedDPIScaling::set(DeviceScaleFactor value) {
    auto recommendedDpiScale = RecommendedDPIScaling::get();
    if (recommendedDpiScale != DeviceScaleFactor::Invalid) {
        int recIndex = GetDPIScalingIndex(recommendedDpiScale);
        int setIndex = GetDPIScalingIndex(value);
        if (recIndex >= 0 && setIndex >= 0) {
            int relativeIndex = setIndex - recIndex;
            SystemParametersInfo(SPI_SETLOGICALDPIOVERRIDE, relativeIndex, (LPVOID)0, 1);
        }
    }
}


array<Size>^ DisplayApi::GetResolutions() {
    auto list = gcnew List<Size>();
    
    DEVMODE devMode;
    memset(&devMode, 0, sizeof(DEVMODE));
    int modeNum = 0;
    while (EnumDisplaySettings(NULL, modeNum, &devMode)) {
        auto size = gcnew Size(devMode.dmPelsWidth, devMode.dmPelsHeight);
        if (!list->Contains(*size)) {
            list->Add(*size);
        }
        modeNum++;
    }

    return list->ToArray();
}


BOOL CALLBACK DisplayMonitorEnumProc(HMONITOR hMonitor, HDC hdcMonitor, LPRECT lprcMonitor, LPARAM dwData) {
    HMONITOR* ph = (HMONITOR*)dwData;
    *ph = hMonitor;
    return FALSE;
}

Size DisplayApi::GetRecommendedResolution() {
    HMONITOR  hMonitor;
    EnumDisplayMonitors(NULL, NULL, DisplayMonitorEnumProc, (LPARAM) &hMonitor);
    DISPLAY_DEVICE ddMon;
    if (!DisplayDeviceFromHMonitor(hMonitor, ddMon)) {
        throw gcnew Exception("Cannot get device from monitor");
    }

    CString DeviceID;
    DeviceID.Format(_T("%s"), ddMon.DeviceID);
    DeviceID = Get2ndSlashBlock(DeviceID);
    return GetSizeForDevID(DeviceID);
}


BOOL DisplayDeviceFromHMonitor(HMONITOR hMonitor, DISPLAY_DEVICE& ddMonOut)
{
    MONITORINFOEX mi;
    mi.cbSize = sizeof(MONITORINFOEX);
    GetMonitorInfo(hMonitor, &mi);

    DISPLAY_DEVICE dd;
    dd.cb = sizeof(dd);
    DWORD devIdx = 0; // device index

    // CString DeviceID;
    bool bFoundDevice = false;
    while (EnumDisplayDevices(0, devIdx, &dd, 0))
    {
        devIdx++;
        if (0 != _tcscmp(dd.DeviceName, mi.szDevice))
            continue;

        DISPLAY_DEVICE ddMon;
        ZeroMemory(&ddMon, sizeof(ddMon));
        ddMon.cb = sizeof(ddMon);
        DWORD MonIdx = 0;

        while (EnumDisplayDevices(dd.DeviceName, MonIdx, &ddMon, 0))
        {
            MonIdx++;

            ddMonOut = ddMon;
            return TRUE;

            ZeroMemory(&ddMon, sizeof(ddMon));
            ddMon.cb = sizeof(ddMon);
        }

        ZeroMemory(&dd, sizeof(dd));
        dd.cb = sizeof(dd);
    }

    return FALSE;
}


CString Get2ndSlashBlock(const CString& sIn)
{
    int FirstSlash = sIn.Find(_T('\\'));
    CString sOut = sIn.Right(sIn.GetLength() - FirstSlash - 1);
    FirstSlash = sOut.Find(_T('\\'));
    sOut = sOut.Left(FirstSlash);
    return sOut;
}


Size GetSizeForDevID(const CString& TargetDevID)
{
    HDEVINFO devInfo = SetupDiGetClassDevsEx(
        &GUID_CLASS_MONITOR, //class GUID
        NULL, //enumerator
        NULL, //HWND
        DIGCF_PRESENT | DIGCF_PROFILE, // Flags //DIGCF_ALLCLASSES|
        NULL, // device info, create a new one.
        NULL, // machine name, local machine
        NULL);// reserved

    if (NULL == devInfo)
        throw gcnew Exception("SetupDiGetClassDevsEx failed");

    bool bRes = false;
    Size result;

    for (ULONG i = 0; ERROR_NO_MORE_ITEMS != GetLastError(); ++i)
    {
        SP_DEVINFO_DATA devInfoData;
        memset(&devInfoData, 0, sizeof(devInfoData));
        devInfoData.cbSize = sizeof(devInfoData);

        if (SetupDiEnumDeviceInfo(devInfo, i, &devInfoData))
        {
            TCHAR Instance[MAX_DEVICE_ID_LEN];
            SetupDiGetDeviceInstanceId(devInfo, &devInfoData, Instance, MAX_PATH, NULL);

            CString sInstance(Instance);
            if (-1 == sInstance.Find(TargetDevID))
                continue;

            HKEY hEDIDRegKey = SetupDiOpenDevRegKey(devInfo, &devInfoData,
                DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_READ);

            if (!hEDIDRegKey || (hEDIDRegKey == INVALID_HANDLE_VALUE))
                continue;

            result = GetMonitorSizeFromEDID(hEDIDRegKey);
            bRes = TRUE;
            RegCloseKey(hEDIDRegKey);
        }
    }
    SetupDiDestroyDeviceInfoList(devInfo);
    if (!bRes)
        throw gcnew Exception("Display device not found");
    return result;
}


Size GetMonitorSizeFromEDID(const HKEY hEDIDRegKey)
{
    BYTE EDIDdata[1024];
    DWORD edidsize = sizeof(EDIDdata);

    if (ERROR_SUCCESS != RegQueryValueEx(hEDIDRegKey, _T("EDID"), NULL, NULL, EDIDdata, &edidsize))
        throw gcnew Exception("EDID data not found");
    // WidthMm = ((EDIDdata[68] & 0xF0) << 4) + EDIDdata[66];
    // HeightMm = ((EDIDdata[68] & 0x0F) << 8) + EDIDdata[67];

    const int dtd = 54; // # start byte of detailed timing desc.

    int horizontalRes = ((EDIDdata[dtd + 4] >> 4) << 8) | EDIDdata[dtd + 2];
    int verticalRes = ((EDIDdata[dtd + 7] >> 4) << 8) | EDIDdata[dtd + 5];
    return Size(horizontalRes, verticalRes);
}


DisplayMode^ DisplayApi::GetRecommendedMode() {
    Size resolution = GetRecommendedResolution();
    DisplayOrientation orientation = DisplayOrientation::Default;
    DeviceScaleFactor scale = RecommendedDPIScaling::get();
    return gcnew DisplayMode(resolution, orientation, scale);
}


DisplayMode^ DisplayApi::GetCurrentMode() {
    DEVMODE devMode;
    memset(&devMode, 0, sizeof(devMode));
    devMode.dmSize = sizeof(DEVMODE);
    EnumDisplaySettings(NULL, ENUM_CURRENT_SETTINGS, &devMode);
    Size resolution = Size(devMode.dmPelsWidth, devMode.dmPelsHeight);
    DisplayOrientation orientation = (DisplayOrientation)devMode.dmDisplayOrientation;

    HWND hwndDesktop = GetDesktopWindow();
    // HWND hwndDesktop = GetConsoleWindow();
    HDC hdc = GetDC(hwndDesktop);
    int dpi = GetDeviceCaps(hdc, LOGPIXELSX);
    ReleaseDC(hwndDesktop, hdc);
    DeviceScaleFactor scale = (DeviceScaleFactor)(dpi * 100 / 96);
    return gcnew DisplayMode(resolution, orientation, scale);
}


void DisplayApi::SetCurrentMode(DisplayMode^ mode) {
    DEVMODE devMode;
    memset(&devMode, 0, sizeof(DEVMODE));
    devMode.dmSize = sizeof(DEVMODE);
    devMode.dmFields = DM_PELSWIDTH | DM_PELSHEIGHT | DM_DISPLAYORIENTATION;
    devMode.dmPelsWidth = mode->Resolution.Width;
    devMode.dmPelsHeight = mode->Resolution.Height;
    devMode.dmDisplayOrientation = (DWORD)mode->Orientation;
    ChangeDisplaySettings(&devMode, 0);
    RecommendedDPIScaling::set(mode->Scale);
}
