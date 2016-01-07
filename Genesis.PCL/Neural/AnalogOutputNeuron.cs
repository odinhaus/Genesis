using System;


namespace Genesis.PCL.Neural
{
    public class AnalogOutputNeuron : IzhikevichNeuron
    {
        /// <summary>
        /// Sets the analog to input voltage conversion to the sigmoid function Tanh(MembranePotential), which will scale the 
        /// output to a value between 0 and 1 according to the Tanh curve whenever the membrane potential is updated and IsSpiking
        /// </summary>
        public AnalogOutputNeuron() 
            : this(a => Math.Tanh(a))
        { }

        /// <summary>
        /// Sets the analog to input voltage conversion to a custom analogInputToMembraneInputConverter function
        /// </summary>
        /// <param name="analogInputToMembraneInputConverter"></param>
        public AnalogOutputNeuron(Func<double, double> membranePotentialToAnalogOuputConverter)
        {
            this.SetExcitatoryDefaults();
            this.Converter = membranePotentialToAnalogOuputConverter;
        }

        /// <summary>
        /// Gets/Sets the analog input value
        /// </summary>
        public double AnalogOutput { get; set; }
        public Func<double, double> Converter { get; private set; }

        protected override void UpdateInputVoltage()
        {
            base.UpdateInputVoltage();
        }

        protected override void UpdateMembranePotential()
        {
            base.UpdateMembranePotential();
            if (IsSpiking)
            {
                AnalogOutput = Converter(MembranePotential);
            }
        }

        public override void Reset()
        {
            base.Reset();
            AnalogOutput = Converter(MembranePotential);
        }
    }
}
