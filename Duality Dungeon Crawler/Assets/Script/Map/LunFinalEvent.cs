using UnityEngine;
[CreateAssetMenu(fileName = "LunFinalEvent", menuName = "BodyMind/Event/LunFinalEvent")]
public class LunFinalEvent : MapEvent
{

    public CombatStartEvent StartCombatEvent;
    public static bool _isInCombat { get; set; } = false;

    public void OnEnable()
    {
        _isInCombat = false;
    }

    public override bool OnExit()
    {
        if (_isInCombat) { return false; }
        DialogChain.Dialog("Before you is a dark shadow... It is Lun, the Moon Diety!")
        .AndThen(@"Lun speaks, ""So, my son has sent you to kill me. If only he understood the love I have for 
        him and that his mother Sol is the one who keeps him in purgatory whilst I try to free him.".TrimMultiLine())
        .AndThen(@"Are you sure you wish to quarrel with me? Leave now and you shall be spared.".TrimMultiLine()).OnFinish(GiveChoice).Display();
        return false;
    }

    public void GiveChoice()
    {
        DialogController.Instance.DisplayDialog("Dare you face Lun?");
        DialogController.Instance.SetOptions(
            ("Return to Purgatory", ReturnToPurgatory),
            ("Fight Lun", FightSol)
        );
        DialogController.Instance.IsVisible = true;
    }

    public void ReturnToPurgatory()
    {
        DialogController.Instance.IsVisible = false;
        PlayerMovementController.Instance.CurrentMap = GameManager.Instance.EntranceMap;
        PlayerMovementController.Instance.Position = (2, 2);
    }

    public void FightSol()
    {
        DialogController.Instance.IsVisible = false;
        StartCombatEvent.OnEnter();
        _isInCombat = true;
    }

}