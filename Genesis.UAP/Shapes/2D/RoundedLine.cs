using Genesis.XNA.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.XNA.Shapes._2D
{
    public class RoundedLine : IMovable3
    {
        private ColoredRoundedLine primitive;

        public Color Color = Color.White;

        public RoundedLine(GraphicsDevice graphics, float thickness, float length, int tellselation, Color baseColor)
            : this(graphics, thickness, length, tellselation, baseColor, null)
        {
        }

        public RoundedLine(GraphicsDevice graphics, float thickness, float length, int tellselation, Color baseColor, Effect customEffect)
        {
            primitive = new ColoredRoundedLine(graphics, thickness, length, tellselation, baseColor);
            primitive.CustomEffect = customEffect;
            Color = baseColor;
            Length = length;
            Thickness = thickness;
            Rotation = Vector3.Zero;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            if (Rotation == null)
            {
                Rotation = Vector3.Zero;
            }

            var world = Matrix.CreateRotationX(Rotation.X) 
                      * Matrix.CreateRotationY(Rotation.Y)
                      * Matrix.CreateRotationZ(Rotation.Z)
                      * Matrix.CreateTranslation(Position);

            primitive.Draw(world, view, projection, Color);
        }

        public void ColorVertex(GraphicsDevice gd, int index, Color color)
        {
            primitive.ColorVertex(gd, index, color);
        }

        public void ColorVertices(GraphicsDevice gd, int[] indices, Color[] colors)
        {
            primitive.ColorVertices(gd, indices, colors);
        }

        public int Vertices { get { return primitive.Vertices; } }

        public Vector3 Velocity { get; set; }

        public Vector3 Position { get; set; }

        public BoundingSphere BoundingSphere { get { return new BoundingSphere(Position, Length); } }

        public float Length { get; private set; }
        public float Thickness { get; private set; }
        public float Radius { get { return Length / 2f; } }
        public Vector3 Rotation { get; set; }
    }
}
