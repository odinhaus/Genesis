using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.XNA.Primitives
{
    public class ColoredRoundedLine : GeometricPrimitive
    {
        public ColoredRoundedLine(GraphicsDevice graphicsDevice, 
            float thickness, float length, int tessellation, Color baseColor)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            BaseColor = baseColor;

            int current = DrawEndCap(0, tessellation, (float)(Math.PI / 2f), thickness / 2f, 0f);
            current = DrawEndCap(current, tessellation, (float)(3 * Math.PI / 2f), thickness / 2f, length);

            AddIndex(0); // left center
            AddIndex(tessellation + 1); // top left
            AddIndex(tessellation + 3); // top right

            AddIndex(0); // left center
            AddIndex(tessellation + 3); // top right
            AddIndex(tessellation + 2); // right center

            AddIndex(0); // left center
            AddIndex(tessellation + 2); // right center
            AddIndex(current - 1); // bottom right

            AddIndex(0); // right center
            AddIndex(current - 1); // bottom right 
            AddIndex(1); // bottom left

            InitializePrimitive(graphicsDevice);
        }

        private int DrawEndCap(int root, int tessellation, float alpha, float radius, float offsetX)
        {
            var vtx = Vector3.Zero;
            vtx.X += offsetX;

            AddVertex(vtx, Vector3.Up, BaseColor);

            float sweep = (float)(Math.PI);
            float delta = sweep / tessellation;
            float end = alpha + sweep;
            int current = root + 1;
            int count = 0;
            while (count <= tessellation)
            {
                if (current > 1)
                {
                    AddIndex(0);
                    AddIndex(current - 1);
                    AddIndex(current);
                }
                vtx = new Vector3((float)Math.Cos(alpha), 0, (float)Math.Sin(alpha)) * radius;
                vtx.X += offsetX;
                AddVertex(vtx, Vector3.Up, BaseColor);

                alpha += delta;
                current++;
                count++;
            }
            AddIndex(root);
            AddIndex(current - 1);
            AddIndex(current);

            AddIndex(root);
            AddIndex(current);
            AddIndex(root + 1);
            return current;
        }

        public Color BaseColor { get; private set; }

    }
}
