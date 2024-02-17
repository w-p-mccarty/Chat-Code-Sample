/*
 * UIManager.cs
 * By: William McCarty
 * Handles The UI Management
 * */
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //UI Manager Singleton
    private static UIManager _Instance;
    public static  UIManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = FindObjectOfType<UIManager>();
            }
            return _Instance;
        }
    }
    //Structure format to display key and sprite icon in editor
    [System.Serializable]
    public struct Icon
    {
        public string Key;
        public Sprite IconVersion;
    }

    #region Different UI Pages
    //Prefabs of Scenes the UIManager will handle
    [SerializeField]
    private ConversationUI _ConversationUI;
    [SerializeField]
    private ProfileUI _ProfileUI;
    [SerializeField]
    private ConversationsListUI _ConversationsListUI;
    #endregion

    [SerializeField]
    private Icon[] Icons;


    public enum UIType
    {
        ConversationsList,
        Profile,
        Conversation,
        None
    }

    //Current trace of UI Pages that were used
    private Stack<UIType> _UIPages = new Stack<UIType>();
    //Current UI Page that is active
    private UIPage _ActivePage;
    public UIPage ActivePage
    {
        get
        {
            return _ActivePage;
        }
    }

    public void Awake()
    {
        _Instance = this;
    }

    /// <summary>
    /// Get The Sprite Icon based on the key passed
    /// </summary>
    /// <param name="key">Sprite Id</param>
    /// <returns>Returns the icon sprite or null if not found</returns>
    public Sprite GetIcon(string key)
    {
        return Array.Find(Icons, x => x.Key == key).IconVersion;
    }

    #region UI Page Commands
    /// <summary>
    /// Active the UI Page Type that is passed in
    /// </summary>
    /// <param name="type">UI Page to activate</param>
    /// <param name="addToStack">whether it should be added to the stack trace</param>
    public void OnUISelect(UIType type, bool addToStack = true)
    {
        switch(type)
        {
            case UIType.Conversation:
                SetToConversationPage(addToStack);
                break;
            case UIType.ConversationsList:
                SetToConversationListPage(addToStack);
                break;
            case UIType.Profile:
                SetToProfilePage(addToStack);
                break;
        }
    }

    /// <summary>
    /// Go back to the previous UI Page
    /// </summary>
    public void OnUIGoBack()
    {
        if(_UIPages.Count > 0)
        {
            
            _UIPages.Pop();
            //Destroy our last UI
            Destroy(_ActivePage.gameObject);
            //Select the previous one
            OnUISelect(_UIPages.Peek(), false);
        }
    }
    #endregion

    #region Active UI Page Setters
    /// <summary>
    /// Sets the ActivePage to the Conversation Page
    /// </summary>
    /// <param name="addToStack">Whether it should be added to the stack trace</param>
    private void SetToConversationPage(bool addToStack = true)
    {
        _ActivePage = Instantiate(_ConversationUI, this.transform.position, Quaternion.identity, this.transform) as ConversationUI;
        if(addToStack)
        {
            _UIPages.Push(UIType.Conversation);
        }
    }

    /// <summary>
    /// Sets the ActivePage to the ConversationList Page
    /// </summary>
    /// <param name="addToStack">Whether it should be added to the stack trace</param>
    private void SetToConversationListPage(bool addToStack = true)
    {
        _ActivePage = Instantiate(_ConversationsListUI, this.transform.position, Quaternion.identity, this.transform) as ConversationsListUI;
        if (addToStack)
        {
            _UIPages.Push(UIType.ConversationsList);
        }
    }

    /// <summary>
    /// Sets the ActivePage to the Profile Page
    /// </summary>
    /// <param name="addToStack">Whether it should be added to the stack trace</param>
    private void SetToProfilePage(bool addToStack = true)
    {
        _ActivePage = Instantiate(_ProfileUI, this.transform.position, Quaternion.identity, this.transform) as ProfileUI;
        if (addToStack)
        {
            _UIPages.Push(UIType.Profile);
        }
    }
    #endregion
}
