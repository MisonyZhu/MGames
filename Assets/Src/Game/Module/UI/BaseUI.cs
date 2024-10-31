using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;
using Object = UnityEngine.Object;

namespace Game
{
    abstract class BaseUI
    {
#pragma warning disable 0649
        public Action onCreate;
        public Action onShow;
        public Action onHide;
        public Action onDestroy;
#pragma warning restore 0649

        UILayer m_Layer;
        int m_OrderInLayer;
        UIHideType m_HideType;
        UIHideFunc m_HideFunc;
        UIEscClose m_EscClose;
        bool m_LoadSync = false;

        protected BaseUI(UILayer layer, int orderInLayer, UIHideType hideType, UIHideFunc hideFunc, UIEscClose escClose, bool loadSync)
        {
            m_Layer = layer;
            m_OrderInLayer = orderInLayer;
            m_HideType = hideType;
            m_HideFunc = hideFunc;
            m_EscClose = escClose;
            m_LoadSync = loadSync;
        }

        #region Override
        protected abstract string prefabPath { get; }

        protected virtual void OnCreate()
        { }

        protected virtual void OnShow()
        { }

        public virtual void Refresh()
        { }

        protected virtual IEnumerator ShowAnimation()
        {
            yield return DefaultShowAnimation();
        }

        public virtual void Update()
        { }

        public virtual bool OnEscClose()
        {
            switch (m_EscClose)
            {
                case UIEscClose.DontClose:
                    return false;
                case UIEscClose.Close:
                    Hide(true);
                    return true;
                case UIEscClose.Block:
                    return true;
            }
            return false;
        }

        public virtual bool OnKeyInput()
        {
            return false;
        }

        public virtual void LateUpdate()
        { }

        protected virtual void OnHide()
        { }

        protected virtual IEnumerator HideAnimation()
        {
            yield return DefaultHideAnimation();
        }

        protected virtual void OnDestroy()
        { }
        #endregion

        #region Visible
        public bool visible { get; private set; } = false;

        public void SetVisible(bool visible, bool playAnimation = true)
        {
            if (this.visible == visible) return;

            this.visible = visible;
            m_PlayAnimation = playAnimation;

            if (this.visible)
            {
                StopHide();
                if (loaded)
                {
                    SetVisibleImpl(true);
                    StartShow();
                }
                else
                {
                    StartLoad();
                }
            }
            else
            {
                StopShow();
                if (loaded)
                {
                    StartHide();
                }
            }
        }

        public void Show(bool playAnimation = true)
        {
            SetVisible(true, playAnimation);
        }

        public void Hide(bool playAnimation = true)
        {
            SetVisible(false, playAnimation);
        }

        bool m_RootSaved = false;
        Vector2 m_RootSavedPosition;

        void SetVisibleImpl(bool visible)
        {
            switch (m_HideFunc)
            {
                case UIHideFunc.Deactive:
                    m_Root.active = visible;
                    break;
                case UIHideFunc.MoveOutOfScreen:
                    if (visible)
                    {
                        if (m_RootSaved)
                        {
                            m_Root.anchoredPosition = m_RootSavedPosition;
                        }
                    }
                    else
                    {
                        m_RootSavedPosition = m_Root.anchoredPosition;
                        m_RootSaved = true;
                        m_Root.anchoredPosition = Vector2.one * 100000000;
                    }
                    break;
            }
        }
        #endregion

        #region Layer
        public UILayer layer
        {
            get => m_Layer;
            set
            {
                if (m_Layer != value)
                {
                    m_Layer = value;
                    RefreshLayer();
                }
            }
        }

        public int orderInLayer
        {
            get => m_OrderInLayer;
            set
            {
                if (m_OrderInLayer != value)
                {
                    m_OrderInLayer = value;
                    RefreshLayer();
                }
            }
        }

        public int sortingOrder => (int)m_Layer * 100 + m_OrderInLayer;

        void RefreshLayer()
        {
            if (m_Canvas)
            {
                m_Canvas.sortingOrder = sortingOrder;
            }
        }
        #endregion

        #region Load
        Transform m_Parent;
        UIControl m_Root;
        UIGenerator m_Generator;
        Canvas m_Canvas;
        public bool loaded { get; private set; } = false;
        Coroutine m_LoadCoroutine;
        
        InstantiateOperation m_LoadRequest;

        void StartLoad()
        {
            if (m_LoadRequest != null) return;

            if (m_LoadSync &&  Application.platform != RuntimePlatform.WebGLPlayer )
                LoadSync();
            else
                m_LoadCoroutine = GameEntry.Instance.StartCoroutine(LoadAsync());
        }

        public void SetParent(Transform parent)
        {
            m_Parent = parent;
            if (loaded)
            {
                m_Root.parent = parent;
            }
        }

        Transform GetParent()
        {
            return m_Parent ?? UIModule.root;
        }

        void LoadSync()
        {
            var go = ResourceModule.Instantiate(prefabPath, GetParent(), false);
            if (!go)
            {
                Debug.LogError(prefabPath + "not exist");
                return;
            }

            OnLoad(go);
        }

        IEnumerator LoadAsync()
        {
            m_LoadRequest = ResourceModule .InstantiateAsync(prefabPath, GetParent(), false);
            yield return m_LoadRequest;
            
            // canceled
            if (m_LoadRequest == null)
            {
                yield break;
            }

            if (!string.IsNullOrEmpty(m_LoadRequest.Error))
            {
                Debug.LogError(m_LoadRequest.Error);
                yield break;
            }
            
            OnLoad(m_LoadRequest.Result);
            m_LoadRequest = null;
            m_LoadCoroutine = null;
        }

        void OnLoad(GameObject go)
        {
            m_Root = new UIControl(this, go.transform);
            m_Generator = m_Root.GetComponent<UIGenerator>();
            InitControls();

            m_Canvas = go.AddMissingComponent<Canvas>();
            m_Canvas.overrideSorting = true;
            RefreshLayer();

            go.AddMissingComponent<GraphicRaycaster>();

            SetVisibleImpl(visible);
            loaded = true;

            OnCreate();
            onCreate?.Invoke();
            EventModule.Dispatch(new UIEventArgs(EventID.UI_Create, this));

            InitAnimation();
            if (visible)
            {
                StartShow();
            }
        }

        void StopLoad()
        {
            if (m_LoadCoroutine != null)
            {
                GameEntry.Instance.StopCoroutine(m_LoadCoroutine);
                m_LoadCoroutine = null;
            }

            if (m_LoadRequest != null)
            {
                m_LoadRequest.Cancel();
                m_LoadRequest = null;
            }
        }
        #endregion

        #region Show
        Coroutine m_ShowCoroutine;

        void StartShow()
        {
            UIModule.HideLayer(layer, orderInLayer, m_PlayAnimation);
            m_ShowCoroutine = GameEntry.Instance.StartCoroutine(DoShow());
        }

        void StopShow()
        {
            if (m_ShowCoroutine != null)
            {
                GameEntry.Instance.StopCoroutine(m_ShowCoroutine);
                m_ShowCoroutine = null;
            }
        }

        IEnumerator DoShow()
        {
            UIModule.AddUI(this);

            OnShow();
            onShow?.Invoke();
            EventModule.Dispatch(new UIEventArgs(EventID.UI_Show, this));

            if (animationEnabled && m_PlayAnimation)
            {
                yield return ShowAnimation();
            }
        }
        #endregion

        #region Hide
        Coroutine m_HideCoroutine;

        void StartHide()
        {
            m_HideCoroutine = GameEntry.Instance.StartCoroutine(DoHide());
        }

        void StopHide()
        {
            if (m_HideCoroutine != null)
            {
                GameEntry.Instance.StopCoroutine(m_HideCoroutine);
                m_HideCoroutine = null;
            }
        }

        IEnumerator DoHide()
        {
            UIModule.RemoveUI(this);
            HideSubUIs();
            UnRegistAllEvents();

            OnHide();
            onHide?.Invoke();
            EventModule.Dispatch(new UIEventArgs(EventID.UI_Hide, this));

            if (animationEnabled && m_PlayAnimation)
            {
                yield return HideAnimation();
            }

            switch (m_HideType)
            {
                case UIHideType.Destroy:
                    Destroy();
                    break;
                case UIHideType.WaitDestroy:
                    SetVisibleImpl(false);
                    yield return new WaitForSeconds(UIConfig.WAIT_DESTROY_TIME);
                    Destroy();
                    break;
                case UIHideType.Hide:
                    SetVisibleImpl(false);
                    break;
            }
        }
        #endregion

        #region Destroy
        public virtual void Destroy()
        {
            DestroySubUIs();
            Hide(false);
            ReleaseAllAssets();

            StopLoad();
            if (!loaded) return;

            OnDestroy();
            onDestroy?.Invoke();
            EventModule.Dispatch(new UIEventArgs(EventID.UI_Destroy, this));

            DestroyControls();
            m_Generator = null;

            m_Root.Destroy();
            m_Root = null;
            loaded = false;
        }
        #endregion

        #region Controls
        UIControl[] m_Controls;

        void InitControls()
        {
            m_Controls = new UIControl[m_Generator.controlCount];
        }

        protected UIControl GetControl(int id)
        {
            var control = m_Controls[id];
            if (control == null)
            {
                control = new UIControl(this, m_Generator.GetControl(id));
                m_Controls[id] = control;
            }
            return control;
        }

        void DestroyControls()
        {
            m_Controls = null;
        }
        #endregion

        #region Events
        EventModule.Container m_EventContainer;

        protected void RegistEvent<T>(int id, Action<T> func, bool triggerOnce = false, int priority = 0) where T : IEventArgs
        {
            if (m_EventContainer == null)
            {
                m_EventContainer = new EventModule.Container();
            }
            m_EventContainer.RegistEvent(id, func, triggerOnce, priority);
        }

        protected void RegistEvent(int id, Action func, bool triggerOnce = true, int priority = 0)
        {
            m_EventContainer.RegistEvent(id, func, triggerOnce, priority);
        }

        protected void UnRegistEvent<T>(int id, Action<T> func) where T : IEventArgs
        {
            m_EventContainer?.UnRegistEvent(id, func);
        }

        void UnRegistAllEvents()
        {
            m_EventContainer?.UnRegistAllEvents();
        }
        #endregion

        #region Assets
        ResourceGroup m_AssetGroup;
        ResourceGroup assetGroup
        {
            get
            {
                if (m_AssetGroup == null)
                    m_AssetGroup = new ResourceGroup();
                return m_AssetGroup;
            }
        }

        List<AssetLoader> m_AssetLoaders;

        protected T LoadAsset<T>(string path) where T : Object
        {
            return assetGroup.LoadAsset<T>(path) as T;
        }

        protected AssetHandle LoadAssetAsync<T>(string path, Action<T> onComplete = null) where T : Object
        {
            var handler = assetGroup.LoadAssetAsync<T>(path);
            handler.Completed  += (res) => onComplete(res as T);
            return handler;
        }

        public AssetLoader GetAssetLoader(Action<Object> onComplete)
        {
            if (m_AssetLoaders == null)
            {
                m_AssetLoaders = new List<AssetLoader>();
            }
        
            var assetLoader = new AssetLoader(onComplete);
            m_AssetLoaders.Add(assetLoader);
            return assetLoader;
        }

        void ReleaseAllAssets()
        {
            if (m_AssetGroup != null)
            {
                m_AssetGroup.Dispose();
                m_AssetGroup = null;
            }

            if (m_AssetLoaders != null)
            {
                foreach (var assetLoader in m_AssetLoaders)
                {
                    assetLoader.Dispose();
                }
                m_AssetLoaders = null;
            }
        }
        #endregion

        #region Animation
        bool m_PlayAnimation = true;
        public bool animationEnabled { get; protected set; }
        UIControl Panel_Center;
        UIControl Panel_Left;
        UIControl Panel_Right;
        UIControl Panel_Bottom;
        UIControl Panel_Top;

        protected virtual void InitAnimation()
        {
            Panel_Center = GetControl("Panel_Center");
            Panel_Left = GetControl("Panel_Left");
            Panel_Right = GetControl("Panel_Right");
            Panel_Bottom = GetControl("Panel_Bottom");
            Panel_Top = GetControl("Panel_Top");

            animationEnabled = Panel_Center != null ||
                Panel_Left != null || Panel_Right != null ||
                Panel_Bottom != null || Panel_Top != null;
        }

        UIControl GetControl(string name)
        {
            var transform = m_Root.transform.Find(name);
            return transform ? new UIControl(this, transform) : null;
        }

        protected IEnumerator DefaultShowAnimation()
        {
            int sw = Screen.width;
            int sh = Screen.height;
            float duration = 0.3f;
            Ease ease = Ease.OutCubic;

            if (Panel_Center != null)
            {
                Panel_Center.localScale = 0;
                Panel_Center.transform.DOScale(1, duration).SetEase(ease);
            }

            if (Panel_Left != null)
            {
                float x = Panel_Left.localPositionX;
                Panel_Left.localPositionX = x - sw;
                Panel_Left.transform.DOLocalMoveX(x, duration).SetEase(ease);
            }

            if (Panel_Right != null)
            {
                float x = Panel_Right.localPositionX;
                Panel_Right.localPositionX = x + sw;
                Panel_Right.transform.DOLocalMoveX(x, duration).SetEase(ease);
            }

            if (Panel_Bottom != null)
            {
                float y = Panel_Bottom.localPositionY;
                Panel_Bottom.localPositionY = y - sh;
                Panel_Bottom.transform.DOLocalMoveY(y, duration).SetEase(ease);
            }

            if (Panel_Top != null)
            {
                float y = Panel_Top.localPositionY;
                Panel_Top.localPositionY = y + sh;
                Panel_Top.transform.DOLocalMoveY(y, duration).SetEase(ease);
            }

            yield return new WaitForSeconds(duration);
        }

        protected IEnumerator DefaultHideAnimation()
        {
            int sw = Screen.width;
            int sh = Screen.height;
            float duration = 0.3f;
            Ease ease = Ease.InCubic;

            if (Panel_Center != null)
            {
                Panel_Center.transform.DOScale(0, duration).SetEase(ease);
            }

            if (Panel_Left != null)
            {
                float x = Panel_Left.localPositionX;
                Panel_Left.transform.DOLocalMoveX(x - sw, duration).SetEase(ease);
            }

            if (Panel_Right != null)
            {
                float x = Panel_Right.localPositionX;
                Panel_Right.transform.DOLocalMoveX(x + sw, duration).SetEase(ease);
            }

            if (Panel_Bottom != null)
            {
                float y = Panel_Bottom.localPositionY;
                Panel_Bottom.transform.DOLocalMoveY(y - sh, duration).SetEase(ease);
            }

            if (Panel_Top != null)
            {
                float y = Panel_Top.localPositionY;
                Panel_Top.transform.DOLocalMoveY(y + sh, duration).SetEase(ease);
            }

            yield return new WaitForSeconds(duration);

            if (Panel_Center != null)
                Panel_Center.localScale = 1;

            if (Panel_Left != null)
                Panel_Left.localPositionX += sw;

            if (Panel_Right != null)
                Panel_Right.localPositionX -= sw;

            if (Panel_Bottom != null)
                Panel_Bottom.localPositionY += sh;

            if (Panel_Top != null)
                Panel_Top.localPositionY -= sh;
        }
        #endregion

        #region SubUIs
        List<BaseUI> m_SubUIs;

        protected T ShowSubUI<T>(Transform parent) where T : BaseUI, new()
        {
            var ui = GetSubUI<T>();
            if (ui == null)
            {
                ui = new T();

                if (m_SubUIs == null)
                    m_SubUIs = new List<BaseUI>();
                m_SubUIs.Add(ui);
            }

            ui.SetParent(parent);
            ui.Show();
            return ui;
        }

        protected void HideSubUI<T>() where T : BaseUI
        {
            GetSubUI<T>()?.Hide();
        }

        protected T GetSubUI<T>() where T : BaseUI
        {
            if (m_SubUIs != null)
            {
                var type = typeof(T);
                for (int i = 0; i < m_SubUIs.Count; ++i)
                {
                    if (m_SubUIs[i].GetType() == type)
                    {
                        return m_SubUIs[i] as T;
                    }
                }
            }
            return null;
        }

        void HideSubUIs()
        {
            if (m_SubUIs != null)
            {
                for (int i = 0; i < m_SubUIs.Count; ++i)
                {
                    m_SubUIs[i].Hide();
                }
            }
        }

        void DestroySubUIs()
        {
            if (m_SubUIs != null)
            {
                for (int i = 0; i < m_SubUIs.Count; ++i)
                {
                    m_SubUIs[i].Destroy();
                }
            }
        }
        #endregion

        #region Loader
        public class SpriteLoader
        {
            public AssetLoader assetLoader;
            public bool setNativeSize;
            public Action<UIControl> onComplete;

            public SpriteLoader(BaseUI ui, Action<Object> onComplete)
            {
                assetLoader = ui.GetAssetLoader(onComplete);
            }

            public void Load(string path)
            {
                assetLoader.Load(path, typeof(Sprite));
            }

            public AssetHandle LoadAsync(string path)
            {
                return assetLoader.LoadAsync(path, typeof(Sprite));
            }
        }

        public class TextureLoader
        {
            public AssetLoader assetLoader;
            public bool setNativeSize;
            public Action<UIControl> onComplete;

            public TextureLoader(BaseUI ui, Action<Object> onComplete)
            {
                assetLoader = ui.GetAssetLoader(onComplete);
            }

            public void Load(string path)
            {
                assetLoader.Load(path, typeof(Texture));
            }

            public AssetHandle LoadAsync(string path)
            {
                return assetLoader.LoadAsync(path, typeof(Texture));
            }
        }
        #endregion
    }

    abstract class SingletonUI<T> : BaseUI where T : BaseUI, new()
    {
        public static T m_Instance;
        public static bool instanced => m_Instance != null;
        public static T instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new T();
                return m_Instance;
            }
        }

        protected SingletonUI(UILayer layer, int orderInLayer, UIHideType hideType, UIHideFunc hideFunc, UIEscClose escClose, bool loadSync)
            : base(layer, orderInLayer, hideType, hideFunc, escClose, loadSync)
        {
        }

        public override void Destroy()
        {
            base.Destroy();

            if (m_Instance == this)
                m_Instance = null;
        }
    }
}
