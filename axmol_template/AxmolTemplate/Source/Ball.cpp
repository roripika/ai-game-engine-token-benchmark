#include "Ball.h"

USING_NS_AX;

Ball* Ball::createBall(BallType type) {
    auto ball = new Ball();
    if (ball && ball->initBall(type)) {
        ball->autorelease();
        return ball;
    }
    AX_SAFE_DELETE(ball);
    return nullptr;
}

bool Ball::initBall(BallType type) {
    if (!Sprite::init()) {
        return false;
    }

    _type = type;

    // 物理ボディ設定 (半径 35px)
    auto physicsBody = PhysicsBody::createCircle(35.0f, PhysicsMaterial(0.1f, 0.5f, 0.3f));
    setPhysicsBody(physicsBody);

    _drawNode = DrawNode::create();
    this->addChild(_drawNode);

    updateVisuals();
    return true;
}

void Ball::setHighlight(bool highlight) {
    _isConnected = highlight;
    updateVisuals();
}

void Ball::updateVisuals() {
    if (!_drawNode) return;
    _drawNode->clear();

    Color4F baseColor;
    switch (_type) {
        case BallType::RED:
            baseColor = Color4F(0.95f, 0.25f, 0.25f, 1.0f);
            break;
        case BallType::BLUE:
            baseColor = Color4F(0.25f, 0.55f, 0.95f, 1.0f);
            break;
        case BallType::YELLOW:
            baseColor = Color4F(0.95f, 0.85f, 0.25f, 1.0f);
            break;
    }

    // 円描画
    _drawNode->drawSolidCircle(Vec2::ZERO, 35.0f, 0.0f, 32, baseColor);
    _drawNode->drawCircle(Vec2::ZERO, 35.0f, 0.0f, 32, false, Color4F::GRAY, 2.0f);

    if (_isConnected) {
        _drawNode->drawCircle(Vec2::ZERO, 38.0f, 0.0f, 32, false, Color4F::WHITE, 4.0f);
    }
}
