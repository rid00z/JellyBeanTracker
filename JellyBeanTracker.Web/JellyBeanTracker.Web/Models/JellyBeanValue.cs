using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PropertyChanged;

namespace JellyBeanTracker.Web.Models
{
    [ImplementPropertyChanged]
    public class JellyBeanValue
    {
        public JellyBeanValue()
        {
            Show = true;
        }

        public Guid Id { get; set; }

        [DisplayName("Jelly Bean Name")]
        public string Name { get; set; }

        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }
        public bool Show { get; set; }

        public List<decimal> Values
        {
            get
            {
                return new List<decimal> { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec };
            }
        }
        public string ReadableValues
        {
            get
            {
                return string.Join (", ", Values);
            }
        }

    }
}