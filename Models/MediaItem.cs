namespace AnimeSearch.Models
{
    class MediaItem
    {
        public MediaItem() { }

        public string Name { get; set; }

        public int Progress { get; set; }

        public int Episodes { get; set; }

        public string ReleaseType { get; set; }

        public string StartDate { get; set; }

        public string Status { get; set; }

        public int Rating { get; set; }
    }
}
