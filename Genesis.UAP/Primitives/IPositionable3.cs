using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.XNA.Primitives
{
    public interface IPositionable3
    {
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        float Radius { get; }
        BoundingSphere BoundingSphere { get; }
    }
}
