using System;


namespace Genesis.PCL.Neural
{
    public class AnalogInputNeuron : IzhikevichNeuron
    {
        /// <summary>
        /// Sets the analog to input voltage conversion to the sigmoid function 30 * Tanh(AnalogValue)
        /// </summary>
        public AnalogInputNeuron() 
            : this(a => 30d * Math.Tanh(a))
        { }

        /// <summary>
        /// Sets the analog to input voltage conversion to a custom analogInputToMembraneInputConverter function
        /// </summary>
        /// <param name="analogInputToMembraneInputConverter"></param>
        public AnalogInputNeuron(Func<double, double> analogInputToMembraneInputConverter)
        {
            this.SetExcitatoryDefaults();
            this.Converter = analogInputToMembraneInputConverter;
        }

        /// <summary>
        /// Gets/Sets the analog input value
        /// </summary>
        public double AnalogInput { get; set; }
        public Func<double, double> Converter { get; private set; }

        protected override void UpdateInputVoltage()
        {
            this.InputVoltage = Converter(AnalogInput);
        }
    }
}
