using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatUIBoxAutoUpdate : MonoBehaviour 
{
	public GameObject targetCharacter {get;set;}
	public StatName stat {get;set;}

	private EmotionalPersonalityModel epm;
	private PhysicalResourceModel prm;

	void Update () 
	{
		if(targetCharacter != null && stat != StatName.None)
		{
			epm = targetCharacter.GetComponent<EmotionalPersonalityModel>();
			prm = targetCharacter.GetComponent<PhysicalResourceModel>();

			float value = float.MinValue;
			if((int)stat > 0 && (int)stat < Precondition.physicalIndex)
				value = epm.GetEmotionValue(stat.ToString());
			else if((int)stat >= Precondition.physicalIndex && (int)stat < Precondition.limitIndex)
				value = prm.GetPhysicalValue(stat.ToString());

			GetComponent<TextMeshProUGUI>().text = stat.ToString() + " = " + value.ToString("0.00");
		}
	}
}
