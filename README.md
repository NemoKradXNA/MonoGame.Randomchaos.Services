# MonoGame.Randomchaos.Services
Services that can be used in any MonoGame project.

Please see the git pages for this project [here](https://nemokradxna.github.io/MonoGame.Randomchaos.Services/)

Code that has been published as a NuGet package can be found [here](https://www.nuget.org/packages?q=MonoGame.Randomchaos.&prerel=true&sortby=relevance).

Hope you find it useful.

# Samples
There are a number of samples showing how the various packages are intended to be used.

## Sample.MonoGame.Randomchaos.Services.Noise
Shows a simple example of noise generation. The noise is rendered to a Texture2D.

## Sample.MonoGame.Randomchaos.Services.Scene
Shows how a simple set of scenes can be set up and moved between.

## Samples.HardwareInstancedParticles
Hardware instanced particles, the camera, coroutine and input packages are used here.

## Samples.MonoGame.Randomchaos.EFCore
A basic example of how the EF package can be used.

## Samples.MonoGame.Randomchaos.Physics
My own fledgling physics engine, based on the book Game Physics Engine Development by Ian Millington.
Examples of force generation.

## Samples.MonoGame.Randomchaos.PostProcessing
A post processing frame work example, has a few post processing effects in it, bleaching, grey scale chromatic aberration.

## Samples.MonoGame.Randomchaos.Primitives3D
How the primatives can be rendered, these are great for building geometry on the fly for prototyping. Also now has a simple Voxel example in it.

## Samples.MonoGame.Randomchaos.Windows.Audio
I am playing around with writing my own Chip Tune audio engine, early days.

# Templates
To make my life easier (when creating new projects) I have created a set of templates that will create a MonoGame project and include the most common packages I use.
The templates come with some place holder assets, I am no artist, so they are pretty basic. The audio files I created with [Boob Box](https://www.beepbox.co/)

## MonoGame.Randomchaos.Cross_Platform_Desktop_Template
## MonoGame.Randomchaos.Windows_Desktop_Template

# Packages
Here is a list of all the packages. If you see one is missing let me know and I'll update this page.

## MonoGame.Randomchaos.EFCore
Some basic (need to do more on this really) EF elements.

## MonoGame.Randomchaos.Extensions
Lots of extension methods to make my life easier for float, Quaternion, Random, Texture2D, Vector2D, Vector3D, and probably more as I need them.

## MonoGame.Randomchaos.Interfaces
This is the base package for most, if not all the packages.

## MonoGame.Randomchaos.Physics
This is the package for my physics engine.

## MonoGame.Randomchaos.PostProcessing
Post processing package, no shaders are supplied, just the framework.

## MonoGame.Randomchaos.Primitives3D
A number of primitives that are created at runtime, Triangle, Quad, Cube, Capsule etc.

## MonoGame.Randomchaos.Services.Audio
No audio files here, just a simple audio framework for playing music and sound effects, manages the volume too.

## MonoGame.Randomchaos.Services.Camera
A 3D camera.

## MonoGame.Randomchaos.Services.Coroutine
Frame work used to run corouties, this is very similar to the one in Unity, I found it was useful and it's a nice to have in MonoGame.

## MonoGame.Randomchaos.Services.Encryption
AeS Encryption.

## MonoGame.Randomchaos.Services.Input
An input manager for Keyboard, Mouse, Gamepad and Touch

## MonoGame.Randomchaos.Services.Interfaces
This is the base package of all the services.

## MonoGame.Randomchaos.Services.Noise
Some noise generators.

## MonoGame.Randomchaos.Services.Scene
Scene management framework. Has built in audio management, input and post processing of both the UI and the Scene if needed.

## MonoGame.Randomchaos.UI
Basic UI implementation, buttons, images, labels, sliders etc.
