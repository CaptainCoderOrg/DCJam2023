using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using CaptainCoder.Core;
using CaptainCoder.Dungeoneering;
// using UnityEngine.ProBuilder.Shapes;
// using CaptainCoder.Dungeoneering;

public class MapBuilder
{
    private MapData _map;

    public MapBuilder(MapData database)
    {
        _map = database;
    }

    public void Build(Transform container)
    {
        GameObject floorContainer = new("Floors");
        floorContainer.transform.parent = container;
        BuildFloors(floorContainer.transform);
        GameObject wallContainer = new("Walls");
        wallContainer.transform.parent = container;
        BuildWalls(wallContainer.transform);
    }

    private float RowOffset(Direction direction)
    {
        return direction switch
        {
            Direction.North => -4,
            // Direction.South => 0,
            // Direction.East => 0,
            Direction.West => -1.5f,
            _ => throw new System.NotImplementedException(),
        };
    }

    private float ColOffset(Direction direction)
    {
        return direction switch
        {
            Direction.North => 2f,
            // Direction.South => 0,
            // Direction.East => 0,
            Direction.West => -0.5f,
            _ => throw new System.NotImplementedException(),
        };
    }

    private void BuildWalls(Transform container)
    {
        List<GameObject> allWalls = new();
        foreach ((WallPosition position, IWall wall) in _map.Grid.Walls)
        {
            if (wall.Symbol == ' ') { continue; }
            GameObject obj = _map.InstantiateWall(wall.Symbol, position.IsNorthSouth, container);
            allWalls.Add(obj);
            float row = position.Position.Row * PlayerMovementController.GridCellSize + RowOffset(position.Direction);
            float col = position.Position.Col * PlayerMovementController.GridCellSize + ColOffset(position.Direction);
            obj.name = $"({position.Position.Row}, {position.Position.Col}, {position.Direction}) - {obj.name}";
            obj.transform.localPosition = new Vector3(row, 0, col);

            WallTileEndDetector[] ends = obj.GetComponentsInChildren<WallTileEndDetector>();
            foreach (WallTileEndDetector end in ends)
            {
                end.DetectOtherEnds();
            }
        }
        // List<GameObject> allWalls = new();
        // for (int r = 0; r < rows.Length; r++)
        // {
        //     // On even rows, we care about the odd columns
        //     // On odd rows, we care about the even columns
        //     int startColumn = r % 2 == 0 ? 1 : 0;
        //     bool isNorthSouth = r % 2 == 1;
        //     for (int c = startColumn; c < rows[r].Length; c += 2)
        //     {
        //         char ch = rows[r][c];
        //         if (ch == ' ') { continue; }
        //         GameObject obj = _database.InstantiateWall(ch, isNorthSouth, container);
        //         allWalls.Add(obj);

        //         float row = (r * .5f);
        //         float col = (c * .5f);
        //         obj.name = $"({row}, {col}) - {obj.name}";
        //         obj.transform.localPosition = new Vector3(row * PlayerMovementController.GridCellSize, 0, col * PlayerMovementController.GridCellSize);

        //         WallTileEndDetector[] ends = obj.GetComponentsInChildren<WallTileEndDetector>();
        //         foreach (WallTileEndDetector end in ends)
        //         {
        //             end.DetectOtherEnds();
        //         }
        //     }
        // }
        // GameObject merged = MergeMeshes(allWalls, _database.WallMaterial);
        // merged.transform.parent = container;
    }

    private void BuildFloors(Transform container)
    {
        foreach ((CaptainCoder.Core.Position position, ITile tile) in _map.Grid.Tiles)
        {
            if (tile.Symbol == ' ') { continue; }
            GameObject obj = _map.InstantiateTile(tile.Symbol, container);
            obj.name = $"{position} - {obj.name}";
            obj.transform.localPosition = new Vector3(position.Row * PlayerMovementController.GridCellSize, 0, position.Col * PlayerMovementController.GridCellSize);
        }
    }
}
