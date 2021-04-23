using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Music : MonoBehaviour {
	//audio
	public AudioMixerGroup audioMixer = null;
	public Slider slider = null;
	public Text tip = null;
	public List<AudioClip> mainClipArr = new List<AudioClip>();
	public List<AudioClip> wingClipArr = new List<AudioClip>();
	private List<AudioSource> mainSourceArr = new List<AudioSource>();
	private List<AudioSource> wingSourceArr = new List<AudioSource>();

	//wing-datawe
	private List<string> wingKeyArr = new List<string>();
	private List<float> wingvolumeArr = new List<float>();
	private int wingTime = 30;
	private float wingPitch = 1.0f;	

	//main-data
	private List<string> mainKeyArr = new List<string>();
	private List<float> mainvolumeArr = new List<float>();
	private List<int> mainTimeArr = new List<int>();
	private float mainPitch = 1.0f;
	private int mainSourceNum = 0;

	//state
	private int time = 0;
	private int timeMain = 0;
	private int timeWing = 0;
	private int indexMain = 0;
	private int indexWing = 0;
	private bool isPlay = false;
	private bool isAuto = true;
	private bool isSlider = false; 
	private bool isEnd = false; 
	private int fps = 80;
	void Start () {
		foreach(AudioClip mainClip in mainClipArr){
			AudioSource mainSource = gameObject.AddComponent<AudioSource>() as AudioSource;
			mainSource.clip = mainClip;
			mainSource.outputAudioMixerGroup = audioMixer;
			mainSourceArr.Add (mainSource);
		}
		mainSourceNum = mainSourceArr.Count;

		foreach(AudioClip wingClip in wingClipArr){
			AudioSource wingSource = gameObject.AddComponent<AudioSource>() as AudioSource;
			wingSource.clip = wingClip;
			wingSource.outputAudioMixerGroup = audioMixer;
			wingSourceArr.Add (wingSource);
		}
	}

	//Update
	void LeftTimer(){
		string[] strArr = wingKeyArr [indexWing].Split (',');
		foreach (string str in strArr){
			if (str != "-1") {
				int i = int.Parse(str);
				wingSourceArr [i].volume = wingvolumeArr [indexWing];
				wingSourceArr [i].pitch = wingPitch;
				wingSourceArr [i].Stop ();
				wingSourceArr [i].Play ();
			}
		}
		indexWing++;
		if (indexWing == wingKeyArr.Count) {
			indexWing = 0;
		}
	}
	void RightTimer(){
		string[] strArr = mainKeyArr [indexMain].Split (',');
		foreach (string str in strArr){
			if (str != "-1") {
				int i = int.Parse(str);
				mainSourceArr [i].volume = mainvolumeArr [indexMain];
				mainSourceArr [i].pitch = mainPitch;
				mainSourceArr [i].Stop ();
				mainSourceArr [i].Play ();
			}
		}
		indexMain++;
		if (indexMain == mainKeyArr.Count) {
			indexMain = 0;
		}
	}
	IEnumerator Wait()
	{
		isPlay = false;
		isEnd = true;
		int t = 10;
		while(true){
			tip.text = t+"s后自动切换";
			yield return new WaitForSeconds(1.0f);
			t--;
			if (t == 0)
				break;
		}
		tip.text = "";
		isPlay = true;
		isEnd = false;
		Next ();
	}
	void FixedUpdate ()
	{
		if (isPlay && !isSlider) {
			time += 1;
			if (slider!=null) slider.value = ((float)time)/(fps*60f);
			if (time >= fps*60) {
				if (isAuto) {
					StartCoroutine("Wait");
				}
				else {
					time = 0;
					timeWing = 0;
					timeMain = 0;
					indexWing = 0;
					indexMain = 0;
				}
			}
			if (time <= fps * 56) {
				timeWing++;
				if (timeWing % wingTime == 0) {
					LeftTimer ();
					timeWing = 0;
				}
			}
			timeMain++;
			if (timeMain % mainTimeArr[indexMain] == 0) {
				RightTimer ();
				timeMain = 0;
			}
		}
	}

	//Loading
	private int randomSpeed(int speed){
		switch(speed){
		case 0:
			{
				switch (Random.Range (0, 7)) {
				case 0:
					return fps/2;
				case 1:
					return fps/2;
				case 2:
					return fps/2;
				case 3:
					return fps/4*3;
				case 4:
					return fps/4*3;
				case 5:
					return fps/4;
				case 6:
					return fps/4;
				}
				break;
			}
		case 1:
			{
				switch (Random.Range (0, 7)) {
				case 0:
					return fps/2;
				case 1:
					return fps/2;
				case 2:
					return fps/2;
				case 3:
					return fps/4;
				case 4:
					return fps/4;
				case 5:
					return fps/4;
				case 6:
					return fps/8;
				}
				break;
			}
		case 2:
			{
				switch (Random.Range (0, 5)) {
				case 0:
					return fps/4;
				case 1:
					return fps/2;
				case 2:
					return fps/4*3;
				case 3:
					return fps/4*3;
				case 4:
					return fps;
				}
				break;
			}
		}
		return fps/2;
	}
	private string[] RandomWingKey(){
		string[] strArr;
		switch(Random.Range(0,9)){
		case 0:
			strArr = new string[] {"0", "4", "7", "-1",
				"1", "5", "8", "-1",
				"2", "6", "9", "-1",
				"9", "6", "-1", "6"
			};
			wingTime = fps/2;
			break;
		case 1:
			strArr = new string[]{
				"0","4","7","8","9","-1","-1","-1",
				"1","5","8","9","10","-1","-1","-1",
				"2","6","9","10","11","-1","-1","-1",
				"2","6","9","10","11","6","9","5"};
			wingTime = fps/4;
			break;
		case 2:
			strArr = new string[]{
				"2","6","9","11","-1","-1","7","-1",
				"1","5","8","10","-1","-1","5","-1",
				"0","4","7","9","-1","-1","4","-1",
				"-1","1","5","8","10","5","8","10"};
			wingTime = fps/4;
			break;
		case 3:
			strArr = new string[]{
				"0","4","7","8","9","-1","-1","-1",
				"1","5","8","9","10","-1","-1","-1",
				"4","8","11","13","14","-1","-1","-1",
				"-1","8","4","8","11","12","10","11"};
			wingTime = fps/4;
			break;
		case 4:
			strArr = new string[]{
				"2","6","9","11","-1","6","9","11",
				"0","4","7","9","-1","4","7","9",
				"1","5","8","10","-1","5","8","10",
				"4","8","11","14","-1","8","13","11"};
			wingTime = fps/4;
			break;
		case 5:
			strArr = new string[]{
				"2","9,12","-1","6","9,12","6","2","9,12",
				"0","9,12","-1","4","9,12","4","0","9,12",
				"4","12,14","-1","8","12,14","8","4","12,14",
				"1","8,10","-1","5","8,10","5","1","8,10"};
			wingTime = fps/4;
			break;
		case 6:
			strArr = new string[]{
				"4","8","12","8","10","8","10","12",
				"2","6","12","6","10","6","10","12",
				"0","4","12","4","10","4","10","12",
				"1","5","12","5","10","5","10","12"};
			wingTime = fps/4;
			break;
		case 7:
			strArr = new string[]{
				"7,9,12","-1","-1","-1",
				"8,10,13","-1","-1","-1",
				"9,12,14","-1","-1","-1",
				"-1","-1","13","-1",
				"7,9,12","-1","-1","-1",
				"8,10,13","-1","-1","-1",
				"4,8,11","-1","-1","-1",
				"-1","-1","13","-1"};
			wingTime = fps/4;
			break;
		case 8:
			strArr = new string[]{
				"7","9,12","9,12","9,12",
				"8","10,13","10,13","10,13",
				"9","12,14","12,14","12,14",
				"8","10,13","-1","13"};
			wingTime = fps/2;
			break;
		default:
			strArr = new string[] {"-1"};
			break;
		}
		return strArr;
	}
	private void NewMusic(){
		//sum
		mainPitch = Random.Range (0.8f,1.2f);
		wingPitch = mainPitch*2;
		int i;
		int t_time,s_time;
		float volume;
		//main-data
		mainKeyArr.Clear ();
		mainTimeArr.Clear ();
		mainvolumeArr.Clear ();
		int speed = Random.Range (0,3);
		bool isDouble;
		if (Random.Range (0.0f,1.0f)<=0.5f) {
			isDouble = true;
		}
		else {
			isDouble = false;
		}
		s_time = 0;
		i = Random.Range (0,mainSourceNum-1);
		//begintime
		mainTimeArr.Add (8*fps);
		mainKeyArr.Add ((-1).ToString ());
		s_time += 8*fps;
		while (true) {
			//time
			t_time = randomSpeed (speed);
			mainTimeArr.Add (t_time);
			s_time += t_time;
			//index
			switch (Random.Range (0,7)) {
			case 0:
				break;
			case 1:
				i -= 1;
				break;
			case 2:
				i -= 2;
				break;
			case 3:
				i -= 3;
				break;
			case 4:
				i += 1;
				break;
			case 5:
				i += 2;
				break;
			case 6:
				i += 3;
				break;
			}
			if (i < 0)
				i = 0+Random.Range(0,5);
			if (i > mainSourceNum-1)
				i = mainSourceNum-1-Random.Range(0,5);
			if (isDouble) {
				switch (Random.Range (0, 6)) {
				case 0:
					mainKeyArr.Add (i.ToString ());
					break;
				case 1:
					if (i + 2 > mainSourceNum - 1)
						i = i - 2;
					mainKeyArr.Add (i.ToString () + "," + (i + 2).ToString ());
					break;
				case 2:
					if (i - 2 < 0)
						i = i + 2;
					mainKeyArr.Add (i.ToString () + "," + (i - 2).ToString ());
					break;
				case 3:
					if (i + 5 > mainSourceNum - 1)
						i = i - 5;
					mainKeyArr.Add (i.ToString () + "," + (i + 5).ToString ());
					break;
				case 4:
					if (i - 5 < 0)
						i = i + 5;
					mainKeyArr.Add (i.ToString () + "," + (i - 5).ToString ());
					break;
				case 5:
					mainKeyArr.Add (i.ToString ());
					break;
				}
			}
			else {
				mainKeyArr.Add (i.ToString ());
			}
			//end
			if (s_time >= fps * 60 - fps*2) {
				t_time = fps * 60 - s_time;
				s_time += t_time;
				mainTimeArr.Add (t_time);
				mainKeyArr.Add ((-1).ToString ());	
			}
			if (s_time >= fps * 60) {
				break;
			}
		}
		foreach (string str in mainKeyArr) {
			volume = Random.Range (0.8f,1.0f);
			mainvolumeArr.Add (volume);
		}

		//wing-data
		wingKeyArr.Clear ();
		wingvolumeArr.Clear ();
		s_time = 0;
		//key & Time
		wingKeyArr.AddRange(RandomWingKey());
		//volume
		volume = Random.Range (0.3f, 0.5f);
		foreach (string str in wingKeyArr) {
			wingvolumeArr.Add (volume);
		}

		//state
		timeMain = 0;
		timeWing = 0;
		indexMain = 0;
		indexWing = 0;
		time = 0;
		slider.value = 0;
	}
	//API
	public void ChangeAuto(GameObject obj=null){
		if (isAuto) {
			isAuto = false;
			obj.GetComponentInChildren<Text> ().text = "单曲循环";
			if (isEnd) {
				tip.text = "";
				StopCoroutine("Wait");
				isPlay = true;
				isEnd = false;
				Next ();
			}
		}
		else{
			isAuto = true;
			obj.GetComponentInChildren<Text> ().text = "随机播放";
		}
	}
	private void Clear(){
		foreach (AudioSource wingSource in wingSourceArr) {
			wingSource.Stop();
		}
		foreach (AudioSource mainSource in mainSourceArr) {
			mainSource.Stop();
		}
	}
	public void Next(){
		if (isEnd) {
			StopCoroutine("Wait");
			tip.text = "";
			isPlay = true;
			isEnd = false;
		}
		if (isPlay) {
			//Clear
			Clear();
			//New
			NewMusic ();
			isPlay = true;
		}
	}
	public void Change(GameObject obj){
		if (obj.GetComponentInChildren<Text> ().text=="开始播放") {
			NewMusic ();
			isPlay = true;
			if(obj!=null) obj.GetComponentInChildren<Text> ().text = "停止播放";
		}
		else {
			if (isEnd) {
				StopCoroutine("Wait");
				tip.text = "";
				isEnd = false;
			}
			isPlay = false;
			if(obj!=null) obj.GetComponentInChildren<Text> ().text = "开始播放";
			//clear
			Clear();
			slider.value = 0;
		}
	}

	public void ToSliderBegin(){
		if (isPlay) {
			Clear ();
			isSlider = true;
		}
	}
	public void ToSliderEnd(){
		if (isEnd) {
			StopCoroutine("Wait");
			tip.text = "";
			isPlay = true;
			isEnd = false;
			isSlider = true;
		}
		if (isPlay) {
			if (slider.value == 0) {
				time = 0;
				timeWing = 0;
				timeMain = 0;
				indexWing = 0;
				indexMain = 0;
			} else {
				float value = slider.value;
				time = (int)(fps * 60 * value);
				//main
				timeMain = 0;
				indexMain = 0;
				int timeMainSum = 0;
				foreach (int mainTime in mainTimeArr) {
					if (timeMainSum + mainTime < time) {
						indexMain++;
						timeMainSum += mainTime;
					} else if (timeMainSum + mainTime == time) {
						if (time != fps * 60) {
							RightTimer ();
							timeMain = 0;
						}
						break;
					} else {
						break;
					}
				}
				//wing
				indexWing = (time / wingTime) % wingKeyArr.Count;
				timeWing = time % wingTime;
			}
			isSlider = false;
		}
	}
}
