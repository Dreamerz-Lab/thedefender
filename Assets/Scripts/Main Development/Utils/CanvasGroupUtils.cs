using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class CanvasGroupUtils
{
	/// <summary>
	/// This function Shows any Canvas Group by turning it's alpha to 1
	/// </summary>
	/// <param name="canvasGroup"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void OpenCanvasGroup(CanvasGroup canvasGroup) {
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;
	}

	/// <summary>
	/// This function Closes any Canvas Group by turning it's alpha to 0
	/// </summary>
	/// <param name="canvasGroup"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void CloseCanvasGroup(CanvasGroup canvasGroup) {
		canvasGroup.alpha = 0f;
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
	}
}
