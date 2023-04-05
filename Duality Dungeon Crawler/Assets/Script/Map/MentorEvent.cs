using CaptainCoder.Core;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MentorEvent", menuName = "BodyMind/Events/Mentor Event")]
public class MentorEvent : MapEvent
{
    private bool _isFirst;
    private void OnEnable()
    {
        _isFirst = true;
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

    public override bool OnInteract()
    {
        First.Display();
        return false;
    }

    private DialogChain First =
        DialogChain.Dialog(@"
        The necromancer speaks to you:
        Heed my word, apprentice. As above, so below. The one above all is the Sun, and the one beneath all is the Moon. 
        Maintain your body and mind against those that wish to destroy you.
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
    
    private DialogChain Unbalanced =
        DialogChain.Dialog(@"
        Remember balance in equity is key to maintaining the dualities of self. 
        Fail to do so, and you shall start where we meet. 
        You are unbalanced, take this Balance rune to begin this journey in duality. 
        ");
    
}