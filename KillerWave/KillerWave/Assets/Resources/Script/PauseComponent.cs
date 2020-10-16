using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class PauseComponent : MonoBehaviour
{
	[SerializeField]
	GameObject pauseScreen;
	[SerializeField]
	AudioMixer masterMixer;
	[SerializeField]
	GameObject musicSlider;
	[SerializeField]
	GameObject effectsSlider;


	void Awake()
	{
		pauseScreen.SetActive(false);
		SetPauseButtonActive(false);
		Invoke("DelayPauseAppear", 5);

		// Setting volume from Player prefs
		masterMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVolume"));
		masterMixer.SetFloat("effectsVol", PlayerPrefs.GetFloat("effectsVolume"));
		musicSlider.GetComponent<Slider>().value = GetMusicLevelFromMixer();
		effectsSlider.GetComponent<Slider>().value = GetEffectsLevelFromMixer();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// <para>Makes the Pause button appear</para>
	/// </summary>
	/// <param name="switchButton"></param>
	void SetPauseButtonActive(bool switchButton)
	{
		ColorBlock col = GetComponentInChildren<Toggle>().colors;
		// Disabling the colors when the button is off
		if (switchButton == false)
		{
			col.normalColor = new Color32(0, 0, 0, 0);
			col.highlightedColor = new Color32(0, 0, 0, 0);
			col.pressedColor = new Color32(0, 0, 0, 0);
			col.disabledColor = new Color32(0, 0, 0, 0);
			GetComponentInChildren<Toggle>().interactable = false;
		}
		else
		{
			col.normalColor = new Color32(245, 245, 245, 255);
			col.highlightedColor = new Color32(245, 245, 245, 255);
			col.pressedColor = new Color32(200, 200, 200, 255);
			col.disabledColor = new Color32(200, 200, 200, 128);
			GetComponentInChildren<Toggle>().interactable = true;
		}

		// Reapplying the color
		GetComponentInChildren<Toggle>().colors = col;
		GetComponentInChildren<Toggle>().transform.GetChild(0).GetChild(0).gameObject.SetActive(switchButton);

	}


	/// <summary>
	/// <para>A delay before the Pause button appears</para>
	/// </summary>
	void DelayPauseAppear()
	{
		SetPauseButtonActive(true);
	}

	/// <summary>
	/// <para>Pauses the game</para>
	/// </summary>
	public void PauseGame()
	{
		pauseScreen.SetActive(true);
		SetPauseButtonActive(false);
		Time.timeScale = 0;
	}

	/// <summary>
	/// <para>Resumes the game</para>
	/// </summary>
	public void Resume()
	{
		pauseScreen.SetActive(false);
		SetPauseButtonActive(true);
		Time.timeScale = 1;
	}

	public void Quit()
	{
		Time.timeScale = 1;
		GameManager.Instance.GetComponent<ScoreManager>().ResetScore();
		GameManager.Instance.GetComponent<ScenesManager>().BeginGame(0);
	}


	/// <summary>
	/// <para>Sets the volume for the music</para>
	/// </summary>
	public void SetMusicLevelFromSlider()
	{
		masterMixer.SetFloat("musicVol", musicSlider.GetComponent<Slider>().value);
		PlayerPrefs.SetFloat("musicVolume", musicSlider.GetComponent<Slider>().value);
	}

	/// <summary>
	/// <para>Sets the volume for the effects</para>
	/// </summary>
	public void SetEffectsLevelFromSlider()
	{
		masterMixer.SetFloat("effectsVol", effectsSlider.GetComponent<Slider>().value);
		PlayerPrefs.SetFloat("effectsVolume",effectsSlider.GetComponent<Slider>().value);
	}

	/// <summary>
	/// <para>Returns the current volume of the mixer</para>
	/// </summary>
	/// <returns></returns>
	float GetMusicLevelFromMixer()
	{
		float musicMixersValue;
		bool result = masterMixer.GetFloat("musicVol", out musicMixersValue);
		if (result)
		{
			return musicMixersValue;
		}
		else
		{
			return 0;
		}
	}
	/// <summary>
	/// <para>Returns the current volume of the effects</para>
	/// </summary>
	/// <returns></returns>
	float GetEffectsLevelFromMixer()
	{
		float effectsMixersValue;
		bool result = masterMixer.GetFloat("effectsVol", out effectsMixersValue);
		if (result)
		{
			return effectsMixersValue;
		}
		else
		{
			return 0;
		}
	}


}
