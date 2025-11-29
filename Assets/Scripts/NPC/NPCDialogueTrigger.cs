using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    public NPCDialogue dialogue;
    private bool playerInRange;

    private void Awake()
    {
        if (visualCue != null)
            visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.StartDialogue(dialogue);
            if (visualCue != null) 
                visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
        if (visualCue != null) 
            visualCue.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
        if (visualCue != null) 
            visualCue.SetActive(false);
    }
}
