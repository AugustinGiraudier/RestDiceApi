using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DiceSideTypeDTO
    {
        public long ID { get; set; }

        public DiceSideDTO prototype { get; set; }

        public int nbPrototype { get; set; }

    }
}
