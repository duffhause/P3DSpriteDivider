using System.Drawing;
using System.Linq;
using System;
using System.Collections.Generic;

namespace P3DSpriteDivider
{
	public partial class P3D
    {
        //Will take a List of chunks and edit images in Texture chunks
        public string GetSpriteName (Chunk c)
        {
                int offset = c.Data[0];
                if (offset == 0)
                {
                    return "";
                }
                offset++;
            return System.Text.Encoding.UTF8.GetString(c.Data[1..offset]);
        }

        public Chunk DivideSprite (Chunk chunk)
        {
            (ImageDataStruct, Chunk, Bitmap) ProcessImageChunk(Chunk subChunk)
            {
                ImageDataStruct imageData = GetImageData(subChunk);
                Chunk image = subChunk.subChunks[0];

                if (!image.ChunkType.SequenceEqual(chunkOrder[IMAGEDATA_CHUNK]))
                {
                    //TODO
                }

                Bitmap imageBitmap = Images.BytesToBitmap(GetImage(image), imageData.format, imageData.width, imageData.height);
                return (imageData, image, imageBitmap);
            }

            SpriteDataChunk spriteData = GetSpriteData(chunk);

            var ogimageChunk = ProcessImageChunk(chunk.subChunks[0]);
            ImageDataStruct imageData = ogimageChunk.Item1;
            Chunk image = ogimageChunk.Item2;
            Bitmap imageBitmap = Images.PadWidthandHeight(ogimageChunk.Item3);

            List<Bitmap> divisions = Images.DivideBitmap(imageBitmap, 1, new int[] {spriteData.ImageWidth, spriteData.ImageHeight});

            chunk.subChunks = new List<Chunk>();

            foreach (Bitmap divs in divisions)
            {
                //Generate new chunk
                byte[] imageDataData = GenerateImageDataData(divs);
                Chunk imageDataChunk = new Chunk();
                imageDataChunk.subChunks = new List<Chunk>();
                imageDataChunk = RecalculateChunk(imageDataChunk, imageDataData);
                imageDataChunk.ChunkType = chunkOrder[IMAGEDATA_CHUNK];

                Chunk imageChunk = new Chunk();
                imageChunk.ChunkType = chunkOrder[IMAGE_CHUNK];
                imageChunk.subChunks = new List<Chunk>();
                imageChunk.subChunks.Add(imageDataChunk);
                imageChunk = RecalculateChunk(imageChunk, GenerateImageData(CalculateImageData(new ImageDataStruct(), divs, spriteData.Name)));

                chunk.subChunks.Add(imageChunk);
            }

            spriteData.ImageCount = divisions.Count;
            spriteData.BlitBorder = 0;
            chunk.Data = GenerateSpriteDataData(spriteData);
            return RecalculateChunk(chunk, chunk.Data);
        }
    }
}