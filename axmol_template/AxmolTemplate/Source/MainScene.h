#ifndef __MAIN_SCENE_H__
#define __MAIN_SCENE_H__

#include "axmol.h"
#include "Ball.h"
#include <vector>

class MainScene : public ax::Scene {
public:
    static ax::Scene* createScene();
    virtual bool init() override;

    CREATE_FUNC(MainScene);

private:
    void createWalls();
    void spawnInitialBalls();
    Ball* spawnSingleBall(const ax::Vec2& pos);

    bool onTouchBegan(ax::Touch* touch, ax::Event* event);
    void onTouchMoved(ax::Touch* touch, ax::Event* event);
    void onTouchEnded(ax::Touch* touch, ax::Event* event);

    Ball* getBallAtPosition(const ax::Vec2& touchPos);
    void connectBall(Ball* ball);
    void updateLine(const ax::Vec2& currentTouchPos);
    void updateScoreUI();

    std::vector<Ball*> _connectedBalls;
    BallType _currentType;
    bool _isDragging = false;
    int _score = 0;

    ax::DrawNode* _lineDrawNode = nullptr;
    ax::Label* _scoreLabel = nullptr;
};

#endif // __MAIN_SCENE_H__
