using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.PCL.Neural
{
    /// <summary>
    /// A model implmentation of the Izhikevich Spiking Neuron, as described in http://www.izhikevich.org/publications/spikes.pdf.
    /// Sample defaults used:
    /// Excitatory Neurons
    /// (ai; bi) = (0:02; 0:2) and (ci; di) = (-65;8) + (-15;-6)r^2, where r is a random variable uniformly distributed on the interval[0, 1]
    /// Inhibitory Neurons
    /// (ai; bi) = (0:02; 0:25) + (0:08; -0:05)r and(ci; di)=(-65; 2).
    /// 
    /// The network loop simply calls Update() on the neuron to advance its membrane and input values.  Once the 
    /// </summary>
    public class IzhikevichNeuron : Neuron
    {
        public IzhikevichNeuron(bool isInhibitory = false)
        {
            if (isInhibitory)
                SetInhibitoryDefaults();
        }
        /// <summary>
        /// The timescale of the recovery variable (a)
        /// </summary>
        public double MembrameRecoveryTimescale = 0.02d;
        /// <summary>
        /// The sensitivity of the recovery variable (b)
        /// </summary>
        public double MembraneRecoverySensitivity = 0.2d;
        /// <summary>
        /// The membrane reset value after spike (c)
        /// </summary>
        public double MembranePotentialReset = -65d -15d * Math.Pow(_random.NextDouble(), 2);
        /// <summary>
        /// The membrane recovery reset value after recovery (d)
        /// </summary>
        public double MembraneRecoveryReset = 8d - 6d * Math.Pow(_random.NextDouble(), 2);
        /// <summary>
        /// The lower limit defining if a neuron will fire a spike to its axon-connected neurons
        /// </summary>
        public double MembraneThreshold = 30d;
        /// <summary>
        /// The membrane potential (v)
        /// </summary>
        public double MembranePotential = -65d;
        /// <summary>
        /// The membrane recovery (u)
        /// </summary>
        public double MembraneRecovery = 0.2d * -65d;
        public override bool IsSpiking { get { return MembranePotential > MembraneThreshold; } }
        /// <summary>
        /// Applies updates to the MembranePotential, MembraneRecovery and InputVoltage values
        /// </summary>
        public override void Update()
        {
            /*
            Bifurcation methodologies [8] enable us to reduce many biophysically
            accurate Hodgkin–Huxley-type neuronal models to a two-dimensional
            (2-D) system of ordinary differential equations of the form

            v' = 0.04v^2 + 5v + 140u + I
            u' = a(bv-u)

            with the auxiliary after-spike resetting

            if v >= 30 mV; 
            then v = c,  u = u + d

            v[i] += 0.5 * ( 0.04 * (v[i]*v[i]) + 5.0 * v[i] + 140 - u[i] + I[i]);
			v[i] += 0.5 * ( 0.04 * (v[i]*v[i]) + 5.0 * v[i] + 140 - u[i] + I[i]);
			u[i] += a[i] * ( b[i] * v[i] - u[i] );
            */

            UpdateInputVoltage();
            UpdateMembranePotential();
            UpdateMembraneRecovery();
        }
        /// <summary>
        /// Resets the MemberanePotential and MembraneRecovery is the neuron is currently firing, setting IsSpiking to false
        /// </summary>
        public override void Reset()
        {
            if (IsSpiking)
            {
                MembranePotential = MembranePotentialReset;
                MembraneRecovery += MembraneRecoveryReset;
            }
        }

        protected virtual void UpdateInputVoltage()
        {
            // gather inputs from Axons
            for (int s = 0; s < Dendrites.Length; s++)
            {
                if (Dendrites[s].Source.IsSpiking)
                    InputVoltage += Dendrites[s].Weight; // we simply increase the voltage by the weight of the connection
            }
        }

        protected virtual void UpdateMembranePotential()
        {
            // the differential update is split into two updates to provide numeric stabililty
            MembranePotential += 0.5d * (0.04d * MembranePotential * MembranePotential + 5d * MembranePotential + 140 - MembraneRecovery + InputVoltage);
            MembranePotential += 0.5d * (0.04d * MembranePotential * MembranePotential + 5d * MembranePotential + 140 - MembraneRecovery + InputVoltage);
            InputVoltage = 0d;
        }

        protected virtual void UpdateMembraneRecovery()
        {
            MembraneRecovery = MembrameRecoveryTimescale * (MembraneRecoverySensitivity * MembranePotential - MembraneRecovery);
        }

        public void SetExcitatoryDefaults()
        {
            MembrameRecoveryTimescale = 0.02d;
            MembraneRecoverySensitivity = 0.2d;
            MembranePotentialReset = -65d - 15d * Math.Pow(_random.NextDouble(), 2);
            MembraneRecoveryReset = 8d - 6d * Math.Pow(_random.NextDouble(), 2);
            MembraneThreshold = 30d;
            MembranePotential = -65d;
            MembraneRecovery = 0.2d * -65d;
        }

        public void SetInhibitoryDefaults()
        {
            MembrameRecoveryTimescale = 0.02d + 0.08d * _random.NextDouble();
            MembraneRecoverySensitivity = 0.25d - 0.05 * _random.NextDouble();
            MembranePotentialReset = -65d;
            MembraneRecoveryReset = 2d;
            MembraneThreshold = 30d;
            MembranePotential = -65d;
            MembraneRecovery = 0.2d * -65d;
        }
    }
}
