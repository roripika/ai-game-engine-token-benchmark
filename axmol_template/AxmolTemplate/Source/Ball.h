#ifndef __BALL_H__
#define __BALL_H__

#include "axmol.h"

enum class BallType {
    RED = 0,
    BLUE = 1,
    YELLOW = 2
};

class Ball : public ax::Sprite {
public:
    static Ball* createBall(BallType type);
    bool initBall(BallType type);

    BallType getType() const { return _type; }
    void setHighlight(bool highlight);
    bool isConnected() const { return _isConnected; }

private:
    BallType _type;
    bool _isConnected = false;
    ax::DrawNode* _drawNode = nullptr;

    void updateVisuals();
};

#endif // __BALL_H__
