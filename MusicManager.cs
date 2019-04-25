using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_16_04
{
    public class MusicManager
    {
        const int MIN_RATING_SONG = 0;
        const int MAX_RATING_SONG = 10;

        List<Band> bands;
        List<Song> songs;

        public void Run()
        {
            bands = new List<Band>();
            songs = new List<Song>();

            bool flag = true;
            Console.WriteLine("Music manager. version 1.0.0");
            while (flag)
            {  
                switch (MainMenu())
                {
                    case 1:
                        {
                            AddGroup();
                            break;
                        }
                    case 2:
                        {
                            AddSong();
                            break;
                        }
                    case 3:
                        {
                            SearchBand();
                            break;
                        }
                    case 4:
                        {
                            SearchSong();
                            break;
                        }
                    case 5:
                        {
                            PrintSongsByRating();
                            break;
                        }
                    case 6:
                        {
                            DeleteGroup();
                            break;
                        }
                    case 7:
                        {
                            DeleteSong();
                            break;
                        }
                }
            }
        }
        private int MainMenu()
        {
            using (var context = new AppContext())
            {
                bands = context.Bands.ToList();
                songs = context.Songs.ToList();
            }

            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1) Добавить группу");
            Console.WriteLine("2) Добавить песню");
            Console.WriteLine("3) Поиск по группам");
            Console.WriteLine("4) Поиск по песням");
            Console.WriteLine("5) Вывести песни по рейтингу");
            Console.WriteLine("6) Удалить группу");
            Console.WriteLine("7) Удалить песню");
            if (int.TryParse(Console.ReadLine(), out int menu))
            {
                if (menu > 0 && menu <= 7)
                {
                    return menu;
                }
            }
            return -1;
        }
        private void AddGroup()
        {
            string groupName;
            Console.WriteLine("\nВведите название группы: ");
            groupName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                Band newBand = new Band();
                newBand.Name = groupName;

                using (var context = new AppContext())
                {
                    bands.Add(newBand);
                    context.Bands.Add(newBand);
                    context.SaveChanges();
                }
            }
            else
            {
                Console.WriteLine("Пустое название группы!");
            }
        }
        private void AddSong()
        {
            string songName, songWordsString, songBandName;
            long songDuration;
            int songRating;
            List<string> songWords = new List<string>();
            Console.WriteLine("\nВведите название песни: ");
            songName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(songName))
            {

                if (bands.Count > 0)
                {
                    Song newSong = new Song();
                    newSong.Name = songName;
                    Console.WriteLine("Введите длительность песни(в секундах): ");
                    if (long.TryParse(Console.ReadLine(), out songDuration))
                    {
                        if (songDuration > 0)
                        {
                            newSong.DurationSeconds = songDuration;
                            Console.WriteLine("Введите рейтинг песни: ");

                            if (int.TryParse(Console.ReadLine(), out songRating))
                            {
                                if (songRating >= MIN_RATING_SONG && songRating <= MAX_RATING_SONG)
                                {
                                    newSong.Rating = songRating;

                                    Console.WriteLine("Добавьте слова: ");
                                    songWordsString = Console.ReadLine();
                                    string[] words = songWordsString.Split(' ');

                                    foreach (var word in words)
                                    {
                                        songWords.Add(word);
                                    }
                                    newSong.Words = songWords;

                                    Console.WriteLine("Введите название группы:\n ");
                                    foreach (var band in bands)
                                    {
                                        Console.WriteLine($"{band.Name}");
                                    }
                                    songBandName = Console.ReadLine();

                                    if (bands.Contains(bands.Where(band => band.Name.ToUpper() == songBandName.ToUpper()).FirstOrDefault()))
                                    {
                                        using (var context = new AppContext())
                                        {
                                            newSong.Band = context.Bands.Where(band => band.Name.ToUpper() == songBandName.ToUpper()).FirstOrDefault();
                                            context.Songs.Add(newSong);
                                            context.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Такой группы нет!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Рейтинг должен быть между {MIN_RATING_SONG} и {MAX_RATING_SONG} включительно");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неверный формат!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Песня не может длиться меньше 1 секунды!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат!");
                    }
                }
                else
                {
                    Console.WriteLine("Сначала нужно добавить группы!");
                }

            }
            else
            {
                Console.WriteLine("Пустое название песни!");
            }
        }

        private void SearchBand()
        {
            Console.WriteLine("Введите название группы, которую хотите найти: ");
            string bandName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(bandName))
            {
                List<Band> finded;
                finded = bands.FindAll(band => band.Name.ToLower() == bandName.ToLower());

                Console.WriteLine($"Найдено {finded.Count} групп: ");
                foreach (var band in finded)
                {
                    band.Print();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Пустая строка!");
            }
        }
        private void SearchSong()
        {
            Console.WriteLine("Введите название песни, которую хотите найти: ");
            string songName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(songName))
            {
                List<Song> finded;
                finded = songs.FindAll(song => song.Name.ToLower() == songName.ToLower());

                Console.WriteLine($"Найдено {finded.Count} песен");
                foreach (var song in finded)
                {
                    song.Print();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Пустая строка!");
            }
        }

        private void PrintSongsByRating()
        {
            Console.WriteLine("По убыванию введите 0");
            Console.WriteLine("По возрастанию другое");
            string style = Console.ReadLine();

            songs.Sort(new SongComparer());

            if (int.TryParse(style, out int dumpy))
            {
                if (dumpy == 0)
                {
                    foreach (var song in songs)
                    {
                        song.Print();
                        Console.WriteLine();
                        return;
                    }
                }
            }

            foreach (var song in songs)
            {
                for (int i = songs.Count - 1; i >= 0; i++)
                {
                    songs[i].Print();
                    Console.WriteLine();
                }
            }
        }
        private void DeleteGroup()
        {
            foreach(var band in bands)
            {
                band.Print();
                Console.WriteLine();
            }
            Console.WriteLine("Введите название группы: ");
            string groupName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                if (bands.Contains(bands.Where(band => band.Name.ToUpper() == groupName.ToUpper()).FirstOrDefault()))
                {
                    using (var context = new AppContext())
                    {
                        var bandToDelete = context.Bands.Where(band => band.Name.ToUpper() == groupName.ToUpper()).FirstOrDefault();
                        context.Entry(bandToDelete).Collection(s => s.Songs).Load();
                        context.Bands.Remove(bandToDelete);
                        context.SaveChanges();
                    }
                }
                else
                {
                    Console.WriteLine("Такой группы нет!");
                }
            }
            else
            {
                Console.WriteLine("Пустая строка!");
            }
        }
        private void DeleteSong()
        {
            foreach (var song in songs)
            {
                song.Print();
                Console.WriteLine();
            }

            Console.WriteLine("Введите название песни: ");
            string songName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(songName))
            {
                if (songs.Contains(songs.Where(song => song.Name.ToUpper() == songName.ToUpper()).FirstOrDefault()))
                {
                    using (var context = new AppContext())
                    {
                        var songToDelete = context.Songs.Where(song => song.Name.ToUpper() == songName.ToUpper()).FirstOrDefault();
                        context.Songs.Remove(songToDelete);
                        context.SaveChanges();
                    }
                }
                else
                {
                    Console.WriteLine("Такой песни нет!");
                }
            }
            else
            {
                Console.WriteLine("Пустая строка!");
            }
        }
    }
}
