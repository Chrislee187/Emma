# EMMA - Extension Method Manager

A simple tool to aid in reusing extension methods by offering access to a central library of extension methods from within Visual Studio with ability to quickly copy methods to your current solution without having to import complete libraries/packages etc..

# How to use

Once installed the Emma window can be found in `View > Other Windows > Emma`, usage should be pretty self-explantory;

* Find the required method using the search or filter drop-downs.

* Click copy to place in the code clipboard

# What does is it actually do then?

Upon first time initialisation, `EMMA` will read the contents of a default C# Extension Method library repository on github (https://github.com/chrislee187/methodbrary).

It will cache a local copy of all the code for extension methods (in you AppData folder) which can then be searehed via the main tool window using Extending type, Return type and/or Method name as search criteria. Once the desired method is found, clicking the copy button will place the method's code in the ClipBoard.

## Why does this exist?

How many different projects have you written the same simple little extension method in? 

I for one have a `ToInt()` extension for `string` in every project that has some form of user inputted or imported data.

```
	public static int ToInt(this string value, int @default = 0)
            => int.TryParse(value, out var result) ? result : @default;

```


Every time I typed this out yet again, for a new project I briefly wondered if there was a better way to avoid this kind of "duplication".

Snippets didn't really work how I wanted, and a whole package dependency felt overkill when I only wanted a few short lines of code most of the time.

 So I started think about them more as specialised "code-snippets" rather than a more typical reusable component. Something that would be fairly specific to Extension Methods.

An Extension Method Manager if you will... hence `EMMA` was born, and once I had a good acronynm for the name it seemed worth actually doing something about the idea;)

