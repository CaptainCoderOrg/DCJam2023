using CaptainCoder.Core;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MentorEvent", menuName = "BodyMind/Events/Mentor Event")]
public class MentorEvent : MapEvent
{
    private bool _isFirst;
    private bool _hasBalanced;
    private void OnEnable()
    {
        _isFirst = true;
        _hasBalanced = false;
        _hints = null;
    }

    public override bool OnEnter()
    {
        int completedIntro = PlayerPrefs.GetInt("CompletedIntro", 0);
        if (completedIntro == 0)
        {
            _isFirst = false;
            First.Display();
            GameManager.Instance.AbilityController.OnAbilityFinished += CheckHarmony;
        }
        else
        {
            _hasBalanced = true;
            _isFirst = false;
        }
        
        return false;
    }

    private void CheckHarmony(AbilityDefinition ability)
    {
        if (ability is BalanceAbility)
        {
            _hasBalanced = true;
            PlayerPrefs.SetInt("CompletedIntro", 1);
        }
    }

    int nextHint = 0;

    public override bool OnExit()
    {
        RuneData harmony = GameManager.Instance.Runes.Harmony;
        PlayerData player = GameManager.Instance.Player;
        if (!player.Runes.HasRune(harmony)) 
        { 
            HarmonyRune.Display();
            return true; 
        }
        if(_hasBalanced) { 
            PlayerPrefs.SetInt("CompletedIntro", 1);

            return false; 
        }
        foreach (var stat in player.Stats.Stats)
        {
            if (stat.Value != 0)
            {
                LackHarmony.Display();
                return true;
            }
        }
        _hasBalanced = true;
        PlayerPrefs.SetInt("CompletedIntro", 1);

        return false;
    }

    public override bool OnInteract()
    {
        int completedIntro = PlayerPrefs.GetInt("CompletedIntro", 0);
        if (completedIntro == 0)
        {
            First.Display();
            _isFirst = false;
        }
        else
        {
            DialogChain[] hints = Hints.Where(f => f.Item1.Invoke()).Select(f => f.Item2).ToArray();
            hints[nextHint++ % hints.Length].Display();
        }
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
        The two cannot exist without the other. Harmony is the key for you to be at peace.".TrimMultiLine())
        .AndThen(@"
        I sadly cannot aide you in this quest, my dear apprentice, for these deities have but prevented me from setting foot past this purgatory. 
        You must choose your way.
        ".TrimMultiLine())
        .AndThen(@"
        The choice to go to the fiery sun, who is an overbearing yet warm presence. But be weary, my apprentice, for they are incredibly strong. 
        The choice to go to the cool moon, who is an enigmatic yet cunning presence. But be weary, my apprentice, for they are incredibly vexing. 
        Only by you seeking to fulfill the harmony into this world  will you be able to save me from my mortal coil.
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
    
    
    private DialogChain UseYinHint =
        DialogChain.Dialog(@"
        Only with the power of the Yin rune can you enter Sol's lair. Stand upon
        Sol's portal and channel your Yin energy.
        ".TrimMultiLine());

    private DialogChain UseYangHint =
        DialogChain.Dialog(@"
        Only with the power of the Yang rune can you enter Lun's lair. Stand upon
        Lun's portal and channel your Yang energy.
        ".TrimMultiLine());

    private DialogChain SpellBookHint =
        DialogChain.Dialog(@"If you find yourself uncertain, check your Spellbook.".TrimMultiLine());

    private DialogChain DensityHint =
        DialogChain.Dialog(@"You can harness Lun's shade to create a heavy ball.".TrimMultiLine());

    private DialogChain ShadeHint =
        DialogChain.Dialog(@"Use Lun's Shade to protect yourself from Sol's light.".TrimMultiLine());

    private DialogChain LightHint =
        DialogChain.Dialog(@"Use Sol's Light to protect yourself from Lun's shade".TrimMultiLine());

    private DialogChain FloatHint =
        DialogChain.Dialog(@"Channeling Sol's Light into Harmony will allow you to float.".TrimMultiLine());
    private DialogChain EyeHint =
        DialogChain.Dialog(@"Channeling Harmony into Sun allows you to see through Sol's Eye.".TrimMultiLine());
    private DialogChain EyeHint2 =
        DialogChain.Dialog(@"Channeling Harmony into Moon allows you to see through Lun's Eye.".TrimMultiLine());

    private DialogChain ChannelHint =
        DialogChain.Dialog(@"Yin energy can be converted into Sun energy.".TrimMultiLine());
    
    private DialogChain ChannelHint2 =
        DialogChain.Dialog(@"Yang energy can be converted into Moon energy.".TrimMultiLine());

    private DialogChain BodyMindHint =
        DialogChain.Dialog(@"You can restore your body and mind energies by focusing Yin and Yang energies.".TrimMultiLine());

    private DialogChain FireballHint =
        DialogChain.Dialog(@"Light and Yin energy can be combined to create fire.".TrimMultiLine());

    private DialogChain IceballHint =
        DialogChain.Dialog(@"Shade and Yang energy can be combined to create ice.".TrimMultiLine());

    private DialogChain LunTempleHint =
        DialogChain.Dialog(@"To defeat Lun you must combine Yang, Moon, Mind, and Harmony.".TrimMultiLine());

    private DialogChain SolTempleHint =
        DialogChain.Dialog(@"To defeat Sol you must combine Body, Sun, Yin, and Harmony.".TrimMultiLine());

    private List<(Func<bool>, DialogChain)> _hints;
    private List<(Func<bool>, DialogChain)> Hints => _hints ??= LoadHints(); 

    private List<(Func<bool>, DialogChain)> LoadHints()
    {
        List<(Func<bool>, DialogChain)>  hints = new();
        var player = GameManager.Instance.Player;
        var runes = GameManager.Instance.Player.Runes;
        var runeManifest = GameManager.Instance.Runes;
        hints.Add((True, SpellBookHint));
        hints.Add((() => !runes.HasRune(runeManifest.Yang), DensityHint));
        hints.Add((() => !runes.HasRune(runeManifest.Yin), ShadeHint));
        hints.Add((() => !runes.HasRune(runeManifest.Yang), LightHint));
        hints.Add((() => !runes.HasRune(runeManifest.Yin), FloatHint));
        hints.Add((True, EyeHint));
        hints.Add((True, EyeHint2));
        hints.Add((() => runes.HasRune(runeManifest.Yin), ChannelHint));
        hints.Add((() => runes.HasRune(runeManifest.Yang), ChannelHint2));
        hints.Add((() => runes.HasRune(runeManifest.Yin) || runes.HasRune(runeManifest.Yang), BodyMindHint));
        hints.Add((() => runes.HasRune(runeManifest.Yin), FireballHint));
        hints.Add((() => runes.HasRune(runeManifest.Yang), IceballHint));
        hints.Add((() => runes.HasRune(runeManifest.Mind), LunTempleHint));
        hints.Add((() => runes.HasRune(runeManifest.Body), SolTempleHint));

        return hints;
    }

    private bool True() => true;

}