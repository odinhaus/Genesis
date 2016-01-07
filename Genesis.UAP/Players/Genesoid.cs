using Genesis.XNA.Primitives;
using Genesis.XNA.Shapes.TwoD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.XNA.Players
{
    public class Genesoid : IMovable3
    {
        private Vector3 _pos = Vector3.Zero;
        public Vector3 Position
        {
            get { return _pos; }
            set
            {
                Vector3 delta = value - _pos;
                face.Position += delta;
                left.Position += delta;
                right.Position += delta;
                butt.Position += delta;
                _pos = value;
            }
        }

        private Vector3 _vel = Vector3.Zero;
        public Vector3 Velocity
        {
            get { return _vel; }
            set
            {
                face.Velocity = value;
                right.Velocity = value;
                left.Velocity = value;
                butt.Velocity = value;
                _vel = value;
            }
        }
        public Color Color = Color.White;
        Arc face;
        Arc right;
        Arc left;
        Arc butt;
        Arc tracking;

        public Genesoid(GraphicsDevice graphics, float radius, Color baseColor)
        {
            Radius = radius;
            Color = baseColor;
            face = new Arc(graphics, radius, 64, radius / 4f, Primitives.FillStyle.Center, (float)(Math.PI / 4f), (float)(Math.PI / 2f), baseColor, radius / 12f, Color.Transparent, radius / 12f, Color.Transparent);
            right = new Arc(graphics, radius, 64, radius / 8f, Primitives.FillStyle.Center, (float)(-Math.PI / 4f), (float)(Math.PI / 2f), Color.Yellow, radius / 12f, Color.Transparent, radius / 12f, Color.Transparent);
            left = new Arc(graphics, radius, 64, radius / 8f, Primitives.FillStyle.Center, (float)(3f * Math.PI / 4f), (float)(Math.PI / 2f), Color.Green, radius / 12f, Color.Transparent, radius / 12f, Color.Transparent);
            butt = new Arc(graphics, radius, 64, radius / 4f, Primitives.FillStyle.Center, (float)(5f * Math.PI / 4f), (float)(Math.PI / 2f), baseColor, radius / 12f, Color.Transparent, radius / 12f, Color.Transparent);
            tracking = new Arc(graphics, radius, 64, radius / 12f, Primitives.FillStyle.Outside, (float)(5f * Math.PI / 4f), (float)(Math.PI / 2f), Color.HotPink, radius / 12f, Color.Transparent, radius / 12f, Color.Transparent);
            BoundingSphere = new BoundingSphere(_pos, Radius);
            Rotation = Vector3.Zero;
        }

        public float Radius { get; private set; }
        public bool IsTracking { get; set; }
        public BoundingSphere BoundingSphere { get; private set; }
        public Vector3 Rotation { get; set; }

        public void Update(GameTime gameTime)
        {
            Vector3 dp = Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pos += dp;
            BoundingSphere = new BoundingSphere(_pos, Radius);
            face.Rotation = this.Rotation;
            face.Update(gameTime);
            right.Rotation = this.Rotation;
            right.Update(gameTime);
            left.Rotation = this.Rotation;
            left.Update(gameTime);
            butt.Rotation = this.Rotation;
            butt.Update(gameTime);
            if (IsTracking)
                tracking.Update(gameTime);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            face.Draw(view, projection);
            left.Draw(view, projection);
            right.Draw(view, projection);
            butt.Draw(view, projection);
            if (IsTracking)
                tracking.Draw(view, projection);
        }
    }
}
