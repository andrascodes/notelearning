using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SocketIOScript : MonoBehaviour
{
    private SocketIOComponent socket;
    private static Color GREEN = new Color(0f, 1f, 0.07796383f, 0.55f);
    private static Color RED = new Color(1f, 0f, 0f, 0.55f);
    private static Color WHITE = new Color(1f, 1f, 1f, 0.1f);
    private static Color BLACK = new Color(0,0,0, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("press", HandleKeyPress);
        socket.On("prompt", HandlePromptChange);
    }

    public void HandlePromptChange(SocketIOEvent e) 
    {
        // what to show on the prompt
        // prompt { text: "C" }
        GameObject.Find("Note Prompt").GetComponent<TextMesh>().text = e.data.GetField("text").str;
    }

    public void HandleKeyPress(SocketIOEvent e) 
    {
        // pressed key, wrong or right
        // if wrong: right key? "press" { right: 'C', wrong: 'A' }

        string rightKey = e.data.GetField("right").str;
        string wrongKey = e.data.GetField("wrong").str;
        string streak = e.data.GetField("streak").str;
        GameObject.Find("Streak").GetComponent<TextMesh>().text = "Streak: " + streak;

        if (wrongKey == null)
        {
            string[] keys = { rightKey };
            GameObject.Find("Key" + rightKey).GetComponent<Renderer>().material.color = GREEN;
            StartCoroutine("SwitchKeysBack", keys);
        }
        else
        {
            string[] keys = { rightKey, wrongKey };
            GameObject.Find("Key" + rightKey).GetComponent<Renderer>().material.color = GREEN;
            GameObject.Find("Key" + wrongKey).GetComponent<Renderer>().material.color = RED;
            StartCoroutine("SwitchKeysBack", keys);
        }
    }

    private IEnumerator SwitchKeysBack(string[] keys)
    {
        // wait 1 seconds and continue
        yield return new WaitForSeconds(3);

        foreach(string key in keys)
        {
            if (key.Contains("#"))
            {
                GameObject.Find("Key" + key).GetComponent<Renderer>().material.color = BLACK;
            }
            else
            {
                GameObject.Find("Key" + key).GetComponent<Renderer>().material.color = WHITE;
            }
        }

    }
}
