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

    // ボールが物理落下して蓄積する 2.5秒後に、純粋なゲーム画面のみを captureScreen で直接保存
    director->getScheduler()->schedule([](float) {
        utils::captureScreen([](bool succeed, std::string_view outputFile) {
            if (succeed) {
                AXLOG("Saved clean axmol_gameplay.png: %s", outputFile.data());
            }
            Director::getInstance()->end();
        }, "axmol_gameplay.png");
    }, scene, 2.5f, 0, 2.5f, false, "screenshot_key");

    return true;
}

void AppDelegate::applicationDidEnterBackground() {
    Director::getInstance()->stopAnimation();
}

void AppDelegate::applicationWillEnterForeground() {
    Director::getInstance()->startAnimation();
}
