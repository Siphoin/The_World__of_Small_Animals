using UnityEngine;

public static class TextureExtentions
{
    public static Texture2D ToTexture2D(this Texture texture)
    {
        Texture2D dest = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        dest.Apply(false);
        Graphics.CopyTexture(texture, dest);
        return dest;
    }

}