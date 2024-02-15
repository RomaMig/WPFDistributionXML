namespace Parser.Interface
{
    public interface IXmlParser
    {
        void Load(string path);
        IPage Parse();
    }
}
