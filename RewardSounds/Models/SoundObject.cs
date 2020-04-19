using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RewardSounds.Models
{
    class SoundObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string RewardID { get; set; }
        public bool IsActive { get; set; }
    }
}
