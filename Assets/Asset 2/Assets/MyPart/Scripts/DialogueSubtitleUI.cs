using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueSubtitleUI : MonoBehaviour
{

    [TextArea(3,4)]
    public string DEBUG_Dialogue;
    public string DEBUG_SpeakerName;
    public float DEBUG_Timer = 4;
    [Space]
    public Animator dialogueAnimator;
    public Text Label_DialogueContent;
    public Text Label_SpeakerName;

    public static DialogueSubtitleUI instance;

    private float timer = 2f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Close();
        timer = 0f;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

        }
        else
        {
            Close();
        }
    }

    [ContextMenu("Test1")]
    public void Test1()
    {
        Open(DEBUG_Dialogue, DEBUG_SpeakerName, DEBUG_Timer);
    }

    public void Open(string dialogue, string speakerName, float timer1)
    {
        dialogueAnimator.SetBool("Close", false);
        Label_DialogueContent.text = dialogue;
        Label_SpeakerName.text = speakerName;
        timer = timer1;
    }

    public void Close()
    {
        dialogueAnimator.SetBool("Close", true);
    }

}
