using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.XNA.Primitives
{
    public class TexturedCirclePrimitive : TexturedGeometricPrimitive
    {
        public TexturedCirclePrimitive(GraphicsDevice graphicsDevice, float diameter, int tessellation, Texture2D texture) : base(graphicsDevice, texture)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            Diameter = diameter;
            Tessellation = tessellation;

            InitializeVertices(diameter, tessellation, 0f);
            
        }

        protected void InitializeVertices(float diameter, int tessellation, float angle)
        {
            // start with a vertex at the center
            float radius = diameter / 2f;
            AddVertex(Vector3.Zero, Vector3.Up, Vector2.Zero);

            Matrix rotate = Matrix.CreateRotationY(-angle);
            
            float pi2 = (float)(Math.PI * 2f);
            float delta = pi2 / tessellation;
            float alpha = 0;

            int current = 0;
            while (alpha < pi2)
            {
                if (current > 1)
                {
                    AddIndex(0);
                    AddIndex(current - 1);
                    AddIndex(current);
                }

                Vector3 vtx = new Vector3((float)Math.Cos(alpha), 0, (float)Math.Sin(alpha));
                
                Matrix trx = new Matrix();
                trx.M11 = vtx.X;
                trx.M12 = vtx.Y;
                Matrix rotated = trx * rotate;

                AddVertex(vtx * radius, Vector3.Up, new Vector2(rotated.M11 * Texture.Width, rotated.M12 * Texture.Height));

                alpha += delta;
                current++;
            }
            AddIndex(0);
            AddIndex(current - 1);
            AddIndex(current);
            AddIndex(0);
            AddIndex(current);
            AddIndex(1);
            InitializePrimitive();
        }

        public void RotateTexture(float angle)
        {
            InitializeVertices(Diameter, Tessellation, angle);
        }

        public float Diameter { get; private set; }

        public int Tessellation { get; private set; }
    }
}
