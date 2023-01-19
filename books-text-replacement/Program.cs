using System.Diagnostics;
using System.Text;

namespace books_text_replacement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKey key;
            do
            {
                Console.Title = "Books";
                Console.Clear();
                Console.WriteLine(@"
1: Load books
2: Modify books
3: Save Changes

");
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1: loadBooks(); break;
                    case ConsoleKey.D2: modifyBooks(); break;
                    case ConsoleKey.D3: saveBooks(); break;
                    case ConsoleKey.Escape:
                        break;
                }

            } while (!key.Equals(ConsoleKey.Escape));
            void loadBooks()
            {
                Console.Clear();
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books");
                foreach (var file in Directory.GetFiles(dir))
                {
                    // IS:
                    var book = new Book(file);

                    // SHOULD BE: using Newtonsoft.Json
                    // var book = JsonConvert.DeserializeObject<Book>(File.ReadAllText(file));

                    Console.WriteLine(book);
                    Console.WriteLine();
                }
                Console.WriteLine("Any key to continue...");
                Console.ReadKey(true);
            }
            void modifyBooks() 
            {
                Console.Clear();
                Console.WriteLine("Enter term to replace:");
                var replace = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Enter replace with:");
                var replaceWith = Console.ReadLine();

                Console.WriteLine("Any key to continue...");
                Console.ReadKey(true);
            }
            void saveBooks() 
            { }
        }
        class Book
        {
            public string? BookNumber { get; set; }
            public string? Title { get; set; }
            public string? Author { get; set; }
            public string? ISBN { get; set; }
            public string? Written { get; set; }
            public string? Release { get; set; }
            public string? Characters { get; set; }
            public string? Genre { get; set; }
            public string? Synopsis { get; set; }

            // TODO: Eliminate this using Json
            public Book(string file)
            {
                var synopsis = new StringBuilder();
                foreach (var line in File.ReadAllLines(file))
                {
                    var parse = line.Split(':').Select(_ => _.Trim()).ToArray();
                    if (parse.Length == 1)
                    {
                        synopsis.Append("\t" + parse[0] + Environment.NewLine);
                    }
                    else
                    {
                        var property = parse[0];
                        switch (property)
                        {
                            case "Book number": BookNumber = parse[1]; break;
                            case "Title": Title = parse[1]; break;
                            case "Author": Author = parse[1]; break;
                            case "ISBN": ISBN = parse[1]; break;
                            case "Written Date": Written = parse[1]; break;
                            case "Release Date": Release = parse[1]; break;
                            case "Characters": Characters = parse[1]; break;
                            case "Genre": Genre = parse[1]; break;
                            case "Synopsis": synopsis.Append("\t" + parse[1] + Environment.NewLine); break;
                            default: Debug.Assert(false, $"Error reading '{property}'"); break;
                        }
                    }
                }
                Synopsis = synopsis.ToString();
            }
            public override string ToString()
            {
                return
                    $"{BookNumber}{Environment.NewLine}" +
                    $"{Title}{Environment.NewLine}" +
                    $"{Author}{Environment.NewLine}" +
                    $"{ISBN}{Environment.NewLine}" +
                    $"{Written}{Environment.NewLine}" +
                    $"{Release}{Environment.NewLine}" +
                    $"{Characters}{Environment.NewLine}" +
                    $"{Genre}{Environment.NewLine}" +
                    $"{Synopsis}{Environment.NewLine}";
        }
        }
    }
}