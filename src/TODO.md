# Known Issues/Bugs

* Focus is not set to the member name search control when window received focus
* Shortcuts for the return/extending types not working (ALT-E, ALT-R)

# TODOS

* Honour the currently active VS Theme (i.e. Dark) better.
* HOTKEY: Pressing return on the Methods list should activate the Copy feature
* After copying text to clipboard, can we return to our code point?
* Automatically add the selected extension method to a file (adding file if necessary) to a selected project (prompt user?)
	*	Use simple DEFAULT convention like for the filename;
		* `Extensions\{extending-type}Extensions.cs`, i.e. for `StringExtensions` for methods that extends 
		the `String/string` type or `CacheOfPerson.cs` for a generic like `Cache<Person>`.
* Settings panel - Extension Method Sources
Settings - dropdown menu from the settings button on the main EMMA window?
	*	Enable/Disable of default Methodbrary
	*	Add custom sources for extension methods
	*	Ability to force a refresh on an Extension Method Source
* Release to VS gallery

# UI Tweaks

* Better UX flow, everything should be able to do be done easily with keyboard, without resorting to mouse
* `CodePreview` textbox should fill the vertical space.
* HSplitter between the `CodePreview` textbox and `Methods` listbox
* Make the tool honour the VS theme?

