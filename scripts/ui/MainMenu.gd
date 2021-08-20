extends Container

var screens = []

func _ready():
	#Storing node objects
	screens.append(get_node("MainMenuContainer"))
	screens.append(get_node("OptionsMenuContainer"))
	screens.append(get_node("PlayMenuContainer"))

	var bgRect = get_node("BgRect")
	var bgTween = get_node("BgTween")
	bgTween.interpolate_property(bgRect, "color", Color.blue-Color(0,0,0,0.5), Color.green-Color(0,0,0,0.5), 10, Tween.TRANS_LINEAR)
	bgTween.start()

#func _process(delta):
#



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
	_navigateToScreen(screens[2])


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


func _on_BgTween_tween_all_completed():
	print("called")
	var bgTween = get_node("BgTween")
	var bgRect = get_node("BgRect")
	
	bgTween.reset_all()
	if(bgRect.color == Color.green-Color(0,0,0,0.5)):
		bgTween.interpolate_property(bgRect, "color", Color.green-Color(0,0,0,0.5), Color.blue-Color(0,0,0,0.5), 10, Tween.TRANS_LINEAR)
	else:
		bgTween.interpolate_property(bgRect, "color", Color.blue-Color(0,0,0,0.5), Color.green-Color(0,0,0,0.5), 10, Tween.TRANS_LINEAR)
	bgTween.start()
