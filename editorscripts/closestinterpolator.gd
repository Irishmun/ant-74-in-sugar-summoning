@tool
extends EditorScript


# Called when the node enters the scene tree for the first time.
func _run():
	var selection = get_editor_interface().get_selection() 
	selection = selection.get_selected_nodes()  # get the actual AnimationPlayer node
	if selection.size()!=1 and not selection is AnimationPlayer: # if the wrong node is selected, do nothing
		return
	else:
		interpolation_change(selection,"ant/idle") # run the funstion in question
		interpolation_change(selection,"ant/walk_Forward") # run the funstion in question


# Called every frame. 'delta' is the elapsed time since the previous frame.
func interpolation_change(selection, name):
	var anim_track_1 = selection[0].get_animation(name) # get the Animation that you are interested in (change "default" to your Animation's name)
	var count  = anim_track_1.get_track_count() # get number of tracks (bones in your case)
	print(count)
	for i in count:
		
		anim_track_1.track_set_interpolation_type(i, 0) # change interpolation mode for every track
		print(anim_track_1.track_get_interpolation_type(i))
