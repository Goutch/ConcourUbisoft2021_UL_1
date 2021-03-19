using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private Image char1Slot;
    [SerializeField] private Image char2Slot;
    [SerializeField] private Text textSlot;

    [SerializeField] private Sprite perso1Sprite;
    [SerializeField] private Sprite perso2Sprite;
    [SerializeField] private TextAsset rawText;
    [SerializeField] private char lineSep = '\n';
    [SerializeField] private char itemSep = ',';
    
    private List<string> _character1 = new List<string>();
    private List<string> _character2 = new List<string>();
    private List<string> _line = new List<string>();
    private int _lineCount = 0;
    private int _numLines = 0;
    
    void Awake()
    {
        string text = rawText.ToString();

        string[] lines = text.Split(lineSep);

        for(var i = 0 ; i < lines.Length - 1 ; i++)
        {
            string[] line = lines[i].Split(itemSep);

            _character1.Insert(i, line[0]);
            _character2.Insert(i, line[1]);
            _line.Insert(i, line[2]);
        }

        _numLines = lines.Length - 1;
        
        char2Slot.transform.localRotation = Quaternion.Euler(0,180,0);
        
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (_lineCount == _numLines)
            {
                gameObject.SetActive(false);
            }
            else
            {
                ReadLine();

                _lineCount += 1;
            }
        }
    }

    private void ReadLine()
    {
        // Character 1
        if (_character1[_lineCount] == "0")
        {
            char1Slot.sprite = null;
            char1Slot.color = Color.clear;
        }
        if (_character1[_lineCount] == "1")
        {
            char1Slot.sprite = perso1Sprite;
            char1Slot.color = Color.white;
        }
        if (_character1[_lineCount] == "2")
        {
            char1Slot.sprite = perso2Sprite;
            char1Slot.color = Color.white;
        }
            
        // Character 2
        if (_character2[_lineCount] == "0")
        {
            char2Slot.sprite = null;
            char2Slot.color = Color.clear;
        }
        if (_character2[_lineCount] == "1")
        {
            char2Slot.sprite = perso1Sprite;
            char2Slot.color = Color.white;
        }
        if (_character2[_lineCount] == "2")
        {
            char2Slot.sprite = perso2Sprite;
            char2Slot.color = Color.white;
        }

        textSlot.text = _line[_lineCount];
    }
}
