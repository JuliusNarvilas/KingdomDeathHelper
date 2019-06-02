using System;
using System.IO;
using UnityEngine;

namespace Common.IO
{
    public static class PNGHelpers
    {
        public static void Serialise(Sprite sprite, Stream stream)
        {
            Serialise(sprite.texture, stream);
        }

        public static void Serialise(Texture2D texture, Stream stream)
        {
            //make uncompressed texture
            Color[] pixels = texture.GetPixels(0, 0, texture.width, texture.height);
            Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            newTexture.SetPixels(pixels);
            newTexture.Apply();

            byte[] pngBytes = ImageConversion.EncodeToPNG(newTexture);
            stream.Write(pngBytes, 0, pngBytes.Length);
            stream.Flush();
        }

        public static Sprite DeserialiseToSprite(Stream stream)
        {
            Texture2D tex = DeserialiseToTexture(stream);
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        public static Texture2D DeserialiseToTexture(Stream stream)
        {
            using (var mem = new MemoryStream())
            {
                IOHelpers.CopyStream(stream, mem);
                byte[] pngBytes = mem.ToArray();

                Texture2D tex = new Texture2D(2, 2);
                ImageConversion.LoadImage(tex, pngBytes);
                return tex;
            }
        }
    }
}
