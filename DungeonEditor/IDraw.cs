using CaptainCoder.Core;
namespace CaptainCoder.Dungeoneering;
public interface IEditorMode
{
    public string Name { get; }
    public string Draw(DungeonEditor editor);
}

public class WallMode : IEditorMode
{
    public static WallMode Instance { get; } = new WallMode();
    public string Name => "Drawing Wall";

    public string Draw(DungeonEditor editor)
    {
        IWall wall = editor.Grid.WallAt(editor.Cursor, editor.Facing);
        if (wall == Wall.NoWall)
        {
            editor.Grid.SetWall(editor.Cursor, editor.Facing, Wall.Solid);
        }
        else if (wall == Wall.Solid)
        {
            editor.Grid.SetWall(editor.Cursor, editor.Facing, Wall.Door);
        }
        else
        {
            editor.Grid.DeleteWall(editor.Cursor, editor.Facing);
        }
        return string.Empty;
    }
}

public class TileMode : IEditorMode
{
    public static TileMode Instance { get; } = new TileMode();
    public string Name => "Drawing Tiles";
    public string Draw(DungeonEditor editor)
    {
        if (editor.Grid.TileAt(editor.Cursor) == Tile.NoTile)
        {
            editor.Grid.SetTile(editor.Cursor, Tile.Floor);
        }
        else
        {
            editor.Grid.DeleteTile(editor.Cursor);
        }
        return string.Empty;
    }
}