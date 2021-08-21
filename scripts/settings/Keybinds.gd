extends Node

static func SetDefaultBindings():
	var defaultBindings = {
		"move_up" : KEY_W,
		"move_down" : KEY_S,
		"move_right" : KEY_D,
		"move_left" : KEY_A,
		"select_item_1" : KEY_Q,
		"select_item_2" : KEY_E,
		"select_item_3" : KEY_R,
		"use_selected_item" : BUTTON_LEFT
	}

	for key in defaultBindings:
		InputMap.action_erase_events(key)
		
		var event = defaultBindings[key]
		if(event != BUTTON_LEFT):
			var scancode = int(event)
			var inputEventKey = InputEventKey.new()
			inputEventKey.set("scancode", scancode)
			InputMap.action_add_event(key, inputEventKey)
		else:
			var inputEventMouseButton = InputEventMouseButton.new()
			inputEventMouseButton.set("button_index", BUTTON_LEFT)
			InputMap.action_add_event(key, inputEventMouseButton)

static func SaveBindings():
	#Loading current keybindings
	var keybindings = {
		"move_up" : InputMap.get_action_list("move_up")[0],
		"move_down" : InputMap.get_action_list("move_down")[0],
		"move_right" : InputMap.get_action_list("move_right")[0],
		"move_left" : InputMap.get_action_list("move_left")[0],
		"select_item_1" : InputMap.get_action_list("select_item_1")[0],
		"select_item_2" : InputMap.get_action_list("select_item_2")[0],
		"select_item_3" : InputMap.get_action_list("select_item_3")[0],
		"use_selected_item" : InputMap.get_action_list("use_selected_item")[0]
	}

	for key in keybindings:
		if keybindings[key].get_class() == InputEventKey.new().get_class():
			var saveText = "Key:" + str(keybindings[key].scancode)
			keybindings[key] = saveText
		elif keybindings[key].get_class() == InputEventMouseButton.new().get_class():
			var saveText = "Mouse:" + str(keybindings[key].button_index)
			keybindings[key] = saveText

	#Saving bindings to file
	var saveFile = File.new()
	saveFile.open("user://keyBindings.save", File.WRITE)
	saveFile.store_line(to_json(keybindings))
	saveFile.close()

func _loadBindings():
	var saveFile = File.new()
	
	if(!saveFile.file_exists("user://keyBindings.save")):
		#File doesn't exist
		return

	saveFile.open("user://keyBindings.save", File.READ)
	var loadedBindings = parse_json(saveFile.get_line())
	print(loadedBindings)

	saveFile.close()
	for key in loadedBindings:
		InputMap.action_erase_events(key)

		var type = loadedBindings[key].split(":")[0]
		var event = loadedBindings[key].split(":")[1]

		if type == "Key":
			var scancode = int(event)
			var inputEventKey = InputEventKey.new()
			inputEventKey.set("scancode", scancode)

			InputMap.action_add_event(key, inputEventKey)
		elif type == "Mouse":
			var buttonIndex = int(event)
			var inputEventMouseButton = InputEventMouseButton.new()
			inputEventMouseButton.set("button_index", buttonIndex)

			InputMap.action_add_event(key, inputEventMouseButton)

func _ready():
	_loadBindings()
