using CaptainCoder.Core;
namespace CaptainCoder.Dungeoneering;

public class DungeonEditor
{
    private readonly Dictionary<ConsoleKey, Action> _inputs;
    private Position _gridOffset = (2, 0);
    private Position _cursor;
    private bool _cursorVisible = true;
    private bool _curserPaused = false;
    private readonly List<string> _messages = new();
    private string _filename = string.Empty;
    public IEditorMode Mode = WallMode.Instance;
    public readonly Queue<IEditorMode> Modes = new Queue<IEditorMode>(
        new IEditorMode[]{
            TileMode.Instance,
            WallMode.Instance
        }
    );


    public DungeonEditor()
    {
        _inputs = new();
        _inputs[ConsoleKey.W] = () => Cursor += Facing.MovePosition();
        _inputs[ConsoleKey.S] = () => Cursor -= Facing.MovePosition();
        _inputs[ConsoleKey.E] = () => Cursor += Facing.RotateClockwise().MovePosition();
        _inputs[ConsoleKey.Q] = () => Cursor -= Facing.RotateClockwise().MovePosition();
        _inputs[ConsoleKey.D] = () => Facing = Facing.RotateClockwise();
        _inputs[ConsoleKey.A] = () => Facing = Facing.RotateCounterClockwise();
        _inputs[ConsoleKey.Delete] = () => WallMode.Instance.Delete(this);
        _inputs[ConsoleKey.Backspace] = () => WallMode.Instance.Delete(this);
        _inputs[ConsoleKey.Spacebar] = () => Mode.Draw(this);
        _inputs[ConsoleKey.D0] = Save;
        _inputs[ConsoleKey.Tab] = NextMode;
        _inputs[ConsoleKey.OemPeriod] = () => TileMode.Instance.Draw(this);
        Blink();
    }

    public async void Blink()
    {
        while (true)
        {
            await Task.Delay(500);
            if (_curserPaused) { continue; }
            _cursorVisible = Grid.TileAt(Cursor) == Tile.NoTile ? true : !_cursorVisible;
            DrawCursor();
        }
    }

    public void NextMode()
    {
        Modes.Enqueue(Mode);
        Mode = Modes.Dequeue();
    }

    public Position Cursor
    {
        get => _cursor;
        private set
        {
            _cursor = Position.Max((0, 0), value);
        }
    }
    public Direction Facing { get; private set; }
    public DungeonGrid Grid { get; private set; } = new DungeonGrid();
    public void DrawScreen()
    {
        Console.Clear();
        Console.WriteLine("Dungeon Editor");
        DisplayMode();
        DrawGrid();
        DrawCursor();
        DrawMessages();
    }

    public void DisplayMode()
    {
        Console.SetCursorPosition(0, 1);
        Console.Write($"Mode: {Mode}");
    }

    public void DrawMessages()
    {
        int messageStart = Console.WindowHeight - 4;
        Console.SetCursorPosition(0, messageStart);
        int startIx = Math.Max(0, _messages.Count - 3);
        for (int ix = startIx; ix < _messages.Count; ix++)
        {
            Console.Write(_messages[ix]);
            Console.SetCursorPosition(0, ++messageStart);
        }
    }

    public void DrawGrid()
    {
        foreach ((Position pos, char ch) in Grid.ToASCII())
        {
            SetCursorPosition(pos + _gridOffset);
            Console.Write(ch);
        }
    }

    public void DrawPosition(Position pos)
    {
        SetCursorPosition(_gridOffset + pos.ToASCIIPosition());
        ITile tile = Grid.TileAt(pos);
        if (tile == Tile.NoTile)
        {
            Console.Write(' ');
        }
        else
        {
            Console.Write(tile.Symbol);
        }
    }

    private void SetCursorPosition(Position pos) => Console.SetCursorPosition(pos.Col, pos.Row);

    private void DrawCursor()
    {
        if (_cursorVisible)
        {
            SetCursorPosition(_gridOffset + Cursor.ToASCIIPosition());
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(GridSymbol);
            Console.ResetColor();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
        }
        else
        {
            DrawPosition(_cursor);
        }
    }

    private char GridSymbol => Facing switch
    {
        Direction.North => '^',
        Direction.East => '>',
        Direction.South => 'v',
        Direction.West => '<',
        _ => throw new NotImplementedException()
    };

    public void HandleInput(ConsoleKeyInfo info)
    {
        _inputs.GetValueOrDefault(info.Key, () => InvalidInput(info.KeyChar)).Invoke();
    }
    public void InvalidInput(char ch) => Log($"Invalid Input: '{ch}'");

    public void Log(string message)
    {
        _messages.Add(message);
        DrawMessages();
    }

    public void Save()
    {
        _curserPaused = true;
        Console.CursorVisible = true;
        Log($"Save As ({_filename}): ");
        string filename = Console.ReadLine()!;
        Log(filename);
        if (filename == string.Empty)
        {
            filename = _filename;

        }
        if (filename == string.Empty)
        {
            _curserPaused = false;
            Log("Cancelled");
            return;
        }
        _filename = filename;
        if (filename != _filename && File.Exists(filename))
        {
            Log("File exists.");
        }
        else
        {
            try
            {
                File.WriteAllText(filename, Grid.ToASCII().ToGridString());
                Log("File saved!");
            }
            catch
            {
                Log("File failed to save.");
            }
        }
        _curserPaused = false;
    }
}