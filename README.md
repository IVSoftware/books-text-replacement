Your post states that you **need to replace a string inside with something else** and you _could_ consider the entire file as "one long string" and do some processing on that. The `Regex` solution is probably a great way to go in that case. 

But I read your code carefully from the perspective of what you're actually trying to _do_. Ok, so my crystal ball isn't' 100% but I believe that if we meet back up a few weeks from now, it will have occurred to you want certain elements:

- A 'class' that represents a Book.
- A _serialization method_ (like Json) that can take a file and turn it into a Book and vice-versa.
- A way to search these Books (like SQLite) based on the properties in the Book class.

***
**Book**

This string replacement that you're wanting to do. Wouldn't it be easier if you had the `Synopsis` all separated out so that you could perform that action in a targeted way?

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
        // Display
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

***
**Serialization** - The hard way

Here's a method that uses the string representations in your post to turn a "file" into a `Book`.

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

***
The solution I would offer to your original question about **Replace Value Text Over Multiple Lines** is to turn it into a class and apply the Replace to one of the book's properties.'

Then you can easily perform a string replacement on the `Synopsis` property. For example:

    book = new Book("492C9F2A7E73.txt");
    book.Synopsis = book.Synopsis.Replace("Blah", "Marklar");

***
But also please consider using something like the Newtonsoft.Json NuGet to simplify your serialization. It still writes the file in plain text and you'll even see the ':' character used in a similar way to your file listings. But the format lets Json reconstuct a `Book` object directly.

    var path = Path.Combine(dir, $"{book.ISBN}.json");
    File.WriteAllText(path, JsonConvert.SerializeObject(book));

Result in file (this is after doing the replacement):

    {
      "BookNumber": "2",
      "Title": "Something Title",
      "Author": "Some Author",
      "ISBN": "7E092CB94CCD",
      "Written": "Some date",
      "Release": "Some date",
      "Characters": "Blah Blah",
      "Genre": "Romance",
      "Synopsis": "Blah Blah Blah Blah Blah Blah Blah Blah\r\n\tBlah Blah Blah Blah Blah Blah Blah Blah\r\n\tBlah Blah Blah Blah Blah Blah Blah Blah\r\n\tBlah Blah Blah Blah Blah Blah Blah Blah\r\n"
    }

_or_
 
    var book = JsonConvert.DeserializeObject<Book>(File.ReadAllText("492C9F2A7E73.txt"));

There's more than one way to do what you asked. The benefit of doing something like this is to set you up going forward for a search engine using the Book class.




