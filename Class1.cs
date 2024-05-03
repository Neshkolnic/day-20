using System;
using System.Collections.Generic;

public class Author
{
    public string FullName { get; set; }
    public string Country { get; set; }
    public int ID { get; set; }
    // Другие свойства, если нужно
}

public class BookFile
{
    public string Format { get; set; }
    public long FileSize { get; set; }
    public string Title { get; set; }
    public string UDC { get; set; }
    public int PageCount { get; set; }
    public string Publisher { get; set; }
    public int Year { get; set; }
    public List<Author> Authors { get; set; }
    public DateTime UploadDate { get; set; }

    public BookFile()
    {
        Authors = new List<Author>();
    }
}
