# EMMA - Extension Method Manager

A simple tool aid in reusing extension methods by offering a selection extension methods, which filtered by extending type, return type and/or name as required and copies a selected method to the ClipBoard ready for quick insertion in to your project.

*NOTE: Requires a valid Github Auth key (see below)*


# What does is it actually do then?

Upon first time initialisation, `EMMA` will read the contents of a default C# Extension Method library repository on github (*TODO* LINK HERE).

It will cache a local copy of all the code for extension methods which can then be seareched via the main tool window using Extending type, Return type and/or Method name as search criteria. Once the desired method is found, clicking the copy button while place the method's code in the ClipBoard.


## Why does this exist?

How many different projects have you written the same simple little extension method in? 

I for one have a `ToInt()` extension for `string` in every project that has some form of user inputted or imported data.

```
	public static int ToInt(this string value, int @default = 0)
            => int.TryParse(value, out var result) ? result : @default;

```


Every time I typed this out yet again, for a new project I briefly wondered if there was a better way to avoid this kind of "duplication".

Originally, I kept a seperate library of "Useful Stuff" that included many extension methods, but a whole package dependency felt overkill when I only wanted a few short lines of code most of the time.

So I started think about them more as specialised "code-snippets" rather than a more typical reusable component. Something that would be fairly specific to Extension Methods and managing them as snippets of text.

An Extension Method Manager is you will... hence `EMMA` was born, and once I had a good acronynm for the name it seemed worth actually doing something about the idea;)

## Github Auth Key

`EMMA` currently requires a personal access token to give access to the Github API, this (currently) must be stored in an environment variable named `EMMA_APP_KEY`.

You can create a key from your Github account settings. See [https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token](https://docs.github.com/en/github/authenticating-to-github/creating-a-personal-access-token) for more details.

