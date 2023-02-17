using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Sirenix.OdinInspector;

namespace Voidless
{
public class TEST_DownloadDataFromWeb : MonoBehaviour
{
	[SerializeField] private string address; 	/// <summary>Web's Address.</summary>

	[Button("Test Download")]
	/// <summary>Downloads Data.</summary>
	private void Download()
	{
		if(string.IsNullOrEmpty(address)) return;

		WebClient client = new WebClient();
		client.DownloadFile(address, Application.dataPath + "/Downloaded Data/TestData.txt");
	}
}
}