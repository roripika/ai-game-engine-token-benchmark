#include "MainScene.h"

USING_NS_AX;

const float MAX_CONNECT_DIST = 120.0f;
const int INITIAL_BALL_COUNT = 30;

Scene* MainScene::createScene() {
    return MainScene::create();
}

bool MainScene::init() {
    if (!Scene::initWithPhysics()) {
        return false;
    }

    getPhysicsWorld()->setGravity(Vec2(0, -980.0f));

    auto visibleSize = _director->getVisibleSize();

    // ラインノード
    _lineDrawNode = DrawNode::create();
    this->addChild(_lineDrawNode, 10);

    // スコアUI
    _scoreLabel = Label::createWithSystemFont("SCORE: 0", "Arial", 36);
    _scoreLabel->setPosition(Vec2(150.0f, visibleSize.height - 50.0f));
    this->addChild(_scoreLabel, 20);

    createWalls();
    spawnInitialBalls();

    // タッチイベント
    auto touchListener = EventListenerTouchOneByOne::create();
    touchListener->onTouchBegan = AX_CALLBACK_2(MainScene::onTouchBegan, this);
    touchListener->onTouchMoved = AX_CALLBACK_2(MainScene::onTouchMoved, this);
    touchListener->onTouchEnded = AX_CALLBACK_2(MainScene::onTouchEnded, this);
    _eventDispatcher->addEventListenerWithSceneGraphPriority(touchListener, this);

    return true;
}

void MainScene::createWalls() {
    auto visibleSize = _director->getVisibleSize();
    auto wallBody = PhysicsBody::createEdgeBox(visibleSize, PHYSICSBODY_MATERIAL_DEFAULT, 50.0f);
    auto wallNode = Node::create();
    wallNode->setPosition(Vec2(visibleSize.width / 2.0f, visibleSize.height / 2.0f));
    wallNode->setPhysicsBody(wallBody);
    this->addChild(wallNode);
}

void MainScene::spawnInitialBalls() {
    auto visibleSize = _director->getVisibleSize();
    for (int i = 0; i < INITIAL_BALL_COUNT; ++i) {
        float x = random(100.0f, visibleSize.width - 100.0f);
        float y = random(visibleSize.height - 400.0f, visibleSize.height - 100.0f);
        spawnSingleBall(Vec2(x, y));
    }
}

Ball* MainScene::spawnSingleBall(const Vec2& pos) {
    BallType type = static_cast<BallType>(random(0, 2));
    auto ball = Ball::createBall(type);
    ball->setPosition(pos);
    this->addChild(ball);
    return ball;
}

bool MainScene::onTouchBegan(Touch* touch, Event* event) {
    Vec2 touchPos = touch->getLocation();
    Ball* ball = getBallAtPosition(touchPos);
    if (ball) {
        _isDragging = true;
        _connectedBalls.clear();
        _currentType = ball->getType();
        connectBall(ball);
        return true;
    }
    return false;
}

void MainScene::onTouchMoved(Touch* touch, Event* event) {
    if (!_isDragging) return;
    Vec2 touchPos = touch->getLocation();
    Ball* ball = getBallAtPosition(touchPos);
    if (ball && ball->getType() == _currentType && !ball->isConnected()) {
        Ball* lastBall = _connectedBalls.back();
        if (lastBall->getPosition().distance(ball->getPosition()) <= MAX_CONNECT_DIST) {
            connectBall(ball);
        }
    }
    updateLine(touchPos);
}

void MainScene::onTouchEnded(Touch* touch, Event* event) {
    if (!_isDragging) return;
    _isDragging = false;
    _lineDrawNode->clear();

    int count = static_cast<int>(_connectedBalls.size());
    if (count >= 3) {
        auto visibleSize = _director->getVisibleSize();
        for (auto ball : _connectedBalls) {
            ball->removeFromParent();
        }
        _score += count * 100;
        updateScoreUI();

        // 補充
        for (int i = 0; i < count; ++i) {
            float x = random(100.0f, visibleSize.width - 100.0f);
            spawnSingleBall(Vec2(x, visibleSize.height - 100.0f));
        }
    } else {
        for (auto ball : _connectedBalls) {
            ball->setHighlight(false);
        }
    }
    _connectedBalls.clear();
}

Ball* MainScene::getBallAtPosition(const Vec2& touchPos) {
    auto physicsWorld = getPhysicsWorld();
    auto shapes = physicsWorld->getShapes(touchPos);
    for (auto shape : shapes) {
        auto body = shape->getBody();
        if (body) {
            auto node = body->getOwner();
            auto ball = dynamic_cast<Ball*>(node);
            if (ball) {
                return ball;
            }
        }
    }
    return nullptr;
}

void MainScene::connectBall(Ball* ball) {
    _connectedBalls.push_back(ball);
    ball->setHighlight(true);
}

void MainScene::updateLine(const Vec2& currentTouchPos) {
    _lineDrawNode->clear();
    if (_connectedBalls.empty()) return;

    for (size_t i = 0; i < _connectedBalls.size() - 1; ++i) {
        _lineDrawNode->drawLine(_connectedBalls[i]->getPosition(), _connectedBalls[i+1]->getPosition(), Color4F::WHITE);
    }
    if (_isDragging) {
        _lineDrawNode->drawLine(_connectedBalls.back()->getPosition(), currentTouchPos, Color4F::WHITE);
    }
}

void MainScene::updateScoreUI() {
    if (_scoreLabel) {
        _scoreLabel->setString(StringUtils::format("SCORE: %d", _score));
    }
}
