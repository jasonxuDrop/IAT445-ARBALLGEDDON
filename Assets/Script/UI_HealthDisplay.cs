using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_HealthDisplay : MonoBehaviour
{
    [HideInInspector] public HitPoint hitPointToTrack;

	public Transform[] hitPointImages;

	public void Update() {

		if (hitPointToTrack && hitPointImages.Length > 0)
		{
			int barCount = Mathf.FloorToInt(((float)hitPointToTrack.hitPoint / (float)hitPointToTrack.hitPointMax) * hitPointImages.Length + 0.1f);
			//Debug.Log("barCount: " + barCount + ", (f) (h/Max)" + ((float) hitPointToTrack.hitPoint / (float) hitPointToTrack.hitPointMax) + ", whole thing without + .1: " + ((float)(hitPointToTrack.hitPoint / hitPointToTrack.hitPointMax) * hitPointImages.Length));
			for (int i = 0; i < hitPointImages.Length; i++)
			{
				hitPointImages[i].gameObject.SetActive(i < barCount);
			}
		}
		else
		{
			Debug.LogWarning("hitPointToTrack never assigned to this UI_HealthDisplay");
		}

	}
}
