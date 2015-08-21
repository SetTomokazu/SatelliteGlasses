using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataView : MonoBehaviour {
	[SerializeField]
	private Image mask;
	[SerializeField]
	private Text txtPrn;
	[SerializeField]
	private Text txtLaunch;
	[SerializeField]
	private Text txtOperation;
	[SerializeField]
	private Text txtAzimuth;
	[SerializeField]
	private Text txtElevation;
	[SerializeField]
	private Text txtSnr;

	private string strPrn = "PRN：";
	private string strLaunch = "運用開始：\n";
	private string strOperation = "打ち上げ：\n";
	private string strAzimuth = "方位：";
	private string strElevation = "仰角：";
	private string strSnr = "SNR：";

	public void SetData(SatelliteData newdata)
	{
		txtPrn.text = strPrn + newdata.prn.ToString();
		txtLaunch.text = strLaunch + newdata.Launch.ToString("yyyy/MM/dd");
		txtOperation.text = strOperation + newdata.Operation.ToString("yyyy/MM/dd");
		txtAzimuth.text = strAzimuth + newdata.Azimuth.ToString("0.0")+"°";
		txtElevation.text = strElevation + newdata.Elevation.ToString("0.0") + "°";
		txtSnr.text = strSnr + newdata.snr.ToString("0.00");
		mask.rectTransform.sizeDelta = Vector2.one * 1000;
	}

	public void HideData()
	{
		mask.rectTransform.sizeDelta = Vector2.zero;
	}
}
