extends Node

func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

#Event Handlers
func _on_SfxSlider_value_changed(value):
	AudioSettings.sfxVolume = value


func _on_MusicSlider_value_changed(value):
	AudioSettings.musicVolume = value
