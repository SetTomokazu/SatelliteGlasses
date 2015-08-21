using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
	int UpdatePeriod = 20;
	public GameObject Satel;
	public Material theta;
	static AndroidJavaObject m_plugin = null;
	public Canvas canvas;
	public Image Compass;
	private Quaternion compassdst;
	private bool isGpsRunning = false;
	private bool isGetGPS = false;
	private bool isDataViewing = false;

	[SerializeField]
	private DataView dataView;

	private Quaternion Adjust;

	/// <summary>
	/// 受信したGPSデータ.
	/// </summary>
	List<string> lstSatellite = new List<string>();
	/// <summary>
	/// GPSの詳細データ.
	/// </summary>
	[SerializeField]
	private SatelliteData[] data = new SatelliteData[32] 
	{
		new SatelliteData(DateTime.Parse("2011/7/16"),	DateTime.Parse("2011/10/15")),
		new SatelliteData(DateTime.Parse("2004/11/6"),	DateTime.Parse("2004/11/23")),
		new SatelliteData(DateTime.Parse("2014/10/30"),	DateTime.Parse("2014/12/13")),
		new SatelliteData(DateTime.Parse("1993/10/26"),	DateTime.Parse("1993/11/22")),
		new SatelliteData(DateTime.Parse("2009/8/17"),	DateTime.Parse("2009/8/27")),

		new SatelliteData(DateTime.Parse("2014/5/17"),	DateTime.Parse("2014/6/11")),
		new SatelliteData(DateTime.Parse("2008/3/15"),	DateTime.Parse("2008/3/25")),
		new SatelliteData(DateTime.Parse("2015/7/16"),	DateTime.MaxValue),
		new SatelliteData(DateTime.Parse("2014/8/2"),	DateTime.Parse("2014/9/18")),
		null,

		new SatelliteData(DateTime.Parse("1999/10/7"),	DateTime.Parse("2000/1/4")),
		new SatelliteData(DateTime.Parse("2006/11/18"),	DateTime.Parse("2006/12/13")),
		new SatelliteData(DateTime.Parse("1997/7/23"),	DateTime.Parse("1998/1/31")),
		new SatelliteData(DateTime.Parse("2000/11/11"),	DateTime.Parse("2000/12/11")),
		new SatelliteData(DateTime.Parse("2007/10/17"),	DateTime.Parse("2007/11/1")),

		new SatelliteData(DateTime.Parse("2003/1/29"),	DateTime.Parse("2003/2/19")),
		new SatelliteData(DateTime.Parse("2005/9/26"),	DateTime.Parse("2005/12/17")),
		new SatelliteData(DateTime.Parse("2001/1/30"),	DateTime.Parse("2001/2/16")),
		new SatelliteData(DateTime.Parse("2004/3/20"),	DateTime.Parse("2004/4/6")),
		new SatelliteData(DateTime.Parse("2000/5/11"),	DateTime.Parse("2000/6/2")),

		new SatelliteData(DateTime.Parse("2003/3/31"),	DateTime.Parse("2003/4/12")),
		new SatelliteData(DateTime.Parse("2003/12/21"),	DateTime.Parse("2004/1/13")),
		new SatelliteData(DateTime.Parse("2004/6/23"),	DateTime.Parse("2004/7/10")),
		new SatelliteData(DateTime.Parse("2012/10/4"),	DateTime.Parse("2014/11/14")),
		new SatelliteData(DateTime.Parse("2010/5/28"),	DateTime.Parse("2010/8/27")),

		new SatelliteData(DateTime.Parse("2015/3/26"),	DateTime.Parse("2015/4/21")),
		new SatelliteData(DateTime.Parse("2013/5/16"),	DateTime.Parse("2013/6/22")),
		new SatelliteData(DateTime.Parse("2000/7/16"),	DateTime.Parse("2000/8/17")),
		new SatelliteData(DateTime.Parse("2004/12/21"),	DateTime.Parse("2008/1/3")),
		new SatelliteData(DateTime.Parse("2014/2/21"),	DateTime.Parse("2014/5/31")),
		
		new SatelliteData(DateTime.Parse("2006/9/26"),	DateTime.Parse("2006/10/13")),
		new SatelliteData(DateTime.Parse("1990/11/26"),	DateTime.Parse("2008/2/27")),
	};
	/// <summary>
	/// 観測できて作成した人工衛星.
	/// </summary>
	[SerializeField]
	private GameObject[] lst = new GameObject[32];

	int GetSatelliteCount = 0;

	List<Sprite> MapList;
	public GameObject SphereScreen;

	public Material material;
	/// <summary>
	/// 照準のメーター.
	/// </summary>
	public Image meter;
	/// <summary>
	/// 照準を合わせている人工衛星.
	/// </summary>
	public GameObject target;
	public Camera MainCamera;
	/// <summary>
	/// 中心のカメラから常時放出する当たり判定.
	/// </summary>
	Ray ray;
	/// <summary>
	/// 当たり判定対象物.
	/// </summary>
	RaycastHit hit;

	void SetSatellite(string satellite)
	{
		lstSatellite = new List<string>();
		lstSatellite.AddRange(satellite.Split('\n'));
	}

	void Awake()
	{
#if UNITY_ANDROID
		// Android上のUnityPlayerクラスのオブジェクトを取得
		m_plugin = new AndroidJavaObject("com.TTE.satellitelistener.SatelliteListener");
		m_plugin.Call("startService", this.name, "GetSatellite", "GetGPSStatusCallback");
#endif
		Setup();
		Adjust = new Quaternion();
	}



	// Use this for initialization
	void Start()
	{
		Input.compass.enabled = true;
	}

	void FInish()
	{
		Debug.Log("Finish");
#if UNITY_ANDROID
		m_plugin.Call("stopService");
		m_plugin = null;
#endif
		Input.location.Stop();
	}

	void GPSStatusCallback(string text)
	{
		Debug.Log("Unity GPSStatusCallBack" + text);
		if (text.Trim() == "ON") isGpsRunning = true;
	}

	// Update is called once per frame
	void Update()
	{

		ray = new Ray(Vector3.zero, meter.transform.position);
		if (Physics.SphereCast(ray, 20f, out hit))
		{
			if (hit.transform.gameObject == target)
			{

				if (meter.fillAmount < 1)
				{
					meter.fillAmount += Time.deltaTime / 5f;
				}
				else
				{
					if (!isDataViewing)
					{
						dataView.SetData(data[int.Parse(target.name) - 1]);
						isDataViewing = true;
					}
					//
					//TODO:情報取得.
				}
			}
			else
			{
				target = hit.transform.gameObject;
				meter.fillAmount = 0;
				dataView.HideData();
				isDataViewing = false;
			}
		}
		else
		{
			meter.fillAmount = 0;
			target = null;
			dataView.HideData();
			isDataViewing = false;
		}
		compassdst = Quaternion.Euler(0, 0, Input.compass.magneticHeading);
		Compass.rectTransform.localRotation = Quaternion.Lerp(Compass.rectTransform.localRotation, compassdst, 0.1f);

		//GPS取得前なら人工衛星の親の方位を合わせる

		if (!isGetGPS)
		{
			Adjust = Quaternion.Euler(0, -Input.compass.magneticHeading + MainCamera.transform.rotation.eulerAngles.y, 0);
			gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.rotation, Adjust, 0.1f);
		}
		
		if (Input.GetKey(KeyCode.Escape))
		{
			FInish();
			Application.Quit();
		}
	}

	/// <summary>
	/// 人工衛星情報更新.
	/// </summary>
	/// <param name="satellite"></param>
	void GetSatellite(string satellite)
	{
		//Debug.Log(satellite);
		//受信した人工衛星の情報を保存する
		SetSatellite(satellite);
		GetSatelliteCount++;
		if (GetSatelliteCount >= UpdatePeriod)
		{
			UpdateSatellite();
			GetSatelliteCount = 0;
		}
		Debug.Log("GetSatelliteCount" + GetSatelliteCount);
	}
	/// <summary>
	/// 画像読み込み.
	/// </summary>
	public void Setup()
	{
		MapList = new List<Sprite>();
		foreach (string file in Directory.GetFiles(Application.persistentDataPath))
		{
			//JPGもしくはPNGファイルだったら画像取得
			if (Path.GetExtension(file) == ".jpg")
			{
				Debug.Log("Create" + file);
				Sprite newsp = new Sprite();
				using (WWW www = new WWW("file://" + file))
				{
					//画像の中心を基本にSpriteを生成
					newsp = Sprite.Create(www.texture,
										new Rect(0, 0, www.texture.width, www.texture.height),
										new Vector2(0.5f, 0.5f),
										1);//Texture,TextureSize,画像の重心(画像の左下が(0,0)右上が(1,1)),pixelsToUnits.
					
				}
				newsp.name = Path.GetFileNameWithoutExtension(file);
				MapList.Add(newsp);
			}
		}
		//地図が発見できたら
		if (MapList.Count > 0)
		{
			MeshRenderer meshRenderer = SphereScreen.GetComponent<MeshRenderer>();
			Material material =Resources.Load<Material>("theta");
			material.mainTexture = MapList[0].texture;
			meshRenderer.material = material;
		}
		else
		{
		}
	}

	/// <summary>
	/// 人工衛星更新
	/// </summary>
	public void UpdateSatellite()
	{
		foreach (string s in lstSatellite)
		{
			string[] sm = s.Split('\t');
			if (sm.Length != 4)
			{
				continue;
			}
			int prn = int.Parse(sm[0]);
			if (prn > 32)
			{
				continue;
			}
			float snr = float.Parse(sm[1]);
			if (snr <= 1.0f)
			{
				continue;
			}
			float Azimuth = float.Parse(sm[2]) * Mathf.Deg2Rad;
			float Elevation = float.Parse(sm[3]) * Mathf.Deg2Rad;
			//z:北 x:東 y:天頂.
			Vector3 pos = snr * new Vector3(
				Mathf.Sin(Azimuth) * Mathf.Cos(Elevation),
				Mathf.Sin(Elevation),
				Mathf.Cos(Azimuth) * Mathf.Cos(Elevation));


			if (Vector3.Distance(pos, Vector3.zero) < 1.0f)
			{
				continue;
			}

			if (lst[prn - 1])
			{
				lst[prn - 1].GetComponent<Satellite>().SetDst(pos);
				Debug.Log("Update Satellite:" + lst[prn - 1].gameObject.name + lst[prn - 1].transform.localPosition.ToString());
			}
			else
			{
				lst[prn - 1] = Instantiate(Satel) as GameObject;
				lst[prn - 1].transform.SetParent(transform, false);
				lst[prn - 1].GetComponent<Satellite>().SetLocation(pos);
				lst[prn - 1].name = prn.ToString();
				data[prn - 1].prn = prn;
				data[prn - 1].Azimuth = float.Parse(sm[2]);
				data[prn - 1].Elevation = float.Parse(sm[3]);
				data[prn - 1].snr = snr;
				Debug.Log("Create Satellite:" + lst[prn - 1].gameObject.name + lst[prn - 1].transform.localPosition.ToString());
			}
		}
		isGetGPS = true;
		GetSatelliteCount = 0;
		Debug.Log("Finish UpdateSatellite");
	}
}
