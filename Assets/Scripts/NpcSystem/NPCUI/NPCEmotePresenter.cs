using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEmotePresenter
{
    private IEmotable _context;
    private UpdateEmote _updateText;
    private Sprite _emoteHappy;
    private Sprite _emoteSad;
    private Sprite _emoteThinking;
    private Sprite _emoteAngry;
    private Sprite _emoteAngrier;

    public NPCEmotePresenter(IEmotable context, UpdateEmote updateText)
    {
        _context = context;
        _updateText = updateText;
        _context.MoodChanged += OnMoodChanged;

        _emoteHappy = Resources.Load<Sprite>("UI/Emotes/Emote_Happy");
        _emoteSad = Resources.Load<Sprite>("UI/Emotes/Emote_Sad");
        _emoteThinking = Resources.Load<Sprite>("UI/Emotes/Emote_Thinking");
        _emoteAngry = Resources.Load<Sprite>("UI/Emotes/Emote_Angry");
        _emoteAngrier = Resources.Load<Sprite>("UI/Emotes/Emote_Angrier");
    }

    private void OnMoodChanged(MoodType mood)
    {
        switch(mood)
        {
            case MoodType.None: 
                _updateText.UpdateImageAlpha(0); 
                break;
            case MoodType.Happy:
                _updateText.UpdateImage(_emoteHappy);
                _updateText.UpdateImageAlpha(1);
                break;
            case MoodType.Sad:
                _updateText.UpdateImage(_emoteSad);
                _updateText.UpdateImageAlpha(1);
                break;
            case MoodType.Thinking:
                _updateText.UpdateImage(_emoteThinking);
                _updateText.UpdateImageAlpha(1);
                break;
            case MoodType.Angry:
                _updateText.UpdateImage(_emoteAngry);
                _updateText.UpdateImageAlpha(1);
                break;
            case MoodType.Angrier:
                _updateText.UpdateImage(_emoteAngrier);
                _updateText.UpdateImageAlpha(1);
                break;
        }
    }



}
