using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Framework;
using Object = UnityEngine.Object;

namespace Game
{
     class UIControl
    {
        public BaseUI ui { get; private set; }
        public Transform root { get; private set; }
        public string path { get; private set; }

        public UIControl()
        { }
        
        public UIControl(BaseUI ui, Transform root, string path = null)
        {
            Init(ui, root, path);
        }

        public void Init(BaseUI ui, Transform root, string path = null)
        {
            this.ui = ui;
            this.root = root;
            this.path = path;

            if (string.IsNullOrEmpty(path))
                m_Transform = root;
        }

        public UIControl GetChildControl(string path)
        {
            return new UIControl(ui, transform, path);
        }

        #region Transform
        Transform m_Transform;
        public Transform transform
        {
            get
            {
                if (!m_Transform)
                {
                    m_Transform = string.IsNullOrEmpty(path) ? root : root.Find(path);
                }
                return m_Transform;
            }
        }

        public Vector3 localPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public float localPositionX
        {
            get => transform.GetLocalPositionX();
            set => transform.SetLocalPositionX(value);
        }

        public float localPositionY
        {
            get => transform.GetLocalPositionY();
            set => transform.SetLocalPositionY(value);
        }
        
        public float localEulerAngleZ
        {
            get => m_Transform.GetLocalEulerAnglesZ();
            set => m_Transform.SetLocalEulerAnglesZ(value);
        }

        public float localScale
        {
            get => m_Transform.localScale.x;
            set => m_Transform.SetLocalScale(value);
        }

        public Transform parent
        {
            get => transform.parent;
            set => transform.SetParent(value, false);
        }

        public void SetParent(UIControl control)
        {
            transform.SetParent(control.transform, false);
        }
        
        public void SetAsFirstSibling()
        {
            transform.SetAsFirstSibling();
        }

        public void SetAsLastSibling()
        {
            transform.SetAsLastSibling();
        }

        public int siblingIndex
        {
            get => transform.GetSiblingIndex();
            set => transform.SetSiblingIndex(value);
        }
        #endregion

        #region RectTransform
        public RectTransform rectTransform => transform as RectTransform;

        public Vector2 pivot
        {
            get => rectTransform.pivot;
            set => rectTransform.pivot = value;
        }
        
        public Vector2 sizeDelta
        {
            get => rectTransform.sizeDelta;
            set => rectTransform.sizeDelta = value;
        }

        public float sizeDeltaX
        {
            get => rectTransform.GetSizeDeltaX();
            set => rectTransform.SetSizeDeltaX(value);
        }

        public float sizeDeltaY
        {
            get => rectTransform.GetSizeDeltaY();
            set => rectTransform.SetSizeDeltaY(value);
        }

        public Vector2 anchoredPosition
        {
            get => rectTransform.anchoredPosition;
            set => rectTransform.anchoredPosition = value;
        }

        public float anchoredPositionX
        {
            get => rectTransform.GetAnchoredPositionX();
            set => rectTransform.SetAnchoredPositionX(value);
        }

        public float anchoredPositionXY
        {
            get => rectTransform.GetAnchoredPositionY();
            set => rectTransform.SetAnchoredPositionY(value);
        }
        
        public Vector2 anchorMin
        {
            get => rectTransform.anchorMin;
            set => rectTransform.anchorMin = value;
        }

        public Vector2 anchorMax
        {
            get => rectTransform.anchorMax;
            set => rectTransform.anchorMax = value;
        }
        #endregion

        #region GameObject
        GameObject m_GameObject;
        public GameObject gameObject
        {
            get
            {
                if (!m_GameObject)
                {
                    m_GameObject = transform.gameObject;
                }
                return m_GameObject;
            }
        }

        public string name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        public bool active
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public GameObject Instantiate(Transform parent)
        {
            var go = Object.Instantiate(gameObject, parent);
            go.name = name;
            return go;
        }

        public void Destroy()
        {
            if (gameObject)
            {
                ObjectEx.Destroy(gameObject);
            }
        }

        public T GetComponent<T>() where T : Component
        {
            return gameObject.GetComponent<T>();
        }
        #endregion
        
        #region graphic
        Graphic m_Graphic;
        public Graphic graphic
        {
            get
            {
                if (!m_Graphic)
                {
                    m_Graphic = GetComponent<Graphic>();
                }
                return m_Graphic;
            }
        }

        public bool raycastTarget
        {
            get => graphic.raycastTarget;
            set => graphic.raycastTarget = value;
        }

        public Color color
        {
            get => graphic.color;
            set => graphic.color = value;
        }

        public void SetColor(int rgba)
        {
            graphic.SetColor(rgba);
        }

        public void SetAlpha(float alpha)
        {
            graphic.SetAlpha(alpha);
        }

        public void SetNativeSize()
        {
            graphic.SetNativeSize();
        }
        #endregion

        #region selectable
        Selectable m_Selectable;
        public Selectable selectable
        {
            get
            {
                if (!m_Selectable)
                {
                    m_Selectable = GetComponent<Selectable>();
                }
                return m_Selectable;
            }
        }

        public bool interactable
        {
            get => selectable.interactable;
            set => selectable.interactable = value;
        }
        #endregion

        #region Image
        Image image => graphic as Image;

        public Sprite sprite
        { 
            get => image.sprite;
            set => image.sprite = value;
        }

        public float fillAmount
        {
            get => image.fillAmount;
            set => image.fillAmount = value;
        }

        // BaseUI.SpriteLoader m_SpriteLoader;
        // BaseUI.SpriteLoader GetSpriteLoader(bool setNativeSize, Action<UIControl> onComplete)
        // {
        //     if (m_SpriteLoader == null)
        //     {
        //         m_SpriteLoader = new BaseUI.SpriteLoader(ui, (asset) =>
        //         {
        //             sprite = asset as Sprite;
        //             if (m_SpriteLoader.setNativeSize)
        //             {
        //                 SetNativeSize();
        //             }
        //             m_SpriteLoader.onComplete?.Invoke(this);
        //         });
        //     }
        //
        //     m_SpriteLoader.setNativeSize = setNativeSize;
        //     m_SpriteLoader.onComplete = onComplete;
        //     return m_SpriteLoader;
        // }
        //
        // public void LoadSprite(string path, bool setNativeSize = false)
        // {
        //     GetSpriteLoader(setNativeSize, null).Load(path);
        // }
        //
        // public void LoadSpriteAsync(string path, bool setNativeSize = false, Action<UIControl> onComplete = null)
        // {
        //     GetSpriteLoader(setNativeSize, onComplete).LoadAsync(path);
        // }
        #endregion

        #region RawImage
        RawImage rawImage => graphic as RawImage;

        public Texture texture
        {
            get => rawImage.texture;
            set => rawImage.texture = value;
        }

        public Rect uvRect
        {
            get => rawImage.uvRect;
            set => rawImage.uvRect = value;
        }

        // BaseUI.TextureLoader m_TextureLoader;
        // BaseUI.TextureLoader GetTextureLoader(bool setNativeSize, Action<UIControl> onComplete)
        // {
        //     if (m_TextureLoader == null)
        //     {
        //         m_TextureLoader = new BaseUI.TextureLoader(ui, (asset) =>
        //         {
        //             texture = asset as Texture;
        //             if (m_TextureLoader.setNativeSize)
        //             {
        //                 SetNativeSize();
        //             }
        //             m_TextureLoader.onComplete?.Invoke(this);
        //         });
        //     }
        //
        //     m_TextureLoader.setNativeSize = setNativeSize;
        //     m_TextureLoader.onComplete = onComplete;
        //     return m_TextureLoader;
        // }

        // public void LoadTexture(string path, bool setNativeSize = false, float cacheTime = ResourceManager.ASSET_CACHE_TIME_USE_DEFAULT)
        // {
        //     GetTextureLoader(setNativeSize, null).Load(path, cacheTime);
        // }
        //
        // public void LoadTextureAsync(string path, bool setNativeSize = false, Action<UIControl> onComplete = null, float cacheTime = ResourceManager.ASSET_CACHE_TIME_USE_DEFAULT)
        // {
        //     GetTextureLoader(setNativeSize, onComplete).LoadAsync(path, cacheTime);
        // }
        #endregion

        #region Text
        Text uiText => graphic as Text;
        
        public string text
        {
            get => uiText.text;
            set => uiText.text = value;
        }
        #endregion

        #region Button
        Button button => selectable as Button;

        public Action<UIControl> onClick
        {
            set
            {
                if (value != null)
                    button.onClick.SetListener(() => value(this));
                else
                    button.onClick.SetListener(null);
            }
        }
        #endregion

        #region InputField
        InputField inputField => selectable as InputField;

        public string inputText
        {
            get => inputField.text;
            set => inputField.text = value;
        }
        #endregion

        #region DropDown
        Dropdown dropDown => selectable as Dropdown;

        public int dropValue
        {
            get => dropDown.value;
            set => dropDown.value = value;
        }
        

        #endregion
        
        #region Event
        public Action<UIControl, PointerEventData> onPointerEnter
        {
            set => gameObject.AddMissingComponent<UIPointerEvents>().onPointerEnter = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onPointerExit
        {
            set => gameObject.AddMissingComponent<UIPointerEvents>().onPointerExit = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onPointerDown
        {
            set => gameObject.AddMissingComponent<UIPointerEvents>().onPointerDown = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onPointerUp
        {
            set => gameObject.AddMissingComponent<UIPointerEvents>().onPointerUp = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onPointerClick
        {
            set => gameObject.AddMissingComponent<UIPointerEvents>().onPointerClick = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onInitializePotentialDrag
        {
            set => gameObject.AddMissingComponent<UIInitializePotentialDragEvent>().onInitializePotentialDrag = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onBeginDrag
        {
            set => gameObject.AddMissingComponent<UIDragDropEvents>().onBeginDrag = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onDrag
        {
            set => gameObject.AddMissingComponent<UIDragDropEvents>().onDrag = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onEndDrag
        {
            set => gameObject.AddMissingComponent<UIDragDropEvents>().onEndDrag = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onDrop
        {
            set => gameObject.AddMissingComponent<UIDragDropEvents>().onDrop = (data) => value(this, data);
        }

        public Action<UIControl, PointerEventData> onScroll
        {
            set => gameObject.AddMissingComponent<UIScrollEvent>().onScroll = (data) => value(this, data);
        }

        public Action<UIControl, BaseEventData> onUpdateSelected
        {
            set => gameObject.AddMissingComponent<UIUpdateSelectedEvent>().onUpdateSelected = (data) => value(this, data);
        }

        public Action<UIControl, BaseEventData> onSelect
        {
            set => gameObject.AddMissingComponent<UISelectEvent>().onSelect = (data) => value(this, data);
        }

        public Action<UIControl, BaseEventData> onDeselect
        {
            set => gameObject.AddMissingComponent<UIDeselectEvent>().onDeselect = (data) => value(this, data);
        }

        public Action<UIControl, BaseEventData> onMove
        {
            set => gameObject.AddMissingComponent<UIMoveEvent>().onMove = (data) => value(this, data);
        }

        public Action<UIControl, BaseEventData> onSubmit
        {
            set => gameObject.AddMissingComponent<UISubmitEvent>().onSubmit = (data) => value(this, data);
        }

        public Action<UIControl, BaseEventData> onCancel
        {
            set => gameObject.AddMissingComponent<UICancelEvent>().onCancel = (data) => value(this, data);
        }
        #endregion
    }
}
