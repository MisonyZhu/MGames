using UnityEngine;

namespace Framework;

public static class QualitySettingsEx
{
    public static int globalTextureMipmapLimit
    {
        get
        {
            return QualitySettings.globalTextureMipmapLimit;
        }
        set
        {
            QualitySettings.globalTextureMipmapLimit = value;
        }
    }
}