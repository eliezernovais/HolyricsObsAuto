using System.Text.Json;

namespace HolyricsObsAuto.Holyrics
{
    public class HolyricsTextResponse
    {
        public HolyricsMap map { get; set; }
    }

    public class HolyricsMap
    {
        public string type { get; set; }
        public string text { get; set; }
    }

}
