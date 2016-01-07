using Genesis.XNA.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.XNA.Shapes.TwoD
{
    public class Circle : IMovable3
    {
        private ColoredCirclePrimitive primitive;

        public Color Color = Color.White;

        public float Radius { get; private set; }

        public Circle(GraphicsDevice graphics, float radius, int tellselation, Color baseColor)
            : this(graphics, radius, tellselation, baseColor, null)
        {
        }

        public Circle(GraphicsDevice graphics, float radius, int tellselation, Color baseColor, Effect customEffect) 
        {
            primitive = new ColoredCirclePrimitive(graphics, radius * 2f, tellselation, baseColor);
            primitive.CustomEffect = customEffect;
            Radius = radius;
            Color = baseColor;
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Matrix view, Matrix projection)
        {
            primitive.Draw(Matrix.CreateTranslation(Position), view, projection, Color);
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

        public Vector3 Rotation { get; set; }

        public BoundingSphere BoundingSphere
        {
            get { return new BoundingSphere(Position, Radius); }
        }
    }
}
