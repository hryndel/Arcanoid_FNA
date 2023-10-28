using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arcanoid_FNA.Resources
{
    public static class TextureLoader
    {
        const bool usingPipeline = false;

        public static Texture2D Load(string filePath, ContentManager content)
        {
            Texture2D image = content.Load<Texture2D>(filePath);

            if (usingPipeline == false)
                PremultiplyTexture(image);
            return image;
        }

        private static void PremultiplyTexture(Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }
    }
}
