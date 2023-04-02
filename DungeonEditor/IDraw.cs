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
        CheckTile(editor);
        return string.Empty;
    }

    public void Delete(DungeonEditor editor)
    {
        editor.Grid.DeleteWall(editor.Cursor, editor.Facing);
        CheckTile(editor);
    }

    private void CheckTile(DungeonEditor editor)
    {
        if (editor.Grid.HasWallsAt(editor.Cursor) && !editor.Grid.HasTileAt(editor.Cursor))
        {
            editor.Grid.SetTile(editor.Cursor, Tile.Floor);
        }
        else if (!editor.Grid.HasWallsAt(editor.Cursor) && editor.Grid.TileAt(editor.Cursor) == Tile.Floor)
        {
            editor.Grid.DeleteTile(editor.Cursor);
        }
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