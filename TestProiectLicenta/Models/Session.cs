using System.Collections.Generic;

namespace TestProiectLicenta.Models
{
    public class Session
    {
        public int UserId { get; set; }
        public string Key { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
