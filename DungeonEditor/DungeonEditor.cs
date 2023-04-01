using CaptainCoder.Core;
namespace CaptainCoder.Dungeoneering;

public class DungeonEditor
{
    private readonly Dictionary<ConsoleKey, Action> _inputs;
    private Position _gridOffset = (2, 0);
    private Position _cursor;
    private List<string> _messages = new();

    public DungeonEditor()
    {
        _inputs = new ();
        _inputs[ConsoleKey.W] = () => Cursor += Facing.MovePosition();
        _inputs[ConsoleKey.S] = () => Cursor -= Facing.MovePosition();
        _inputs[ConsoleKey.D] = () => Facing = Facing.RotateClockwise();
        _inputs[ConsoleKey.A] = () => Facing = Facing.RotateCounterClockwise();
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
        DrawGrid();
        DrawCursor();
        DrawMessages();
    }

    public void DrawMessages()
    {
        int messageStart = Console.WindowHeight - 4;
        Console.SetCursorPosition(0, messageStart);
        int startIx = Math.Max(0,_messages.Count - 3);
        for (int ix = startIx; ix < _messages.Count; ix++)
        {
            Console.Write(_messages[ix]);
            Console.SetCursorPosition(0, ++messageStart);
        }
    }

    public void DrawGrid()
    {
        foreach ((Position pos, char ch ) in Grid.ToASCII())
        {
            SetCursorPosition(pos + _gridOffset);
            Console.Write(ch);
        }
    }

    private void SetCursorPosition(Position pos) => Console.SetCursorPosition(pos.Col, pos.Row);

    private void DrawCursor()
    {
        SetCursorPosition(_gridOffset + Cursor.ToASCIIPosition());
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(GridSymbol);
        Console.ResetColor();
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
}