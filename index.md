The source code here is for the Nuget packages that can be found [here](https://www.nuget.org/packages?q=Monogame.randomchaos)

Original versions were created as .Net Core 3.1 assemblies, but MonoGame uses (at the time of writing 4th March 2022) .Net Standard class libraries in its templates. This means that they can't reference .NET core 3.1 assemblies. As of the 3rd of March 2022, I moved all the projects over to .Net Standard, when MonoGame is updated to use .Net 6, I dare say Ill update them all again to be .Net 6 assemblies.

So, with all that, what packages are actually here, what do they do, and how cn they be used?

## MonoGame.RandomchaosPpacakges
### [MonoGame.Randomchaos.Interfaces](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Interfaces)
This project holds base line interfaces and abstract classes to be used in conjunction with the other packages here. You can use them to create your own versions of the assemblies here to allowing you to extend the functionality in your own projects.

There is no sample for this as it has little to no functionality in it and is a supporting package.

### [MonoGame.Randomchaos.Services.Audio](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Audio)
This project gives you are service that can be used to play music and sound effects in your game, it has support for 3D sound and you have full volume control over the audio you play.

#### [Sample - TODO]()

### [MonoGame.Randomchaos.Services.Camera](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Camera)
This project will give you access to a 3D camera, using the Transform class in MonoGame.Randomchaos.Intrfaces to rotate and translate the camera in 3D world space. I plan to alter this camera later to support 2D.

#### [Sample - TODO]()

### [MonoGame.Randomchaos.Services.Coroutine](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Coroutine)
Another supporting package, but could be used independently of the others if required. It gives you a service that can manage and run coroutines in your games, similar to how Unity has co routines.

#### [Sample - TODO]()

### [MonoGame.Randomchaos.Services.Encryption](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Encryption)
This package gives some standard services with methods to be able to encrypt data.

#### [Sample - TODO]()

### [MonoGame.Randomchaos.Services.Input](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Input)
This package gives you a service that will manage taking user input from the keyboard, mouse, gamepad and touch.

#### [Sample - TODO]()

### [MonoGame.Randomchaos.Services.Interfaces](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Interfaces)
Again, as with MonoGame.Randomchaos.Interfaces, this packages is a support package and is used so you can build your own services as well as being used by all the services packages here.

There is no sample for this.

### [MonoGame.Randomchaos.Services.Noise](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Noise)
This package has services that will help generate noise for you in your projects.

#### [Sample](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/Sample.MonoGame.Randomchaos.Services.Noise)

### [MonoGame.Randomchaos.Services.Scene](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.Services.Scene)
This package will give you a service that will give you the ability to manage scenes in your projects.

#### [Sample - TODO]()

### [MonoGame.Randomchaos.UI](https://github.com/NemoKradXNA/MonoGame.Randomchaos.Services/tree/main/MonoGame.Randomchaos.UI)
This package will give you some basic UI for your projects.

#### [Sample - TODO]()

## Tutorials
So, as I create tutorials for each of the packages I will add them in here. These will use the samples provided above but give more detail and explanation about them.

I hope you find these packages useful,

Charles.


