using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.PCL.Neural
{
    public abstract class Neuron
    {
        protected static Random _random = new Random(1000); // we seed this to generate reproducible results
        /// <summary>
        /// Custom data assigned to the neuron by the utilizing system
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Represents connections that will provide a spike to this neuron when they fire
        /// </summary>
        public Synapse[] Dendrites = new Synapse[0];
        /// <summary>
        /// The input voltage from axons or inputs (I)
        /// </summary>
        public double InputVoltage = 0;
        /// <summary>
        /// Indicates whether the neuron is currently in a spiking state
        /// </summary>
        public abstract bool IsSpiking { get; }
        /// <summary>
        /// Updates the membrane potential for the neuron
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// Resets spiked neurons to recovery state
        /// </summary>
        public abstract void Reset();
    }
}
