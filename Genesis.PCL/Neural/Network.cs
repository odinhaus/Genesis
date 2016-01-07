using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.PCL.Neural
{
    public class Network
    {
        public Neuron[] Neurons = new Neuron[0];

        public virtual void Update()
        {
            UpdateNeurons();
            ResetSpikedNeurons();
        }

        protected virtual void ResetSpikedNeurons()
        {
            for (int n = 0; n < Neurons.Length; n++)
            {
                Neurons[n].Reset(); // only neurons that are firing will reset
            }
        }

        protected virtual void UpdateNeurons()
        {
            for(int n = 0; n < Neurons.Length; n++)
            {
                Neurons[n].Update();
            }
        }
    }
}
