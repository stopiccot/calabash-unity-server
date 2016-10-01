#import <UIKit/UIKit.h>

// Same as UnityDeviceModel() from Classes/Unity/DeviceSettings.mm but works for simulator too
extern "C" const char* CalabashDeviceModel()
{
    NSString* simulatorDeviceModel = [[[NSProcessInfo processInfo] environment] objectForKey:@"SIMULATOR_MODEL_IDENTIFIER"];
    if (simulatorDeviceModel != nil) {
        return AllocCString(simulatorDeviceModel);
    }
    
    return UnityDeviceModel();
}

// Completely the same as UnityDeviceGeneration() from Classes/Unity/DeviceSettings.mm but uses CalabashDeviceModel() instead of UnityDeviceModel()
// Implementation copied from Unity 5.4.1p1
extern "C" int CalabashDeviceGeneration()
{
    static int _DeviceGeneration = deviceUnknown;
    
    if (_DeviceGeneration == deviceUnknown)
    {
        const char* model = CalabashDeviceModel();
        
        if (!strcmp(model, "iPhone2,1"))
            _DeviceGeneration = deviceiPhone3GS;
        else if (!strncmp(model, "iPhone3,",8))
            _DeviceGeneration = deviceiPhone4;
        else if (!strncmp(model, "iPhone4,",8))
            _DeviceGeneration = deviceiPhone4S;
        else if (!strncmp(model, "iPhone5,",8))
        {
            int rev = atoi(model+8);
            if (rev >= 3) _DeviceGeneration = deviceiPhone5C; // iPhone5,3
            else		  _DeviceGeneration = deviceiPhone5;
        }
        else if (!strncmp(model, "iPhone6,",8))
            _DeviceGeneration = deviceiPhone5S;
        else if (!strncmp(model, "iPhone7,2",9))
            _DeviceGeneration = deviceiPhone6;
        else if (!strncmp(model, "iPhone7,1",9))
            _DeviceGeneration = deviceiPhone6Plus;
        else if (!strncmp(model, "iPhone8,1",9))
            _DeviceGeneration = deviceiPhone6S;
        else if (!strncmp(model, "iPhone8,2",9))
            _DeviceGeneration = deviceiPhone6SPlus;
        else if (!strncmp(model, "iPhone8,4",9))
            _DeviceGeneration = deviceiPhoneSE1Gen;
        else if (!strncmp(model, "iPhone9,1",9) || !strncmp(model, "iPhone9,3",9))
            _DeviceGeneration = deviceiPhone7;
        else if (!strncmp(model, "iPhone9,2",9) || !strncmp(model, "iPhone9,4",9))
            _DeviceGeneration = deviceiPhone7Plus;
        else if (!strcmp(model, "iPod4,1"))
            _DeviceGeneration = deviceiPodTouch4Gen;
        else if (!strncmp(model, "iPod5,",6))
            _DeviceGeneration = deviceiPodTouch5Gen;
        else if (!strncmp(model, "iPad2,", 6))
        {
            int rev = atoi(model+6);
            if(rev >= 5)	_DeviceGeneration = deviceiPadMini1Gen; // iPad2,5
            else			_DeviceGeneration = deviceiPad2Gen;
        }
        else if (!strncmp(model, "iPad3,", 6))
        {
            int rev = atoi(model+6);
            if(rev >= 4)	_DeviceGeneration = deviceiPad4Gen; // iPad3,4
            else			_DeviceGeneration = deviceiPad3Gen;
        }
        else if (!strncmp(model, "iPad4,", 6))
        {
            int rev = atoi(model+6);
            if (rev >= 7)
                _DeviceGeneration = deviceiPadMini3Gen;
            else if (rev >= 4)
                _DeviceGeneration = deviceiPadMini2Gen; // iPad4,4
            else
                _DeviceGeneration = deviceiPadAir1;
        }
        else if (!strncmp(model, "iPad5,", 6))
        {
            int rev = atoi(model+6);
            if (rev == 1 || rev == 2)
                _DeviceGeneration = deviceiPadMini4Gen;
            else if (rev >= 3)
                _DeviceGeneration = deviceiPadAir2;
        }
        else if (!strncmp(model, "iPad6,", 6))
        {
            int rev = atoi(model+6);
            if (rev == 7 || rev == 8)
                _DeviceGeneration = deviceiPadPro1Gen;
            else if (rev == 3 || rev == 4)
                _DeviceGeneration = deviceiPadPro10Inch1Gen;
        }
        
        // completely unknown hw - just determine form-factor
        if (_DeviceGeneration == deviceUnknown)
        {
            if (!strncmp(model, "iPhone",6))
                _DeviceGeneration = deviceiPhoneUnknown;
            else if (!strncmp(model, "iPad",4))
                _DeviceGeneration = deviceiPadUnknown;
            else if (!strncmp(model, "iPod",4))
                _DeviceGeneration = deviceiPodTouchUnknown;
            else
                _DeviceGeneration = deviceUnknown;
        }
    }
    return _DeviceGeneration;
}
