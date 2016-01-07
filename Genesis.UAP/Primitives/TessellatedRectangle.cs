using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.XNA.Primitives
{
    public class TessellatedRectangle : GeometricPrimitive
    {
        public TessellatedRectangle(GraphicsDevice graphicsDevice, Vector2 size, Vector2 tesselation, Color baseColor)
            : this(graphicsDevice, size, tesselation, (vector) => baseColor)
        { }

        public TessellatedRectangle(GraphicsDevice graphicsDevice, Vector2 size, Vector2 tesselation, Func<Vector3, Color> baseColorSelector)
        {
            var cols = size.X / tesselation.X;
            var rows = size.Y / tesselation.Y;

            for(float x = 0f; x <= size.X; x+= cols)
            {
                for (float y = 0f; y <= size.Y; y += rows)
                {
                    var pos = new Vector3(x, 0, y);
                    AddVertex(pos, Vector3.Up, baseColorSelector(pos));
                }
            }

            for (int x = 0; x < cols; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    //AddIndex()
                }
            }
        }
    }
}
