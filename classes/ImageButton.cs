using System;
using System.Collections.Generic;
using Godot;

namespace AngryBird.classes;

[GlobalClass]
public partial class ImageButton : TextureButton
{
    #region StatedTexture enum

    public enum StatedTexture
    {
        Light,
        Dark,
    }

    #endregion

    private static readonly Dictionary<StatedTexture, int> StatedTextureMap =
        new()
        {
            { StatedTexture.Light, 40 },
            { StatedTexture.Dark, -150 },
        };

    private void Setup()
    {
        // 确定点击区域
        var bitmap = new Bitmap();
        bitmap.CreateFromImageAlpha(TextureNormal.GetImage());
        TextureClickMask = bitmap;

        // 处理hover，pressed等状态
        var statedTextures = GenerateStatedTextures(TextureNormal);
        TexturePressed = TextureNormal;
        TextureHover = statedTextures[StatedTexture.Light];
        TextureDisabled = statedTextures[StatedTexture.Dark];
    }


    private static Dictionary<StatedTexture, Texture2D> GenerateStatedTextures(Texture2D texture)
    {
        var image = texture.GetImage();
        var imageFormat = image.GetFormat();
        Dictionary<StatedTexture, Texture2D> result = new()
        {
            { StatedTexture.Light, null! },
            { StatedTexture.Dark, null! },
        };

        if (imageFormat is not (Image.Format.Rgba8 or Image.Format.Rgb8))
            throw new NotSupportedException(
                $"ImageButton only accepts Rgba8 and Rgb8 images, the image format is {imageFormat}");

        var sourceData = (byte[])image.Data["data"];
        var isRgba8 = imageFormat == Image.Format.Rgba8;

        foreach (var textureType in result.Keys)
        {
            var data = new byte[sourceData.Length];
            for (var i = 0; i < sourceData.Length; i++)
                data[i] = (i % (isRgba8 ? 4 : 3)) switch
                {
                    0 => (byte)Math.Max(0, Math.Min(255, sourceData[i] + StatedTextureMap[textureType])),
                    1 => (byte)Math.Max(0, Math.Min(255, sourceData[i] + StatedTextureMap[textureType])),
                    2 => (byte)Math.Max(0, Math.Min(255, sourceData[i] + StatedTextureMap[textureType])),
                    _ => sourceData[i]
                };

            var newImage = Image.CreateFromData(image.GetWidth(), image.GetHeight(), false,
                isRgba8 ? Image.Format.Rgba8 : Image.Format.Rgb8, data);

            result[textureType] = ImageTexture.CreateFromImage(newImage);
        }

        return result;
    }

    public override void _Ready()
    {
        base._Ready();
        Setup();
    }
}