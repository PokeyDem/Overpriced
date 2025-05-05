using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEmotePresenter
{
    private NpcBehaviour _npcBehaviour;
    private UpdateText _updateText;
    public NPCEmotePresenter(NpcBehaviour npcBehaviour, UpdateText updateText)
    {
        _npcBehaviour = npcBehaviour;
        _updateText = updateText;
        _npcBehaviour.Initialized += OnInitialized;
        _npcBehaviour.DecidingStarted += OnDecidingStarted;
        _npcBehaviour.ItemSelected += OnItemSelected;
        _npcBehaviour.ItemRejected += OnItemRejected;
    }

    private void OnInitialized() 
    {
        _updateText.UpdateTextMesh("",Color.white);
        _updateText.SetActive(true);
    }

    private void OnDecidingStarted() => 
        _updateText.UpdateTextMesh("?",Color.yellow);


    private void OnItemSelected() =>
        _updateText.UpdateTextMesh(":)", Color.green);

    private void OnItemRejected() =>
        _updateText.UpdateTextMesh(":(", Color.red);



}
