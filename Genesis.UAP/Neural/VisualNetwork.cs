using Genesis.PCL.Neural;
using Genesis.XNA.Shapes.TwoD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.UAP.Neural
{
    public class VisualNetwork : Network
    {
        public VisualNetwork(GraphicsDevice graphicsDevice, IEnumerable<Neuron> sensoryNeurons, IEnumerable<Neuron> interNeurons, IEnumerable<Neuron> responsiveNeurons)
        {
            this.SensoryNeurons = sensoryNeurons;
            this.InterNeurons = interNeurons;
            this.ResponsiveNeurons = responsiveNeurons;
            this.Neurons = sensoryNeurons.Union(interNeurons).Union(responsiveNeurons).ToArray();
        }

        public IEnumerable<Neuron> SensoryNeurons { get; private set; }
        public IEnumerable<Neuron> InterNeurons { get; private set; }
        public IEnumerable<Neuron> ResponsiveNeurons { get; private set; }

        public IList<Circle> SensoryGlyphs { get; private set; }
        public IList<Circle> InterGlyphs { get; private set; }
        public IList<Circle> ResponsiveGlyphs { get; private set; }

        public void Update(GameTime gameTime)
        {
            base.Update();
        }

        long _count = 0;
        protected override void ResetSpikedNeurons()
        {
            var sb = new StringBuilder();
            sb.Append(_count);

            foreach (var n in Neurons)
            {
                sb.Append(", ");
                sb.Append(((IzhikevichNeuron)n).MembranePotential);
            }

            Debug.WriteLine(sb.ToString());
            base.ResetSpikedNeurons();
            _count++;
        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
