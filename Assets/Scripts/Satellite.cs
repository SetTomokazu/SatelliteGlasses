using UnityEngine;
using System.Collections;

public class Satellite : MonoBehaviour {

	[SerializeField]
	private float far = 25.0f;
	[SerializeField]
	Animator animator;

	private Vector3 lookat = Vector3.down * 1000;

	private Vector3 center;
	private Vector3 dst;
	private float timecount = 0;
	void Awake()
	{
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update()
	{
		gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, dst, 0.1f);
		gameObject.transform.LookAt(lookat);
		/*
		if (timecount == 0)
		{
			dst = center + Random.insideUnitSphere * 3;
			timecount = GetRandom() * 5.0f;
		}
		else
		{
			timecount -= Time.deltaTime;
		}
		*/
	}
	/// <summary>
	/// 座標更新.
	/// </summary>
	/// <param name="newdst"></param>
	public void SetDst(Vector3 newdst)
	{
		center = newdst * far;
		dst = center + Random.insideUnitSphere * 3;
	}
	/// <summary>
	/// 初期設定.
	/// </summary>
	/// <param name="pos"></param>
	public void SetLocation(Vector3 pos)
	{
		center = pos * far;
		dst = center + Random.insideUnitSphere * 3;
		gameObject.transform.localPosition = center;
		gameObject.transform.localScale = Vector3.one * 15f;
		gameObject.transform.LookAt(lookat);
	}
	public void ResetAnimator()
	{
		animator.SetFloat("Flag", 0.0f);
		Debug.Log(gameObject.name + "Stay:" + animator.GetFloat("Flag"));
	}
	private float GetRandom()
	{
		Random.seed = int.Parse(gameObject.name);
		return Random.value;
	}
}
