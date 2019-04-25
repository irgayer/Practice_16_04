using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_16_04
{
    public class Song : Entity
    {
        public string Name { get; set; }
        public long DurationSeconds { get; set; }
        public int Rating { get; set; }
        public Band Band { get; set; }
        public ICollection<string> Words { get; set; } = new List<string>();

        


        public void Print()
        {
            Console.WriteLine($"Название     : {Name}");
            long mins, seconds;
            mins = DurationSeconds / 60;
            seconds = DurationSeconds % 60;
            Console.WriteLine($"Длительность : {mins}:{seconds}");
            Console.WriteLine($"Рейтинг      : {Rating}");
            Console.WriteLine($"Исполнитель  : {Band.Name}");
            Console.WriteLine($"Слова: ");
            foreach(var word in Words)
            {
                Console.Write(word + " ");
            }
        }
    }
}
