extends Node2D

const BallScript = preload("res://ball.gd")
@export var ball_scene: PackedScene = preload("res://ball.tscn")

const MAX_CONNECT_DISTANCE: float = 120.0
const INITIAL_BALL_COUNT: int = 30
const SPAWN_Y: float = 150.0

var connected_balls: Array = []
var current_type: int = -1
var is_dragging: bool = false
var score: int = 0

@onready var line_2d: Line2D = $Line2D
@onready var score_label: Label = $CanvasLayer/ScoreLabel

func _ready() -> void:
	_create_walls()
	_spawn_initial_balls()
	_update_score_ui()
	get_tree().create_timer(1.5).timeout.connect(_take_screenshot)

func _take_screenshot() -> void:
	var img = get_viewport().get_texture().get_image()
	img.save_png("godot_screenshot.png")
	print("Saved godot_screenshot.png")
	get_tree().quit()


func _create_walls() -> void:
	var viewport_size = get_viewport_rect().size
	var wall_thickness = 50.0
	
	# 下壁
	_add_wall_body(Rect2(0, viewport_size.y - wall_thickness, viewport_size.x, wall_thickness))
	# 左壁
	_add_wall_body(Rect2(-wall_thickness, 0, wall_thickness, viewport_size.y))
	# 右壁
	_add_wall_body(Rect2(viewport_size.x, 0, wall_thickness, viewport_size.y))

func _add_wall_body(rect: Rect2) -> void:
	var static_body = StaticBody2D.new()
	var col = CollisionShape2D.new()
	var shape = RectangleShape2D.new()
	shape.size = rect.size
	col.shape = shape
	col.position = rect.position + rect.size / 2.0
	static_body.add_child(col)
	add_child(static_body)

func _spawn_initial_balls() -> void:
	var viewport_size = get_viewport_rect().size
	for i in range(INITIAL_BALL_COUNT):
		var pos = Vector2(randf_range(100, viewport_size.x - 100), randf_range(100, SPAWN_Y + 200))
		_spawn_single_ball(pos)

func _spawn_single_ball(pos: Vector2) -> Node2D:
	var ball = ball_scene.instantiate() as Node2D
	ball.position = pos
	var random_type = randi() % 3
	ball.setup(random_type)
	add_child(ball)
	return ball

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed:
				_start_drag(event.position)
			else:
				_end_drag()
	elif event is InputEventMouseMotion and is_dragging:
		_process_drag(event.position)

func _start_drag(mouse_pos: Vector2) -> void:
	var ball = _get_ball_at_pos(mouse_pos)
	if ball:
		is_dragging = true
		connected_balls.clear()
		current_type = ball.ball_type
		_connect_ball(ball)

func _process_drag(mouse_pos: Vector2) -> void:
	var ball = _get_ball_at_pos(mouse_pos)
	if ball and ball.ball_type == current_type and not ball in connected_balls:
		var last_ball = connected_balls.back()
		if last_ball and last_ball.global_position.distance_to(ball.global_position) <= MAX_CONNECT_DISTANCE:
			_connect_ball(ball)
	
	_update_line(mouse_pos)

func _connect_ball(ball: Node2D) -> void:
	connected_balls.append(ball)
	ball.set_highlight(true)

func _update_line(current_mouse_pos: Vector2) -> void:
	line_2d.clear_points()
	for b in connected_balls:
		line_2d.add_point(b.global_position)
	if is_dragging and not connected_balls.is_empty():
		line_2d.add_point(current_mouse_pos)

func _end_drag() -> void:
	if not is_dragging:
		return
	
	is_dragging = false
	line_2d.clear_points()
	
	var count = connected_balls.size()
	if count >= 3:
		# 消去
		for b in connected_balls:
			b.queue_free()
		score += count * 100
		_update_score_ui()
		
		# 補充
		var viewport_size = get_viewport_rect().size
		for i in range(count):
			var spawn_pos = Vector2(randf_range(100, viewport_size.x - 100), SPAWN_Y)
			_spawn_single_ball(spawn_pos)
	else:
		# ハイライト解除
		for b in connected_balls:
			b.set_highlight(false)
			
	connected_balls.clear()

func _get_ball_at_pos(pos: Vector2) -> Node2D:
	var space_state = get_world_2d().direct_space_state
	var query = PhysicsPointQueryParameters2D.new()
	query.position = pos
	query.collide_with_bodies = true
	var result = space_state.intersect_point(query)
	for res in result:
		if res.collider.get_script() == BallScript:
			return res.collider as Node2D
	return null

func _update_score_ui() -> void:
	if score_label:
		score_label.text = "SCORE: %d" % score
