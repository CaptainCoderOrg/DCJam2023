using CaptainCoder.Core;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MentorEvent", menuName = "BodyMind/Events/Mentor Event")]
public class MentorEvent : MapEvent
{
    private bool _isFirst;
    private bool _hasBalanced;
    private void OnEnable()
    {
        _isFirst = true;
        _hasBalanced = false;
    }

    public override bool OnEnter()
    {
        if (_isFirst)
        {
            _isFirst = false;
            First.Display();
        }
        
        return false;
    }

    public override bool OnExit()
    {
        RuneData harmony = GameManager.Instance.Runes.Harmony;
        PlayerData player = GameManager.Instance.Player;
        if (!player.Runes.HasRune(harmony)) 
        { 
            HarmonyRune.Display();
            return true; 
        }
        if(_hasBalanced) { return false; }
        foreach (var stat in player.Stats.Stats)
        {
            if (stat.Value != 0)
            {
                LackHarmony.Display();
                return true;
            }
        }
        _hasBalanced = true;
        return false;
    }

    public override bool OnInteract()
    {
        First.Display();
        return false;
    }

    private static void GiveHarmonyRune()
    {
        RuneData harmony = GameManager.Instance.Runes.Harmony;
        PlayerData player = GameManager.Instance.Player;
        player.Runes.AddRune(harmony);
        MessageController.Display("You now know the power of the Harmony rune.");
    }

    private DialogChain First =
        DialogChain.Dialog(@"
        The necromancer speaks to you, 
        ""Heed my word, apprentice. As above, so below. The one above all is the Sun, and the one beneath all is the Moon. 
        Maintain your body and mind against those that wish to destroy you.""
        ".TrimMultiLine())
        .AndThen(@"
        The rageful Sun deity is Sol, the manifestation of life and the energy of yin. 
        The calm Moon deity Lun is the manifestation of death and the energy of yang. 
        The two cannot exist without the other. Balance is the key for you to be at peace.".TrimMultiLine())
        .AndThen(@"
        I sadly cannot aide you in this quest, my dear apprentice, for these deities have but prevented me from setting foot past this purgatory. 
        You must choose your way.
        ".TrimMultiLine())
        .AndThen(@"
        The choice to go to the fiery sun, who is an overbearing yet warm presence. But be weary, my apprentice, for they are incredibly strong. 
        The choice to go to the cool moon, who is an enigmatic yet cunning presence. But be weary, my apprentice, for they are incredibly vexing. 
        Only by you seeking to fulfill the balance into this world  will you be able to save me from my mortal coil.
        ".TrimMultiLine());
    
    private DialogChain HarmonyRune =
        DialogChain.Dialog(@"
        As you move to step away, your mentor stops you and speaks,
        ""Remember harmony in equity is key to maintaining the dualities of self. 
        Fail to do so, and you shall start where we meet. 
        You lack harmony, take this Harmony rune to begin this journey in duality.""
        ".TrimMultiLine(), "Take Rune")
        .AndThen(@"
        You reach out to take the rune. Once in your hands, it begins to vibrate. You feel
        a new energy enter your body.
        ".TrimMultiLine())
        .AndThen(@"
        ""Excellent!"", the Necromancer says. ""You now know the power of the Harmony rune.""
        ".TrimMultiLine()).OnFinish(GiveHarmonyRune);

    private DialogChain LackHarmony =
        DialogChain.Dialog(@"
        As you move to step away, your mentor stops you and speaks,
        ""Remember harmony in equity is key to maintaining the dualities of self. 
        Fail to do so, and you shall start where we meet. 
        You lack harmony, breath deeply and use the Harmony runes' energy.""
        ".TrimMultiLine());
    
}