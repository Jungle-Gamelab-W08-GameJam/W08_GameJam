using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    float time;
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time < 0.5f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - time);
        }
        else
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, time);
            if (time > 1f) time = 0;
        }
    }
}
