using CaptainCoder.Dungeoneering;
// See https://aka.ms/new-console-template for more information
DungeonEditor editor = new ();
ConsoleKeyInfo input;

do 
{
    editor.DrawScreen();
    input = Console.ReadKey();
    editor.HandleInput(input);
} while (input.Key != ConsoleKey.Escape);
