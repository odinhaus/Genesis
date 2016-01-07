using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.PCL.Neural
{
    public class Synapse
    {
        /// <summary>
        /// The source of a spike
        /// </summary>
        public Neuron Source;
        /// <summary>
        /// The receiver of a spike
        /// </summary>
        public Neuron Target;
        /// <summary>
        /// Represents the strength of the connection between neurons.
        /// </summary>
        public double Weight;
        /// <summary>
        /// Represents the measure of the synapse's ability to alter its weight, 1.0 being the most 
        /// adapaptable, and 0.0 being non-changeable. Plasticity is decreased every time a reward statis is signaled 
        /// across the network.
        /// </summary>
        public double Plasticity = 1.0;
        /// <summary>
        /// Represents a simple feedback mechanism, indicating the de.  The neuron will apply changes to synaptic weights based on the 
        /// the current statis state, in combination with whether the neuron has fired recently, or not.  Recent firing 
        /// in conjunction with reward stasis (+ output) will serve to repeat the last change to synaptic weights that contributed to the firing, 
        /// whereas punishment (- output) will serve to undo the changes in synaptic weight that lead to the firing.  
        /// </summary>
        public StasisNeuron[] Stasis = null;
        /// <summary>
        /// Updates the synaptic weight and plasticity values based on the current statis
        /// </summary>
        public void Update()
        {

        }
    }
}
