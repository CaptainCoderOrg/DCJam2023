using UnityEngine;
[CreateAssetMenu(fileName = "EndOfWorldEvent", menuName = "BodyMind/Event/EndOfWorld")]
public class EndOfWorldEvent : MapEvent
{
    public string God;
    public string LivingGod;

    public override bool OnEnter()
    {
        DialogChain.Dialog(@$"With {God} defeated, {LivingGod} is no longer impeded in their quarrel.")
        .AndThen("The Necromancer is no longer trapped in purgatory forever being torn between his parents, Sol and Lun.")
        .AndThen($"With hatred in his heart, he seeks his revenge on {LivingGod} slaying them.")
        .AndThen($"Alas, with both destroyed, the world no longer has life or death and all must continue forever in a deathless purgatory...")
        .OnFinish(FullHarmony.Credits)
        .Display();
        return false;
    }

}