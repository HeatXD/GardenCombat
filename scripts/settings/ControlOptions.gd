extends Node

var bindingsChanged = false

var canChangeKey = false
var actionString
enum ACTIONS {move_up, move_down, move_left, move_right, use_item_1, use_item_2, use_item_3}

func _ready():
	_setKeys()

func _setKeys():
	for action in ACTIONS:
		get_node("HSplitContainer/ControlsOptionsContainer/" + str(action) + "/Button").set_pressed(false)
		if !InputMap.get_action_list(action).empty():
			get_node("HSplitContainer/ControlsOptionsContainer/" + str(action) + "/Button").set_text(InputMap.get_action_list(action)[0].as_text())
		else:
			get_node("HSplitContainer/ControlsOptionsContainer/" + str(action) + "/Button").set_text("Not Mapped")

func _markButton(string):
	canChangeKey = true
	actionString = string

	for action in ACTIONS:
		if action != string:
			get_node("HSplitContainer/ControlsOptionsContainer/" + str(action) + "/Button").set_pressed(false)

func _changeKey(newKey):
	#Delete key of pressed button
	if !InputMap.get_action_list(actionString).empty():
		InputMap.action_erase_event(actionString, InputMap.get_action_list(actionString)[0])

	#Check if new key was assigned somewhere
	for action in ACTIONS:
		if InputMap.action_has_event(action, newKey):
			InputMap.action_erase_event(action, newKey)

	#Add new Key
	InputMap.action_add_event(actionString, newKey)

	#Marking bindings as changed
	bindingsChanged = true
	get_node("HSplitContainer/RightContainer/SavedLabel").visible = true

	_setKeys()

func _input(event):
	if event is InputEventKey:
		if canChangeKey:
			_changeKey(event)
			canChangeKey = false


func _saveBindings():
	Keybinds.SaveBindings()
	bindingsChanged = false
	get_node("HSplitContainer/RightContainer/SavedLabel").visible = false
# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

#Event Handlers
func _on_Button_MoveUp_pressed():
	_markButton("move_up")


func _on_Button_MoveDown_pressed():
	_markButton("move_down")


func _on_Button_MoveRight_pressed():
	_markButton("move_right")


func _on_Button_MoveLeft_pressed():
	_markButton("move_left")


func _on_Button_UseItem1_pressed():
	_markButton("use_item_1")


func _on_Button_UseItem2_pressed():
	_markButton("use_item_2")


func _on_Button_UseItem3_pressed():
	_markButton("use_item_3")


func _on_SaveButton_pressed():
	_saveBindings()

func _unhandled_input(event):
	if event is InputEventKey:
		if event.pressed and event.scancode == KEY_ESCAPE:
			if bindingsChanged:
				#Prompting user to save altered keybinds
				get_node("../../SaveConfirmDialog").show()
			else:
				get_node("../../../OptionsMenuContainer").visible = false
				get_node("../../../MainMenuContainer").visible = true


func _on_SaveConfirmDialog_confirmed():
	_saveBindings()


func _on_BackButton_pressed():
	if bindingsChanged:
		#Prompting user to save altered keybinds
		get_node("../../SaveConfirmDialog").show()
	else:
		get_node("../../../OptionsMenuContainer").visible = false
		get_node("../../../MainMenuContainer").visible = true


func _on_ReturnToDefaultButton_pressed():
	bindingsChanged = true
	get_node("HSplitContainer/RightContainer/SavedLabel").visible = true

	Keybinds.SetDefaultBindings()
	_setKeys()
