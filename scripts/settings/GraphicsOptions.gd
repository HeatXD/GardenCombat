extends Node

var resolutionButton
var fullscreenButton

func _ready():

	#Storing node objects
	fullscreenButton = get_node("HSplitContainer/GraphicsOptionsContainer/Fullscreen/FullscreenButton")
	resolutionButton = get_node("HSplitContainer/GraphicsOptionsContainer/Resolution/ResolutionButton")

	#Setting FullscreenButton to correct state
	_updateFullscreenButton()

	#Populating resolutionButton with options
	resolutionButton.add_item("1280x720", 0)
	resolutionButton.add_item("1366x768", 1)
	resolutionButton.add_item("1600x900", 2)
	resolutionButton.add_item("1920x1080", 3)
	resolutionButton.add_item("2560x1440", 4)
	resolutionButton.add_item("3840x2160", 5)


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _updateFullscreenButton():
	if(OS.window_fullscreen):
		fullscreenButton.text = "On"
		fullscreenButton.pressed = true
		resolutionButton.disabled = true
	else:
		fullscreenButton.text = "Off"
		fullscreenButton.pressed = false
		resolutionButton.disabled = false

#Event Handlers
func _on_ResolutionButton_item_selected(index):
	#Changing game resolution to selected option
	var optionText = resolutionButton.get_item_text(index)
	var splitText = optionText.split("x", true, 1)
	var newSize = Vector2(int(splitText[0]), int(splitText[1]))

	if(!OS.window_fullscreen):
		OS.window_size = newSize

		#Centering screen
		var monitorSize = OS.get_screen_size()
		var windowSize = OS.window_size
		
		var xOffset = (monitorSize.x - windowSize.x)/2
		var yOffset = (monitorSize.y - windowSize.y)/2

		if(!OS.window_borderless):
			#Adjusting for windows option bar at the top of the window
			yOffset -= 9.5
		
		OS.window_position = Vector2(xOffset, yOffset)
	else:
		OS.window_size = newSize
		OS.window_position = Vector2(0,0)




func _on_FullscreenButton_toggled(button_pressed):
	#Toggles the fullscreen setting
	OS.window_fullscreen = button_pressed
	_updateFullscreenButton()
