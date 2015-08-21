using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UguiRichOutline : Outline
{
	public int copyCount = 16;
	public override void ModifyVertices(System.Collections.Generic.List<UIVertex> verts)
	{
		if (!IsActive())
			return;

		var start = 0;
		var end = verts.Count;
		for (int i = 0; i < copyCount; i++)
		{
			float x = Mathf.Sin(Mathf.PI * 2 * i / copyCount) * effectDistance.x;
			float y = Mathf.Cos(Mathf.PI * 2 * i / copyCount) * effectDistance.y;
			ApplyShadow(verts, effectColor, start, verts.Count, x, y);
			start = end;
			end = verts.Count;
		}
	}
}
