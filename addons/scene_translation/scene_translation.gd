extends CanvasLayer

signal before_scene_changed(old_scene: Node, new_scene: Node)
signal after_scene_changed(old_scene: Node, new_scene: Node)

const min_thread_load_time: float = 2
const fade_duration: float = 0.3

@onready var fade_color_rect: ColorRect = $FadeColorRect
@onready var loading_color_rect: ColorRect = $LoadingColorRect
@onready var loading_progress_bar: ProgressBar = $LoadingColorRect/VBoxContainer/LoadingProgressBar

var thread_load_scene_file: String
var thread_load_time: float
var is_thread_load_pause: bool

func _ready() -> void:
	hide_loading_page()

func hide_loading_page():
	loading_color_rect.hide()
	thread_load_time = 0
	is_thread_load_pause = false
	thread_load_scene_file = ""
	set_process(false)

func fade_to_black():
	var tween := create_tween()
	tween.set_pause_mode(Tween.TWEEN_PAUSE_PROCESS)
	tween.tween_property(fade_color_rect, "color:a", 1.0, fade_duration)
	await tween.finished

func fade_from_black():
	var tween := create_tween()
	tween.set_pause_mode(Tween.TWEEN_PAUSE_PROCESS)
	tween.tween_property(fade_color_rect, "color:a", 0.0, fade_duration)
	await tween.finished

func change_scene_to_packed(packed_scene: PackedScene):
	if not packed_scene:
		return
	
	await fade_to_black()
	# hide the loading progress page
	hide_loading_page()
	
	var old_scene := get_tree().current_scene
	var new_scene := packed_scene.instantiate() as Node
	var root := get_tree().root
	
	before_scene_changed.emit(old_scene, new_scene)
	
	root.remove_child(old_scene)
	old_scene.queue_free()
	
	root.add_child(new_scene)
	get_tree().current_scene = new_scene
	
	after_scene_changed.emit(old_scene, new_scene)
	
	await fade_from_black()

func change_scene_to_packed_with_pause(packed_scene: PackedScene):
	get_tree().paused = true
	await change_scene_to_packed(packed_scene)
	get_tree().paused = false

func change_scene_to_file(scene_file: String):
	change_scene_to_packed(load(scene_file))

func change_scene_to_file_with_pause(scene_file: String):
	get_tree().paused = true
	await change_scene_to_file(scene_file)
	get_tree().paused = false

func change_scene_to_file_async(scene_file: String):
	is_thread_load_pause = false
	_internal_change_scene_to_file_async(scene_file)

func change_scene_to_file_with_pause_async(scene_file: String):
	is_thread_load_pause = true
	_internal_change_scene_to_file_async(scene_file)

func _internal_change_scene_to_file_async(scene_file: String):
	thread_load_scene_file = scene_file
	thread_load_time = 0
	var loadError := ResourceLoader.load_threaded_request(scene_file)
	if loadError != OK:
		printerr("Failed to load: %s" % scene_file)
		return
	set_process(true)
	await fade_to_black()
	loading_color_rect.show()

func _process(delta: float) -> void:
	thread_load_time += delta
	var progress := []
	var thread_load_status := ResourceLoader.load_threaded_get_status(thread_load_scene_file, progress)
	match thread_load_status:
		ResourceLoader.ThreadLoadStatus.THREAD_LOAD_LOADED:
			var resource := ResourceLoader.load_threaded_get(thread_load_scene_file)
			if thread_load_time < min_thread_load_time:
				await get_tree().create_timer(min_thread_load_time - thread_load_time).timeout
			if is_thread_load_pause:
				change_scene_to_packed_with_pause(resource as PackedScene)
			else:
				change_scene_to_packed(resource as PackedScene)
				
		ResourceLoader.ThreadLoadStatus.THREAD_LOAD_INVALID_RESOURCE:
			pass
		ResourceLoader.ThreadLoadStatus.THREAD_LOAD_IN_PROGRESS:
			loading_progress_bar.value = progress[0]
		ResourceLoader.ThreadLoadStatus.THREAD_LOAD_FAILED:
			printerr("Thread load failed: %s" % thread_load_scene_file)
