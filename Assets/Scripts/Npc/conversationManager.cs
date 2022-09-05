using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(conversationManager))]
public class NPC_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        conversationManager script = (conversationManager)target;

        script.Moving = EditorGUILayout.Toggle("Moving", script.Moving);
        if (script.Moving)
        {
            script.speed = EditorGUILayout.ObjectField("speed", script.speed, typeof(InputField), true) as InputField;
            script.startPos = EditorGUILayout.ObjectField("start position", script.startPos, typeof(InputField), true) as InputField;
            script.targetPos = EditorGUILayout.ObjectField("target position", script.targetPos, typeof(InputField), true) as InputField;
        }
    }
}
#endif

public class conversationManager : MonoBehaviour
{
    public List<string> npcDialogues = new List<string>();

    public GameObject speechBubble;
    public GameObject playerBody;
    public GameObject joyStick;
    public GameObject textBox;

    [HideInInspector]
    public bool Moving;

    [HideInInspector]
    public InputField speed;
    [HideInInspector]
    public InputField startPos;
    [HideInInspector]
    public InputField targetPos;

    Text textBoxText;
    int CurrentLine = -1;
    int numberOfDialogues = 1;

    bool playerNextTo = false;
    bool firstCall = true;
    bool canEncounter = true;
    bool OnOff = false;

    Vector3 SavedPosition;
    Vector3 NewPosition;


    void Start()
    {
        textBoxText = textBox.GetComponentInChildren<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        speechBubble.SetActive(true);
        playerNextTo = true;
    }

    

    private void OnTriggerExit2D(Collider2D collider)
    {
        speechBubble.SetActive(false);
        canEncounter = true;
        playerNextTo = false;

        NewPosition = Vector3.zero;
        SavedPosition = Vector3.zero;
    }

    void Update()
    {
        if (playerNextTo == true && canEncounter == true)
        {
            onPlayerInteraction();
        }
    }

    void onPlayerInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SavedPosition = playerBody.transform.position;
        }
        if (Input.GetMouseButtonUp(0))
        {
            NewPosition = playerBody.transform.position;
            if(SavedPosition == NewPosition)
            {
                BasicTextProgression();
            }
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                SavedPosition = playerBody.transform.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                NewPosition = playerBody.transform.position;
                if (NewPosition == SavedPosition)
                {
                    BasicTextProgression();    
                }
            }
        }
    }

    public void BasicTextProgression()
    {
        while (firstCall)
        {
            joyStick.SetActive(false);
            speechBubble.SetActive(false);
            textBox.SetActive(true);
            firstCall = false;

            textBoxText.text = "" + npcDialogues[0];
            CurrentLine = 0;

            numberOfDialogues = (npcDialogues.Count - 1);

            return;
        }
        while (!firstCall)
        {
            while (numberOfDialogues != CurrentLine)
            {
                CurrentLine += 1;
                textBoxText.text = "" + npcDialogues[CurrentLine];

                return;
            }
            
            if (numberOfDialogues == CurrentLine)
            {
                textBoxText.text = "";

                canEncounter = false;
                textBox.SetActive(false);
                joyStick.SetActive(true);

                firstCall = true;

                return;
            }
        }
    }
}
