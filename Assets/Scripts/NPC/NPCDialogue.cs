using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;

    [TextArea(3, 10)]
    public string[] sentences;
}
