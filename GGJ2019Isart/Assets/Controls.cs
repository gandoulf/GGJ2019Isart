// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class Controls : InputActionAssetReference
{
    public Controls()
    {
    }
    public Controls(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // Controler
        m_Controler = asset.GetActionMap("Controler");
        m_Controler_Fire = m_Controler.GetAction("Fire");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Controler = null;
        m_Controler_Fire = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Controler
    private InputActionMap m_Controler;
    private InputAction m_Controler_Fire;
    public struct ControlerActions
    {
        private Controls m_Wrapper;
        public ControlerActions(Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire { get { return m_Wrapper.m_Controler_Fire; } }
        public InputActionMap Get() { return m_Wrapper.m_Controler; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(ControlerActions set) { return set.Get(); }
    }
    public ControlerActions @Controler
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new ControlerActions(this);
        }
    }
}
