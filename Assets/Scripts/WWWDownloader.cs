using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WWWDownloader : MonoBehaviour
{
	private const string GOOGLE_DRIVE_DOMAIN = "drive.google.com";
	private const string GOOGLE_DRIVE_DOMAIN2 = "https://drive.google.com";
	private const int GOOGLE_DRIVE_MAX_DOWNLOAD_ATTEMPT = 3;

	private readonly UnityWebRequest webRequest;
	//private readonly DownloadProgress downloadProgress;

	private Uri downloadAddress;
	private string downloadPath;

	private bool asyncDownload;
	private object userToken;

	private bool downloadingDriveFile;
	private int driveDownloadAttempt;

	public void DownloadFile(string address, string fileName)
	{
		downloadingDriveFile = address.StartsWith(GOOGLE_DRIVE_DOMAIN) || address.StartsWith(GOOGLE_DRIVE_DOMAIN2);
		if (downloadingDriveFile)
		{
			address = GetGoogleDriveDownloadAddress(address);
			driveDownloadAttempt = 1;

		}

		downloadAddress = new Uri(address);
		downloadPath = Path.Combine(Application.persistentDataPath, fileName);

		StartCoroutine(FirstDownloadFileInternal());
	}

	private IEnumerator FirstDownloadFileInternal()
	{
		Debug.Log("Comienza la primera descarga " + downloadAddress + " en " + downloadPath);
		UnityWebRequest www = new UnityWebRequest(downloadAddress);
		www.downloadHandler = new DownloadHandlerBuffer();
		yield return www.SendWebRequest();
		Debug.Log(www.downloadHandler.text);
		//debugText.text = www.downloadHandler.text;
		if (www.error != null)
		{
			Debug.Log(www.error);
			//debugText.text = www.error;
		}

		// Or retrieve results as binary data
		byte[] results = www.downloadHandler.data;
		System.IO.File.WriteAllBytes(downloadPath, results);
        if (!ProcessDriveDownload())
        {
			StartCoroutine(SecondDownloadFileInternal());
        }
		
		//Now Save it
		//System.IO.File.WriteAllBytes(savePath, results);
	}

	private IEnumerator SecondDownloadFileInternal()
	{
		Debug.Log("Comienza la segunda descarga " + downloadAddress);
		UnityWebRequest www = new UnityWebRequest(downloadAddress);
		www.downloadHandler = new DownloadHandlerBuffer();
		yield return www.SendWebRequest();
		//Debug.Log(www.downloadHandler.text);
		//debugText.text = www.downloadHandler.text;
		if (www.error != null)
		{
			Debug.Log(www.error);
			//debugText.text = www.error;
		}

		// Or retrieve results as binary data
		byte[] results = www.downloadHandler.data;
		System.IO.File.WriteAllBytes(downloadPath, results);

	}


	// Downloading large files from Google Drive prompts a warning screen and requires manual confirmation
	// Consider that case and try to confirm the download automatically if warning prompt occurs
	// Returns true, if no more download requests are necessary
	private bool ProcessDriveDownload()
	{
		FileInfo downloadedFile = new FileInfo(downloadPath);
		if (downloadedFile == null)
			return true;

		// Confirmation page is around 50KB, shouldn't be larger than 60KB
		if (downloadedFile.Length > 60000L)
			return true;

		// Downloaded file might be the confirmation page, check it
		string content;
		using (var reader = downloadedFile.OpenText())
		{
			// Confirmation page starts with <!DOCTYPE html>, which can be preceeded by a newline
			char[] header = new char[20];
			int readCount = reader.ReadBlock(header, 0, 20);
			if (readCount < 20 || !(new string(header).Contains("<!DOCTYPE html>")))
				return true;

			content = reader.ReadToEnd();
		}

		int linkIndex = content.LastIndexOf("href=\"/uc?");
		if (linkIndex < 0)
			return true;

		linkIndex += 6;
		int linkEnd = content.IndexOf('"', linkIndex);
		if (linkEnd < 0)
			return true;

		//downloadedFile.Delete();
		downloadAddress = new Uri("https://drive.google.com" + content.Substring(linkIndex, linkEnd - linkIndex).Replace("&amp;", "&"));
		return false;
	}

	// Handles the following formats (links can be preceeded by https://):
	// - drive.google.com/open?id=FILEID
	// - drive.google.com/file/d/FILEID/view?usp=sharing
	// - drive.google.com/uc?id=FILEID&export=download
	public string GetGoogleDriveDownloadAddress(string address)
	{
		int index = address.IndexOf("id=");
		int closingIndex;
		if (index > 0)
		{
			index += 3;
			closingIndex = address.IndexOf('&', index);
			if (closingIndex < 0)
				closingIndex = address.Length;
		}
		else
		{
			index = address.IndexOf("file/d/");
			if (index < 0) // address is not in any of the supported forms
				return string.Empty;

			index += 7;

			closingIndex = address.IndexOf('/', index);
			if (closingIndex < 0)
			{
				closingIndex = address.IndexOf('?', index);
				if (closingIndex < 0)
					closingIndex = address.Length;
			}
		}
		MonoBehaviour.print(string.Concat("https://drive.google.com/uc?id=", address.Substring(index, closingIndex - index), "&export=download"));
		return string.Concat("https://drive.google.com/uc?id=", address.Substring(index, closingIndex - index), "&export=download");
	}

}