#ifndef __APP_DELEGATE_H__
#define __APP_DELEGATE_H__

#include "axmol.h"

class AppDelegate : private ax::Application {
public:
    AppDelegate();
    virtual ~AppDelegate();

    virtual void initGLContextAttrs() override;
    virtual bool applicationDidFinishLaunching() override;
    virtual void applicationDidEnterBackground() override;
    virtual void applicationWillEnterForeground() override;
};

#endif // __APP_DELEGATE_H__
