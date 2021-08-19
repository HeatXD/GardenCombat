extends Container

var screens = []

func _ready():
	#Storing node objects
	screens.append(get_node("MainMenuContainer"))
	screens.append(get_node("OptionsMenuContainer"))

#func _process(delta):
#	pass

func _navigateToScreen(targetScreen):
	#Hides all screens and shows target screen
	for screen in screens:
		screen.visible = false;

	targetScreen.visible = true;

#Main Screen Event Handlers
func _unhandled_input(event):
	if event is InputEventKey:
		if event.pressed and event.scancode == KEY_ESCAPE:
			if(!get_node("OptionsMenuContainer/OptionTabsContainer").current_tab == 2):
				_navigateToScreen(screens[0])

func _on_PlayButton_pressed():
	#Navigates to play menu
	_navigateToScreen(screens[0])


func _on_OptionsButton_pressed():
	#Navigates to options menu
	_navigateToScreen(screens[1])


func _on_ExitButton_pressed():
	#Exits the game
	get_tree().quit()



#Options Screen Event Handler
func _on_BackButton_pressed():
	_navigateToScreen(screens[0])


func _on_SaveConfirmDialog_confirmed():
	#Saving keybind settings
	Keybinds.SaveBindings()
