using UnityEngine;

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
    public RuneData Balance;
}