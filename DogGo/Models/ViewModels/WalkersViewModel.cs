using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkersViewModel
    {
        public Walker Walker { get; set; }
        public List<Walks> Walks { get; set; }
        public int WalkTime{ get; set; }
    }
}
