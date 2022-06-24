# PerfStat
[![NuGet](https://img.shields.io/nuget/v/PerfStat.MonoGame.svg)](https://www.nuget.org/packages/PerfStat.MonoGame/) [![Build status](https://ci.appveyor.com/api/projects/status/r4cd8vcao5i84xo7?svg=true)](https://ci.appveyor.com/project/RomanShapiro/PerfStat)
[![Chat](https://img.shields.io/discord/628186029488340992.svg)](https://discord.gg/ZeHxhCY)

Widget showing the performance of a MonoGame/FNA app

![](/images/perfwidget.png)

## Adding Reference
There are two ways of referencing PerfStat in the project:
1. Through nuget(works only for MonoGame): 
    * https://www.nuget.org/packages/PerfStat.MonoGame/
2. As source code(works for all supported engines):
    
    a. Clone this repo.
    
    b. Add src/PerfStat/PerfStat.{Engine}.csproj to the solution.

      * If FNA is used, then [FontStashSharp](https://github.com/FontStashSharp/FontStashSharp) and [NvgSharp](https://github.com/rds1983/NvgSharp) should be cloned too. The overall folder structure is expected to be following: ![](images/FolderStructure.png)


# Usage
1. PerfStat requires Game to be created with stencil.
   That could be archived by setting PreferredDepthStencilFormat to DepthFormat.Depth24Stencil8 in the GraphicsDeviceManager creation.

I.e.
```c#
public Game1()
{
	_graphics = new GraphicsDeviceManager(this)
	{
		PreferredBackBufferWidth = 1000,
		PreferredBackBufferHeight = 600,
		PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8
	};
}
```

2. Add following usings:
  ```c#
  using NvgSharp;
  using PerfStat;
  ```
  
3. Add following fields to the Game:
  ```c#
  private NvgContext _nvgContext;
  private readonly PerfGraphWidget _perfGraph = new PerfGraphWidget();
  ```
  
4. Add NvgContext creation to the Game.LoadContent:
  ```c#
  _nvgContext = new NvgContext(GraphicsDevice);
  ```
  
5. Finally add following code to the Game.Draw:
  ```c#
  _perfGraph.Update(gameTime.ElapsedGameTime.TotalSeconds);

  _nvgContext.BeginFrame(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 1.0f);
  _perfGraph.Render(_nvgContext, 5, 5);
  _nvgContext.EndFrame();
  ```  
  Where `graphics` is GraphicsDeviceManager.

6. Performance statistics widget should appear:
![](/images/perfstat.gif)

## Credits
* [nanovg](https://github.com/memononen/nanovg)
