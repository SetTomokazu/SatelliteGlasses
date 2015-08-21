using UnityEngine;
using System.Collections;
using System;

public class SatelliteData{
	public int prn;
	public float snr;
	public float Azimuth;
	public float Elevation;
	public DateTime Launch;
	public DateTime Operation;

	public SatelliteData(DateTime launch, DateTime operation)
	{
		this.prn = 0;
		this.snr = 0;
		this.Azimuth = 0;
		this.Elevation = 0;
		this.Launch = new DateTime();
		this.Launch = launch;
		this.Operation = new DateTime();
		this.Operation = operation;
	}

}
