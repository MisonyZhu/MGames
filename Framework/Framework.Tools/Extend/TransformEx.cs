using UnityEngine;
using System.Text;

namespace Framework
{
    public static class TransformEx
  {
    public static bool IsOrIsChildOf(this Transform transform, Transform parent) => (UnityEngine.Object) transform == (UnityEngine.Object) parent || transform.IsChildOf(parent);

    public static void GetLocalPosition(
      this Transform transform,
      out float x,
      out float y,
      out float z)
    {
      Vector3 localPosition = transform.localPosition;
      x = localPosition.x;
      y = localPosition.y;
      z = localPosition.z;
    } 

    public static void SetLocalPosition(this Transform transform, float x, float y, float z) => transform.localPosition = new Vector3(x, y, z);

    public static float GetLocalPositionX(this Transform transform) => transform.localPosition.x;

    public static void SetLocalPositionX(this Transform transform, float x) 
    {
      Vector3 localPosition = transform.localPosition with
      {
        x = x
      };
      transform.localPosition = localPosition;
    }

    public static float GetLocalPositionY(this Transform transform) => transform.localPosition.y;

    public static void SetLocalPositionY(this Transform transform, float y)
    {
      Vector3 localPosition = transform.localPosition with
      {
        y = y
      };
      transform.localPosition = localPosition;
    }

    public static float GetLocalPositionZ(this Transform transform) => transform.localPosition.z;

    public static void SetLocalPositionZ(this Transform transform, float z)
    {
      Vector3 localPosition = transform.localPosition with
      {
        z = z
      };
      transform.localPosition = localPosition;
    }

    public static void GetPosition(
      this Transform transform,
      out float x,
      out float y,
      out float z)
    {
      Vector3 position = transform.position;
      x = position.x;
      y = position.y;
      z = position.z;
    }

    public static void SetPosition(this Transform transform, float x, float y, float z) => transform.position = new Vector3(x, y, z);

    public static float GetPositionX(this Transform transform) => transform.position.x;

    public static void SetPositionX(this Transform transform, float x)
    {
      Vector3 position = transform.position with { x = x };
      transform.position = position;
    }

    public static float GetPositionY(this Transform transform) => transform.position.y;

    public static void SetPositionY(this Transform transform, float y)
    {
      Vector3 position = transform.position with { y = y };
      transform.position = position;
    }

    public static float GetPositionZ(this Transform transform) => transform.position.z;

    public static void SetPositionZ(this Transform transform, float z)
    {
      Vector3 position = transform.position with { z = z };
      transform.position = position;
    }

    public static void SetLocalRotation(
      this Transform transform,
      float x,
      float y,
      float z,
      float w)
    {
      transform.localRotation = new Quaternion(x, y, z, w);
    }

    public static void GetLocalEulerAngles(
      this Transform transform,
      out float x,
      out float y,
      out float z)
    {
      Vector3 localEulerAngles = transform.localEulerAngles;
      x = localEulerAngles.x;
      y = localEulerAngles.y;
      z = localEulerAngles.z;
    }

    public static void SetLocalEulerAngles(this Transform transform, float x, float y, float z) => transform.localEulerAngles = new Vector3(x, y, z);

    public static float GetLocalEulerAnglesX(this Transform transform) => transform.localEulerAngles.x;

    public static void SetLocalEulerAnglesX(this Transform transform, float x)
    {
      Vector3 localEulerAngles = transform.localEulerAngles with
      {
        x = x
      };
      transform.localEulerAngles = localEulerAngles;
    }

    public static float GetLocalEulerAnglesY(this Transform transform) => transform.localEulerAngles.y;

    public static void SetLocalEulerAnglesY(this Transform transform, float y)
    {
      Vector3 localEulerAngles = transform.localEulerAngles with
      {
        y = y
      };
      transform.localEulerAngles = localEulerAngles;
    }

    public static float GetLocalEulerAnglesZ(this Transform transform) => transform.localEulerAngles.z;

    public static void SetLocalEulerAnglesZ(this Transform transform, float z)
    {
      Vector3 localEulerAngles = transform.localEulerAngles with
      {
        z = z
      };
      transform.localEulerAngles = localEulerAngles;
    }

    public static void GetEulerAngles(
      this Transform transform,
      out float x,
      out float y,
      out float z)
    {
      Vector3 eulerAngles = transform.eulerAngles;
      x = eulerAngles.x;
      y = eulerAngles.y;
      z = eulerAngles.z;
    }

    public static void SetEulerAngles(this Transform transform, float x, float y, float z) => transform.eulerAngles = new Vector3(x, y, z);

    public static float GetEulerAnglesX(this Transform transform) => transform.eulerAngles.x;

    public static void SetEulerAnglesX(this Transform transform, float x)
    {
      Vector3 eulerAngles = transform.eulerAngles with
      {
        x = x
      };
      transform.eulerAngles = eulerAngles;
    }

    public static float GetEulerAnglesY(this Transform transform) => transform.eulerAngles.y;

    public static void SetEulerAnglesY(this Transform transform, float y)
    {
      Vector3 eulerAngles = transform.eulerAngles with
      {
        y = y
      };
      transform.eulerAngles = eulerAngles;
    }

    public static float GetEulerAnglesZ(this Transform transform) => transform.eulerAngles.z;

    public static void SetEulerAnglesZ(this Transform transform, float z)
    {
      Vector3 eulerAngles = transform.eulerAngles with
      {
        z = z
      };
      transform.eulerAngles = eulerAngles;
    }

    public static void LookAt(this Transform transform, float x, float y, float z) => transform.LookAt(new Vector3(x, y, z));

    public static void SetLocalScale(this Transform transform, float x, float y, float z) => transform.localScale = new Vector3(x, y, z);

    public static void SetLocalScale(this Transform transform, float scale) => transform.localScale = new Vector3(scale, scale, scale);

    public static void GetLocalScale(
      this Transform transform,
      out float x,
      out float y,
      out float z)
    {
      Vector3 localScale = transform.localScale;
      x = localScale.x;
      y = localScale.y;
      z = localScale.z;
    }

    public static void GetLossyScale(
      this Transform transform,
      out float x,
      out float y,
      out float z)
    {
      Vector3 lossyScale = transform.lossyScale;
      x = lossyScale.x;
      y = lossyScale.y;
      z = lossyScale.z;
    }

    public static void GetInverseTransformPoint(
      this Transform transform,
      float posx,
      float posy,
      float posz,
      out float x,
      out float y,
      out float z)
    {
      Vector3 vector3 = transform.InverseTransformPoint(posx, posy, posz);
      x = vector3.x;
      y = vector3.y;
      z = vector3.z;
    }

    public static string GetPath(this Transform transform, Transform parent = null)
    {
      if (!(bool) (UnityEngine.Object) transform || !(bool) (UnityEngine.Object) transform.parent || (UnityEngine.Object) transform == (UnityEngine.Object) parent)
        return "";
      string path = transform.name;
      for (Transform parent1 = transform.parent; (bool) (UnityEngine.Object) parent1 && (UnityEngine.Object) parent1 != (UnityEngine.Object) parent; parent1 = parent1.parent)
        path = parent1.name + "/" + path;
      return path;
    }

    public static UnityEngine.Object Find(this Transform parent, string path, System.Type type)
    {
      if (!(bool) (UnityEngine.Object) parent)
        return (UnityEngine.Object) null;
      Transform transform = string.IsNullOrEmpty(path) ? parent : parent.Find(path);
      if (!(bool) (UnityEngine.Object) transform)
        return (UnityEngine.Object) null;
      if (type == typeof (Transform))
        return (UnityEngine.Object) transform;
      return type == typeof (GameObject) ? (UnityEngine.Object) transform.gameObject : (UnityEngine.Object) transform.GetComponent(type);
    }

    public static T Find<T>(this Transform parent, string path) where T : UnityEngine.Object => parent.Find(path, typeof (T)) as T;

    public static Transform FindByName(this Transform parent, string name)
    {
      if (string.IsNullOrEmpty(name))
        return (Transform) null;
      Transform byName1 = parent.Find(name);
      if ((UnityEngine.Object) byName1 != (UnityEngine.Object) null)
        return byName1;
      int childCount = parent.childCount;
      for (int index = 0; index < childCount; ++index)
      {
        Transform byName2 = parent.GetChild(index).FindByName(name);
        if ((UnityEngine.Object) byName2 != (UnityEngine.Object) null)
          return byName2;
      }
      return (Transform) null;
    }

    public static void SetParentAndIdentity(this Transform transform, Transform parent)
    {
      transform.SetParent(parent, false);
      transform.SetIdentity();
    }

    public static void SetIdentity(this Transform transform)
    {
      transform.localPosition = Vector3.zero;
      transform.localRotation = Quaternion.identity;
      transform.localScale = Vector3.one;
    }

    public static void DestroyChildren(this Transform transform)
    {
      if (!(bool) (UnityEngine.Object) transform)
        return;
      for (int index = transform.childCount - 1; index >= 0; --index)
        ObjectEx.Destroy((UnityEngine.Object) transform.GetChild(index).gameObject);
    }
    
    // public static string GetSiblingPath(this Transform transform, Transform parent = null)
    // {
    //   if (!(bool) (UnityEngine.Object) transform || !(bool) (UnityEngine.Object) transform.parent || (UnityEngine.Object) transform == (UnityEngine.Object) parent)
    //     return "";
    //   StringBuilder stringBuilder = Global.stringBuilder;
    //   stringBuilder.Append(transform.GetSiblingIndex());
    //   for (Transform parent1 = transform.parent; (bool) (UnityEngine.Object) parent1 && (UnityEngine.Object) parent1 != (UnityEngine.Object) parent; parent1 = parent1.parent)
    //     stringBuilder.Insert(0, '/').Insert(0, parent1.GetSiblingIndex());
    //   return stringBuilder.ToString();
    // } 

    public static Transform FindBySiblingPath(this Transform parent, string path, int start = 0)
    {
      if (!(bool) (UnityEngine.Object) parent)
        return (Transform) null;
      if (path == null || path.Length <= start)
        return parent;
      int index1 = 0;
      for (int index2 = start; index2 < path.Length; ++index2)
      {
        char ch = path[index2];
        if (ch == '/')
        {
          if (index1 >= parent.childCount)
            return (Transform) null;
          parent = parent.GetChild(index1);
          index1 = 0;
        }
        else
        {
          if (ch < '0' || ch > '9')
            return (Transform) null;
          index1 = index1 * 10 + ((int) ch - 48);
        }
      }
      return index1 < parent.childCount ? parent.GetChild(index1) : (Transform) null;
    }
  }
}