using UnityEngine;
using System.Collections;
using System;

public class GPSData{
	/// <summary>
	/// 衛星番号.
	/// </summary>
	public string prn { get; set; }
	/// <summary>
	/// 打ち上げ年月日.
	/// </summary>
	public DateTime Launch { get; set; }
	/// <summary>
	/// 運用開始年月日.
	/// </summary>
	public DateTime operation { get; set; }


}
