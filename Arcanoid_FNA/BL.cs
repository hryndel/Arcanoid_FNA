using Arcanoid_FNA.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Arcanoid_FNA
{
    public class BL : Game
    {
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        private Texture2D Platform, Back, Ball, Block;

        private int PositionPlatformX = 0, Kfx = 1, Kfy = 1;
        private bool Defeat = false;

        private readonly IList<Block> Blocks;
        private static KeyboardState key = Keyboard.GetState();
        private Ball BallXY = new Ball();

        public BL()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Resources";
            Graphics.PreferredBackBufferHeight = 1000;
            Graphics.PreferredBackBufferWidth = 840;
            BallXY.x = Graphics.PreferredBackBufferHeight / 2; 
            BallXY.y = Graphics.PreferredBackBufferHeight - 120;

            Blocks = new List<Block>();
            CreateBlocks();
            Graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Back = TextureLoader.Load("background", Content);
            Platform = TextureLoader.Load("platform", Content);
            Ball = TextureLoader.Load("ball", Content);
            Block = TextureLoader.Load("block", Content);
        }

        protected override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Back, new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight), Color.White);
            SpriteBatch.Draw(Platform, new Rectangle(PositionPlatformX, Graphics.PreferredBackBufferHeight - 60, 150, 30), Color.White);
            SpriteBatch.Draw(Ball, new Rectangle(BallXY.x, BallXY.y, 50, 50), Color.White);
            DisplayBlocks();
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!Defeat)
            {
                Press();
                MoveBall();
                DestroyBlock();
            }
            base.Update(gameTime);
        }

        public void Press()
        {
            key = Keyboard.GetState();
            if (key.IsKeyDown(Keys.Left))
            {
                if (PositionPlatformX > 0)
                {
                    PositionPlatformX -= 10;
                }
            }
            else if (key.IsKeyDown(Keys.Right))
            {
                if (PositionPlatformX < Graphics.PreferredBackBufferWidth - 150)
                {
                    PositionPlatformX += 10;
                }
            }
        }

        public void MoveBall()
        {
            if (BallXY.x >= Graphics.PreferredBackBufferWidth - 50)
                Kfx = -1;
            else if (BallXY.x <= 0)
                Kfx = 1;

            if (BallXY.y >= Graphics.PreferredBackBufferHeight - 50)
                Defeat = true;
            else if (BallXY.y <= 0)
                Kfy = -1;

            if ((BallXY.x >= PositionPlatformX || BallXY.x + 50 >= PositionPlatformX) && 
                (BallXY.x <= PositionPlatformX + 150 || BallXY.x + 50 <= PositionPlatformX + 150) &&
                BallXY.y + 50 == Graphics.PreferredBackBufferHeight - 60)
            {
                Kfy *= -1;
            }

            BallXY.y -= 10 * Kfy;
            BallXY.x += 10 * Kfx;
        }

        private void CreateBlocks()
        {
            int placeX = 0, placeY = 0;
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Blocks.Add(new Block
                    {
                        x = placeX,
                        y = placeY,
                        color = random.Next(0, 4)
                    });
                    placeX += 120;
                }
                placeY += 40;
                placeX = 0;
            }
        }

        private void DisplayBlocks()
        {
            Color color = Color.White;
            foreach (var block in Blocks)
            {
                switch (block.color)
                {
                    case 0: color = Color.White; break;
                    case 1: color = Color.Aquamarine; break;
                    case 2: color = Color.GreenYellow; break;
                    case 3: color = Color.MediumPurple; break;
                }
                SpriteBatch.Draw(Block, new Rectangle(block.x, block.y, 120, 40), color);
            }
        }

        private void DestroyBlock()
        {
            foreach (var block in Blocks)
            {
                if ((BallXY.y == block.y + 40 || BallXY.y + 50 == block.y) &&
                    ((BallXY.x >= block.x && BallXY.x <= block.x + 120) || (BallXY.x + 50 >= block.x && BallXY.x + 50 <= block.x + 120)))
                {
                    Kfy *= -1;
                    Blocks.Remove(block);
                    break;
                }

                if ((BallXY.y <= block.y + 40 || BallXY.y + 50 <= block.y + 40) && (BallXY.y >= block.y || BallXY.y + 50 >= block.y) && (BallXY.x + 50 == block.x || BallXY.x == block.x + 120))
                {
                    Kfx *= -1;
                    Blocks.Remove(block);
                    break;
                }
            }
            if (Blocks.Count == 0)
            {
                Defeat = true;
            }
        }
    }
}
