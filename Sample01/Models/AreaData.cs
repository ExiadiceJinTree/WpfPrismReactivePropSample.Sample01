using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample01.Models
{
    public sealed class AreaData
    {
        public int Value { get; set; }
        
        public string Name { get; set; }


        public AreaData(int value, string name)
        {
            this.Value = value;
            this.Name = name;
        }
    }
}
