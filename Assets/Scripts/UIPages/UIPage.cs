/*
 * UIPage.cs
 * By: William McCarty
 * Class Handles Global Functions all UI Sections share
 * */
using UnityEngine;

public abstract class UIPage : MonoBehaviour
{
    /// <summary>
    /// When the UI Screen is backed out of
    /// </summary>
    public virtual void Back()
    {
        //Issue to be tossed off the stack and load the previous
        UIManager.Instance.OnUIGoBack();
    }
	
}
