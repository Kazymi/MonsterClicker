using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(TMP_Text))]
public class TextController : MonoBehaviour
{
    private TMP_Text _text;
    
    public void UpdateText(string newText)
    {
        if (_text == null)
        {
            _text = GetComponent<TMP_Text>();
        }
        _text.text = newText;
    }
}