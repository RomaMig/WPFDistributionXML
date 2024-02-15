using Parser.Interface;

namespace Parser
{
    public class Page : IPage
    {
        public int FormatVersion { get; set; } 
        public string From { get; set; } 
        public string To { get; set; }
        public int Id {  get; set; }
        public string Text { get; set; }
        public string TextColor { get; set; }
        public string Image {  get; set; }
    }
}
