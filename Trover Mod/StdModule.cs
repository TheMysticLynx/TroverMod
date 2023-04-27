using System;
using System.Collections.Generic;
using Indev2;
using Modding;

//Class name is mod in default namespace
public class Mod : IMod
{
    public static Interface Interface;
    public string UniqueName => "302b35d1-050c-4481-b232-30877b58a72d-Mystic-Trover";
    public string DisplayName => "Trover Mod";
    public string Author => "Mystic";
    public string Version => "2.0";
    public string Description => "Adds the trash mover to the game!";
    public string[] Dependencies => Array.Empty<string>();

    public void Initialize(Modding.Interface @interface)
    {
        Interface = @interface;
    }

    public IEnumerable<CellProcessor> GetCellProcessors(ICellGrid cellGrid)
    {
        yield return new BasicCellProcessor(cellGrid);
        yield return new SlideCellProcessor(cellGrid);
        yield return new FreezeProcessor(cellGrid);
        yield return new GeneratorCellProcessor(cellGrid);
        yield return new CWRotateProcessor(cellGrid);
        yield return new CCWRotateProcessor(cellGrid);
        yield return new MoverCellProcessor(cellGrid);
        yield return new TrashMover(cellGrid);
        yield return new WallCellProcessor(cellGrid);
        yield return new TrashCellProcessor(cellGrid);
        yield return new EnemyCellProcessor(cellGrid);
    }
}
