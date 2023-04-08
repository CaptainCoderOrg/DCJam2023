using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "RuneManifest", menuName = "BodyMind/Rune Manifest")]
public class RuneManifest : ScriptableObject
{
    public RuneData Yin;
    public RuneData Yang;
    public RuneData Sun;
    public RuneData Moon;
    public RuneData Body;
    public RuneData Mind;
    public RuneData Fear;
    public RuneData Calm;
    public RuneData Harmony;
    private RuneData[] _runes;
    public RuneData[] Runes => new []{Yin, Yang, Sun, Moon, Body, Mind, Fear, Calm, Harmony}.OrderBy(rune => rune.RuneIndex).ToArray();

    public RuneData ToRuneData(string phrase) => Runes[int.Parse(phrase)-1];

    public IEnumerable<RuneData> FromPhrase(string phrase)
    {
        foreach (char ch in phrase)
        {
            yield return Runes[int.Parse($"{ch}")-1];
        }
    }

}