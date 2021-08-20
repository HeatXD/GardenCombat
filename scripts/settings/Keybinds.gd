extends Node

static func SetDefaultBindings():
	var defaultBindings = {
		"move_up" : KEY_W,
		"move_down" : KEY_S,
		"move_right" : KEY_D,
		"move_left" : KEY_A,
		"use_item_1" : KEY_Q,
		"use_item_2" : KEY_E,
		"use_item_3" : KEY_R
	}

	for key in defaultBindings:
		InputMap.action_erase_events(key)
		
		var event = defaultBindings[key]
		var scancode = int(event)
		var inputEventKey = InputEventKey.new()
		inputEventKey.set("scancode", scancode)
		
		InputMap.action_add_event(key, inputEventKey)

static func SaveBindings():
	#Loading current keybindings
	var keybindings = {
		"move_up" : InputMap.get_action_list("move_up")[0].scancode,
		"move_down" : InputMap.get_action_list("move_down")[0].scancode,
		"move_right" : InputMap.get_action_list("move_right")[0].scancode,
		"move_left" : InputMap.get_action_list("move_left")[0].scancode,
		"use_item_1" : InputMap.get_action_list("use_item_1")[0].scancode,
		"use_item_2" : InputMap.get_action_list("use_item_2")[0].scancode,
		"use_item_3" : InputMap.get_action_list("use_item_3")[0].scancode
	}

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
		
		var event = loadedBindings[key]
		var scancode = int(event)
		var inputEventKey = InputEventKey.new()
		inputEventKey.set("scancode", scancode)

		InputMap.action_add_event(key, inputEventKey)

func _ready():
	_loadBindings()
