using UnityEngine;
using NaughtyAttributes;

public class MapLoaderController : MonoBehaviour
{
    public readonly static string ChildContainer = "Generated Grid";
    [field: SerializeField]
    public MapData MapData { get; private set; }


    [Button("Force Rebuild")]
    public void BuildMap()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == ChildContainer)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        MapBuilder builder = new(MapData);
        GameObject parent = new(ChildContainer);
        parent.transform.SetParent(this.transform);
        parent.transform.localPosition = new Vector3(0, 0, 0);
        builder.Build(parent.transform);
    }
}