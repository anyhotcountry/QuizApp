public class SearchResults
{
    public string kind { get; set; }
    public Url url { get; set; }
    public Queries queries { get; set; }
    public Context context { get; set; }
    public Searchinformation searchInformation { get; set; }
    public Item[] items { get; set; }
}

public class Url
{
    public string type { get; set; }
    public string template { get; set; }
}

public class Queries
{
    public Request[] request { get; set; }
    public Nextpage[] nextPage { get; set; }
}

public class Request
{
    public string title { get; set; }
    public string totalResults { get; set; }
    public string searchTerms { get; set; }
    public int count { get; set; }
    public int startIndex { get; set; }
    public string inputEncoding { get; set; }
    public string outputEncoding { get; set; }
    public string safe { get; set; }
    public string cx { get; set; }
    public string searchType { get; set; }
}

public class Nextpage
{
    public string title { get; set; }
    public string totalResults { get; set; }
    public string searchTerms { get; set; }
    public int count { get; set; }
    public int startIndex { get; set; }
    public string inputEncoding { get; set; }
    public string outputEncoding { get; set; }
    public string safe { get; set; }
    public string cx { get; set; }
    public string searchType { get; set; }
}

public class Context
{
    public string title { get; set; }
}

public class Searchinformation
{
    public float searchTime { get; set; }
    public string formattedSearchTime { get; set; }
    public string totalResults { get; set; }
    public string formattedTotalResults { get; set; }
}

public class Item
{
    public string kind { get; set; }
    public string title { get; set; }
    public string htmlTitle { get; set; }
    public string link { get; set; }
    public string displayLink { get; set; }
    public string snippet { get; set; }
    public string htmlSnippet { get; set; }
    public string mime { get; set; }
    public Image image { get; set; }
}

public class Image
{
    public string contextLink { get; set; }
    public int height { get; set; }
    public int width { get; set; }
    public int byteSize { get; set; }
    public string thumbnailLink { get; set; }
    public int thumbnailHeight { get; set; }
    public int thumbnailWidth { get; set; }
}