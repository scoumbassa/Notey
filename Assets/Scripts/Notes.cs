using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notes : MonoBehaviour {

    private Vector2 size;
    public string text;
    public Vector3 position;
    InputField inputField;
    string header;
    
    private void Update()
    {
        position = Camera.main.WorldToScreenPoint(transform.position);
        position.y = Screen.height - position.y;
        
    }

    private void Start()
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();

        //listening for text submission
        inputField.onEndEdit.AddListener(delegate {
                                            UpdateText(inputField);
                                            RectTransform rect = inputField.GetComponent<RectTransform>();
                                            rect.anchoredPosition = new Vector3(5000f, 0f, 0f);
        });
        header = text;
    }

    /// <summary>
    /// Updates text of the selected note.
    /// </summary>
    /// <param name="input"></param>
    private void UpdateText(InputField input)
    {
        string activeNote = GameObject.Find("UserDefinedTarget").GetComponent<UDTManager>().activeNote;
        if(activeNote != null)
            GameObject.Find(activeNote).GetComponentInChildren<Notes>().text = header + "\n" + input.text;
    }

    /// <summary>
    /// Sets style of the GUI element (GUI box in this case).
    /// </summary>
    public GUIStyle style = new GUIStyle();
    public GUIStyle del = new GUIStyle();
    public void SetStyle()
    {
        Texture2D whiteTex = new Texture2D(1, 1);
        whiteTex.SetPixel(0, 0, Color.white);

        style.alignment = TextAnchor.UpperLeft;
        style.normal.background = whiteTex;
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.fixedHeight = 0;
        style.fixedWidth = 300;
        style.padding = new RectOffset(6, 6, 5, 5);
        style.wordWrap = true;

        del.alignment = TextAnchor.MiddleCenter;
        del.normal.background = whiteTex;
        del.fontSize = 30;
        del.fontStyle = FontStyle.Bold;
        del.fixedHeight = 80;
        del.fixedWidth = 80;

        LabelSize();
    }

    /// <summary>
    /// Calculates label size.
    /// </summary>
    private void LabelSize()
    {
        size = style.CalcSize(new GUIContent(text));
    }

    private void OnGUI()
    {
        SetStyle();
        
        float width = 300;
        float height = 300;

        if (GUI.Button(new Rect(position.x - 0.5f * width, position.y - 0.5f * height, width, height), ""))
        {
            //open text input field
            RectTransform rect = inputField.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(0f, -300f, 1f);
            inputField.ActivateInputField();
            inputField.Select();
            inputField.MoveTextEnd(true);
            GameObject.Find("UserDefinedTarget").GetComponent<UDTManager>().activeNote = transform.parent.name;
        }
        GUI.backgroundColor = new Color32(255, 255, 153, 255);
        GUI.Box(new Rect(position.x - 0.5f*width, position.y - 0.5f*height, width, height), text, style);

        GUI.backgroundColor = Color.red;
        float delWidth = 80;
        float delHeight = 80;
        if (GUI.Button(new Rect(position.x + delWidth, position.y - height + delHeight, width, height), "DEL", del))
        {
            string name = transform.parent.name;

            Destroy(transform.parent.gameObject);
            //GameObject.Find("UserDefinedTarget").GetComponent<UDTManager>().DestroyObject(GameObject.Find(name));
        }
    }

    
}
