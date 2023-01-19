using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace books_text_replacement
{
    internal class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);
        static void Main(string[] args)
        {
            #region D E C L A R A T I O N S
            List<Book> Books = new List<Book>();
            ConsoleKey key;
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Books");
            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
            #endregion D E C L A R A T I O N S

            do
            {
                Console.Title = "Books";
                Console.Clear();
                Console.WriteLine(@"
1: Load books
2: Modify books
3: Save Changes
4: Open Folder in File Explorer

");
                BringWindowToTop(hWnd);
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.D1: loadBooks(); break;
                    case ConsoleKey.D2: modifyBooks(); break;
                    case ConsoleKey.D3: saveBooks(); break;
                    case ConsoleKey.D4: Process.Start("explorer.exe", dir); break;
                    case ConsoleKey.Escape:
                        break;
                }

            } while (!key.Equals(ConsoleKey.Escape));
            void loadBooks()
            {
                Books.Clear();
                Console.Clear();
                foreach (var file in Directory.GetFiles(dir, "*.txt"))
                {
                    // IS:
                    Books.Add(new Book(file));

                    // SHOULD BE: using Newtonsoft.Json
                    // var book = JsonConvert.DeserializeObject<Book>(File.ReadAllText(file));

                    Console.WriteLine(string.Join(Environment.NewLine, Books));
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
                foreach (var book in Books)
                {
                    book.Synopsis = book.Synopsis.Replace(replace, replaceWith);
                }
                Console.Clear();
                Console.WriteLine(string.Join(Environment.NewLine, Books));

                Console.WriteLine("Any key to continue...");
                Console.ReadKey(true);
            }
            void saveBooks() 
            {
                foreach (var book in Books)
                {
                    var path = Path.Combine(dir, $"{book.ISBN}.json");
                    File.WriteAllText(path, JsonConvert.SerializeObject(book, Formatting.Indented));
                }
            }
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
                            case "Synopsis": synopsis.Append(parse[1] + Environment.NewLine); break;
                            default: Debug.Assert(false, $"Error reading '{property}'"); break;
                        }
                    }
                }
                Synopsis = synopsis.ToString();
            }
            public override string ToString()
            {
                return
                    $"Book number : {BookNumber}{Environment.NewLine}" +
                    $"Title : {Title}{Environment.NewLine}" +
                    $"Author : {Author}{Environment.NewLine}" +
                    $"ISBN : {ISBN}{Environment.NewLine}" +
                    $"Written Date : {Written}{Environment.NewLine}" +
                    $"Release Date : {Release}{Environment.NewLine}" +
                    $"Characters : {Characters}{Environment.NewLine}" +
                    $"Genre : {Genre}{Environment.NewLine}" +
                    $"Synopsis : {Synopsis}{Environment.NewLine}";
        }
        }
    }
}