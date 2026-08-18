#include "AppDelegate.h"
#include "MainScene.h"

USING_NS_AX;

static ax::Size designResolutionSize = ax::Size(720, 1280);

AppDelegate::AppDelegate() {}

AppDelegate::~AppDelegate() {}

void AppDelegate::initGLContextAttrs() {}

bool AppDelegate::applicationDidFinishLaunching() {
    auto director = Director::getInstance();
    auto renderView = director->getRenderView();
    if (!renderView) {
        renderView = RenderViewImpl::createWithRect("Axmol TsumTsumu", Rect(0, 0, designResolutionSize.width, designResolutionSize.height));
        director->setRenderView(renderView);
    }

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
