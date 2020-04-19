using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public Text dialog;
    public bool isVisible = false;
    public Camera cam;
    int currentScene;

    public string[] sentences;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        dialog = GameObject.FindWithTag("Text").GetComponent<Text>();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(TypeSentence(sentences[currentScene - 1]));
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            dialog.transform.position = cam.WorldToScreenPoint(transform.position);
        }
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialog.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialog.text += letter;
            yield return new WaitForSeconds(1/10f);
        }
    }
}
