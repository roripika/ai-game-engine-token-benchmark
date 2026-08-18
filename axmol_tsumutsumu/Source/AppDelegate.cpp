#include "AppDelegate.h"
#include "MainScene.h"

USING_NS_AX;

AppDelegate::AppDelegate() {}

AppDelegate::~AppDelegate() {}

void AppDelegate::initGLContextAttrs() {
    GLContextAttrs glContextAttrs = {8, 8, 8, 8, 24, 8, 0};
    GLView::setGLContextAttrs(glContextAttrs);
}

bool AppDelegate::applicationDidFinishLaunching() {
    auto director = Director::getInstance();
    auto glview = director->getOpenGLView();
    if (!glview) {
        glview = GLViewImpl::createWithRect("Axmol TsumTsumu", Rect(0, 0, 720, 1280));
        director->setOpenGLView(glview);
    }

    director->setDisplayStats(false);
    director->setAnimationInterval(1.0f / 60);

    auto scene = MainScene::createScene();
    director->runWithScene(scene);

    // 1.5秒後にスクリーンショットを自動キャプチャして終了
    director->getScheduler()->schedule([](float) {
        utils::captureScreen([](bool succeed, std::string_view outputFile) {
            if (succeed) {
                AXLOG("Saved axmol_screenshot.png at %s", outputFile.data());
            }
            Director::getInstance()->end();
        }, "axmol_screenshot.png");
    }, scene, 1.5f, 0, 1.5f, false, "screenshot_key");

    return true;
}

void AppDelegate::applicationDidEnterBackground() {
    Director::getInstance()->stopAnimation();
}

void AppDelegate::applicationWillEnterForeground() {
    Director::getInstance()->startAnimation();
}
