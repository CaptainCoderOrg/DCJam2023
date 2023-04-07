#if UNITY_EDITOR
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using CaptainCoder.Core;
using CaptainCoder.Dungeoneering;
using UnityEditor;
// using UnityEngine.ProBuilder.Shapes;
// using CaptainCoder.Dungeoneering;

public class MapBuilder
{
    private MapData _map;
    private int MiniMapLayer = LayerMask.NameToLayer("MiniMap");
    private int WallLayer = LayerMask.NameToLayer("Walls");
    private int FloorLayer = LayerMask.NameToLayer("Floors");

    public MapBuilder(MapData database)
    {
        _map = database;
    }

    public void Build(Transform container)
    {
        GameObject floorContainer = new("Floors");
        floorContainer.transform.SetParent(container);
        floorContainer.transform.localPosition = default;
        GameObject ceilingContainer = new("Ceiling");
        ceilingContainer.transform.SetParent(container);
        ceilingContainer.transform.localPosition = default;
        GameObject eventContainer = new("Events");
        eventContainer.transform.SetParent(container);
        eventContainer.transform.localPosition = default;
        BuildFloors(floorContainer.transform, ceilingContainer.transform, eventContainer.transform);
        SetLayer(floorContainer, FloorLayer);

        GameObject miniMapFloors = GameObject.Instantiate(floorContainer);
        miniMapFloors.name = "MiniMap Floors";
        miniMapFloors.transform.SetParent(container);
        miniMapFloors.transform.localPosition = default;
        SetLayer(miniMapFloors, MiniMapLayer);


        GameObject wallContainer = new("Walls");
        wallContainer.transform.SetParent(container);
        wallContainer.transform.localPosition = default;
        BuildWalls(wallContainer.transform);
        SetLayer(wallContainer, WallLayer);

    }
    private void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
        {
            SetLayer(child.gameObject, layer);
        }
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
    }

    private void BuildFloors(Transform floorContainer, Transform ceilingContainer, Transform eventContainer)
    {
        foreach ((CaptainCoder.Core.Position position, ITile tile) in _map.Grid.Tiles)
        {
            if (tile.Symbol == ' ') { continue; }
            GameObject obj = _map.InstantiateTile(tile.Symbol, floorContainer);
            obj.name = $"{position} - {obj.name}";
            obj.transform.localPosition = new Vector3(position.Row * PlayerMovementController.GridCellSize, 0, position.Col * PlayerMovementController.GridCellSize);

            GameObject ceiling = _map.InstantiateTile(tile.Symbol, ceilingContainer);
            ceiling.name = $"{position} - {obj.name}";
            ceiling.transform.localPosition = new Vector3(position.Row * PlayerMovementController.GridCellSize, 5, position.Col * PlayerMovementController.GridCellSize);

            if (_map.TryGetEventsAt(position, out MapData.IEventEntry entry))
            {
                foreach (GameObject prefab in entry.Prefabs)
                {
                    GameObject eventObj = PrefabUtility.InstantiatePrefab(prefab) as GameObject; //, eventContainer);
                    SpikeTrapController c = eventObj.GetComponent<SpikeTrapController>();
                    if (c != null)
                    {
                        // c.Position = new MutablePosition();
                        c.Position.Row = position.Row;
                        c.Position.Col = position.Col;
                    }

                    PositionController positionController = eventObj.GetComponent<PositionController>();
                    if (positionController != null)
                    {
                        // c.Position = new MutablePosition();
                        positionController.Position.Row = position.Row;
                        positionController.Position.Col = position.Col;
                    }
                    eventObj.name = $"{entry.Name} - {position}";
                    eventObj.transform.SetParent(eventContainer);
                    eventObj.transform.localPosition = new Vector3(position.Row * PlayerMovementController.GridCellSize, 0, position.Col * PlayerMovementController.GridCellSize);
                }
            }
        }
    }

}
#endif