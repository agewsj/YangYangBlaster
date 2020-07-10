#import <Foundation/Foundation.h>

extern "C" {
    char *GetBundleVersion() {
        NSString *bundleVersion = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
        const char *s = [bundleVersion UTF8String];
        return strcpy((char *)malloc(strlen(s) + 1), s);
    }

    char *GetCountryCode()
    {
        NSLocale *locale = [NSLocale currentLocale];
        NSString *countryCode = [locale objectForKey: NSLocaleCountryCode];
        
        char* res = (char*)malloc(strlen([countryCode UTF8String]) + 1);
        return strcpy(res, [countryCode UTF8String]);
    }

    char *GetDeviceVersion()
    {
        NSString *systemVersion = [[UIDevice currentDevice] systemVersion];
        const char *s = [systemVersion UTF8String];
        return strcpy((char *)malloc(strlen(s) + 1), s);
    }
    
//Unity String으로 사용할 수 있도록 변환 시켜주는 함수
    char* MakeStringCopy (const char* string)
    {
        if (string == NULL)
            return NULL;
       
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        return res;
    }
}
