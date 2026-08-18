extends RigidBody2D
class_name Ball

enum BallType { RED, BLUE, YELLOW }

@export var ball_type: BallType = BallType.RED
var is_connected: bool = false

# 色定義
const COLORS = {
	BallType.RED: Color(0.95, 0.25, 0.25),
	BallType.BLUE: Color(0.25, 0.55, 0.95),
	BallType.YELLOW: Color(0.95, 0.85, 0.25)
}

func _ready() -> void:
	input_pickable = true
	queue_redraw()

func setup(type: BallType) -> void:
	ball_type = type
	queue_redraw()

func set_highlight(highlight: bool) -> void:
	is_connected = highlight
	queue_redraw()

func _draw() -> void:
	var base_color = COLORS.get(ball_type, Color.WHITE)
	# 円描画 (半径 35px)
	draw_circle(Vector2.ZERO, 35.0, base_color)
	draw_arc(Vector2.ZERO, 35.0, 0, TAU, 32, Color.DARK_GRAY, 2.0)
	
	if is_connected:
		# ハイライト枠線
		draw_arc(Vector2.ZERO, 38.0, 0, TAU, 32, Color.WHITE, 4.0)
