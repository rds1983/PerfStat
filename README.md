# PerfStat
Widget that visualises the performance of MonoGame/FNA app

![](/images/perfwidget.png)

# Usage
1. `Install-Package PerfStat.MonoGame` (or `Install-Package PerfStat.FNA` for FNA)
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
