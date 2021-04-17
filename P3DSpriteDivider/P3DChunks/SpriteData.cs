using System;
using System.Collections.Generic;
using System.Text;

namespace P3DSpriteDivider
{
	public partial class P3D
	{
        public struct SpriteDataChunk
        {
            public int ImageCount;
            public string Name;
            public int NativeX;
            public int NativeY;
            public string Shader;
            public int ImageWidth;
            public int ImageHeight;
            public int BlitBorder;
        } //Structure to hold univeral sprite data

        public static byte[] GenerateSpriteDataData(SpriteDataChunk spriteDat)
        {
            int offset = 0;
            byte[] textureDataChunk = new byte[1 + spriteDat.Name.Length + 1 + spriteDat.Shader.Length + (6 * 4)];
            Array.Copy(BitConverter.GetBytes(spriteDat.Name.Length), 0, textureDataChunk, offset++, 1);
            Array.Copy(Encoding.ASCII.GetBytes(spriteDat.Name), 0, textureDataChunk, offset, spriteDat.Name.Length); offset += spriteDat.Name.Length;
            Array.Copy(BitConverter.GetBytes(spriteDat.NativeX), 0, textureDataChunk, offset, 4); offset += 4;
            Array.Copy(BitConverter.GetBytes(spriteDat.NativeY), 0, textureDataChunk, offset, 4); offset += 4;
            Array.Copy(BitConverter.GetBytes(spriteDat.Shader.Length), 0, textureDataChunk, offset++, 1);
            Array.Copy(Encoding.ASCII.GetBytes(spriteDat.Shader), 0, textureDataChunk, offset, spriteDat.Shader.Length); offset += spriteDat.Shader.Length;
            Array.Copy(BitConverter.GetBytes(spriteDat.ImageWidth), 0, textureDataChunk, offset, 4); offset += 4;
            Array.Copy(BitConverter.GetBytes(spriteDat.ImageHeight), 0, textureDataChunk, offset, 4); offset += 4;
            Array.Copy(BitConverter.GetBytes(spriteDat.ImageCount), 0, textureDataChunk, offset, 4); offset += 4;
            Array.Copy(BitConverter.GetBytes(spriteDat.BlitBorder), 0, textureDataChunk, offset, 4); offset += 4;
            return textureDataChunk;
        }

        public static SpriteDataChunk GetSpriteData(P3D.Chunk sprite)
        {
            SpriteDataChunk spriteData = new SpriteDataChunk();
            int offset = 0;
            int nameLength = sprite.Data[offset++];
            spriteData.Name = System.Text.Encoding.UTF8.GetString(Utility.GetRange(sprite.Data, offset, nameLength));
            offset += nameLength;

            spriteData.NativeX = Utility.Byte4ToInt(sprite.Data, offset); offset += 4;
            spriteData.NativeY = Utility.Byte4ToInt(sprite.Data, offset); offset += 4;

            int shaderLength = sprite.Data[offset++];
            spriteData.Shader = System.Text.Encoding.UTF8.GetString(Utility.GetRange(sprite.Data, offset, shaderLength));
            offset += shaderLength;

            spriteData.ImageWidth = Utility.Byte4ToInt(sprite.Data, offset); offset += 4;
            spriteData.ImageHeight = Utility.Byte4ToInt(sprite.Data, offset); offset += 4;
            spriteData.ImageCount = Utility.Byte4ToInt(sprite.Data, offset); offset += 4;
            spriteData.BlitBorder = Utility.Byte4ToInt(sprite.Data, offset);

            return spriteData;
        }

    }
}
