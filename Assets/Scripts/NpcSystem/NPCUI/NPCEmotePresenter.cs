using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEmotePresenter
{
    private NpcBehaviour _npcBehaviour;
    private UpdateText _updateText;
    private Sprite _emoteHappy;
    private Sprite _emoteSad;
    private Sprite _emoteThinking;

    public NPCEmotePresenter(NpcBehaviour npcBehaviour, UpdateText updateText)
    {
        _npcBehaviour = npcBehaviour;
        _updateText = updateText;
        _npcBehaviour.Initialized += OnInitialized;
        _npcBehaviour.DecidingStarted += OnDecidingStarted;
        _npcBehaviour.ItemSelected += OnItemSelected;
        _npcBehaviour.ItemRejected += OnItemRejected;
        _emoteHappy = Resources.Load<Sprite>("UI/Emotes/Emote_Happy");
        _emoteSad = Resources.Load<Sprite>("UI/Emotes/Emote_Sad");
        _emoteThinking = Resources.Load<Sprite>("UI/Emotes/Emote_Thinking");
    }

    private void OnInitialized()
    {
        _updateText.UpdateImageAlpha(0);
    }

    private void OnDecidingStarted()
        {
            _updateText.UpdateImage(_emoteThinking);
            _updateText.UpdateImageAlpha(1);
        }
        


    private void OnItemSelected()
    {
        _updateText.UpdateImage(_emoteHappy);
        _updateText.UpdateImageAlpha(1);
    }
     

    private void OnItemRejected() =>
        _updateText.UpdateImage(_emoteSad);



}
