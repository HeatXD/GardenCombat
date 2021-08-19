extends Node

static func SaveBindings():
	#Loading current keybindings
	var keybindings = {
		"move_up" : InputMap.get_action_list("move_up"),
		"move_down" : InputMap.get_action_list("move_down"),
		"move_right" : InputMap.get_action_list("move_right"),
		"move_left" : InputMap.get_action_list("move_left"),
		"use_item_1" : InputMap.get_action_list("use_item_1"),
		"use_item_2" : InputMap.get_action_list("use_item_2"),
		"use_item_3" : InputMap.get_action_list("use_item_3"),
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

	saveFile.close()
	for key in loadedBindings:
		#InputMap.action_erase_events(key)
		for event in loadedBindings[key]:
			var ev = InputEvent.new()
			#InputMap.action_add_event(key, inputEvent)
			print(ev)

func _ready():
	_loadBindings()
