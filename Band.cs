using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_16_04
{
    public class Band : Entity
    {
        public string Name { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();

        public void Print()
        {
            Console.WriteLine($"Название         : {Name}");
            Console.WriteLine($"Количество песен : {Songs.Count}");
        }
    }
}
