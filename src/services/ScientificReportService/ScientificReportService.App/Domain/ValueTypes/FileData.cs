namespace ScientificReportService.App.Domain.ValueTypes;

public struct FileData
{
    public Stream FileStream { get; private init; }
    
    public string ContentType { get; private init; }
    
    public string FileName { get; private init; }

    public FileData(Stream fileFileStream, string contentType, string fileName)
    {
        FileStream = fileFileStream;
        ContentType = contentType;
        FileName = fileName;
    }
}