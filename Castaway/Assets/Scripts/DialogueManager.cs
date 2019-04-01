using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
	public GameObject DialogueTextBoxPrefab;

	public void Speak(GameObject speakingCharacter, string text)
	{
		if(text != "")
			StartCoroutine(CreateText(speakingCharacter, text));
	}

	private IEnumerator CreateText(GameObject speakingCharacter, string text)
	{
		Vector3 cPos = speakingCharacter.transform.position;
		Vector3 pos = new Vector3(cPos.x, cPos.y + 4, cPos.z);
		GameObject newText = Instantiate(DialogueTextBoxPrefab, pos, Quaternion.identity);
		newText.GetComponentInChildren<TextMeshProUGUI>().text = text;
		yield return new WaitForSeconds(5.0f);
		Destroy(newText);
	}
}