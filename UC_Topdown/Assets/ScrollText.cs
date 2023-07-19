using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrollText : MonoBehaviour
{
    public float InitialDelay = 0.1f;
    public float CharWait = 0.5f;
    public float CommaWait = 0.3f;
    public float PeriodWait = 0.5f;

    public TextMeshProUGUI textMesh;
	public GameObject textCanvas;
    List<string> text = new List<string>();
	public bool isFinishedReading = false;
	Transform reference;

	public void OpenTextbox() {
		Time.timeScale = 0;
		textCanvas.SetActive(true);
	}

	public void CloseTextbox() {
		Time.timeScale = 1;
		textCanvas.SetActive(false);
	}

	public void StartTextTyping(List<string> _text, Transform _ref) {
		OpenTextbox();
		text = _text;
        textMesh.text = "";
		reference = _ref;
        StartCoroutine(TypeText());
	}

    IEnumerator TypeText()
	{
        yield return new WaitForSecondsRealtime(InitialDelay);
		int i = 0;
		while(i < text.Count) {
				foreach (char c in text[i])
				{
					textMesh.text += c;
					yield return new WaitForSecondsRealtime(CharWait);
					if (c == ',')
						yield return new WaitForSecondsRealtime(CommaWait);
					if (c == '.')
						yield return new WaitForSecondsRealtime(PeriodWait);
				}
				while(!Input.GetButton("Fire1")) {
					yield return null;
				}
				textMesh.text = "";
				i++;
			}
		isFinishedReading = true;
		CloseTextbox();
		reference.SendMessage("EndReading");
    }
}