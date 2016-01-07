using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.PCL.Neural
{
    public class StasisNeuron : Neuron
    {

        public StasisNeuron()
            : this ((analog) => 1 - Math.Tanh(analog / 300d))
        { }

        public StasisNeuron(Func<double, double> analogToStatisConverter)
        {
            this.Converter = analogToStatisConverter;
        }

        public Func<double, double> Converter { get; private set; }

        /// <summary>
        /// The relative measure of error for the statis.  A value of 0 means no error, and increasing 
        /// values > 0 represent higher degrees of error.  This value will be projected to a value from 0 - 1 
        /// by the Converter function used when constructing the neuron.
        /// </summary>
        public double AnalogError { get; set; }

        public override bool IsSpiking
        {
            get
            {
                return true;
            }
        }

        public override void Reset()
        {
            
        }

        public override void Update()
        {
            InputVoltage = Converter(AnalogError);
        }
    }
}
