using System.ComponentModel;

namespace RewardSounds.Models
{
    public class SoundObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string RewardID { get; set; }
        public int UserLevel { get; set; }
        public bool IsActive { get; set; }

    }
}
