using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Script", "FungusReset", "ResetSpeak")]
public class FungusReset : Command
{
    public GameObject npcPrefab;

    public override void OnEnter()
    {
        npcPrefab.GetComponent<NPCController>().ResetSpeakAndFungus();
        Continue();
    }
}
