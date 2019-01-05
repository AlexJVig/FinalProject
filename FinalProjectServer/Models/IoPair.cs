using System.Collections.Generic;

namespace FinalProjectServer.Models
{
    public class IoPair
    {
        public ICollection<double> Input
        {
            get;
            set;
        }

        public ICollection<double> Output
        {
            get;
            set;
        }
    }
}