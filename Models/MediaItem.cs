using System;
using System.Collections.Generic;
using System.Text;

namespace AnimeSearch.Models
{
    class MediaItem
    {
        MediaItem(string name, int progress, int episodes,
                  string releaseType, string startDate, 
                  string status, int rating)
        {
            Name = name;
            Progress = progress;
            Episodes = episodes;
            ReleaseType = releaseType;
            StartDate = startDate;
            Status = status;
            Rating = rating;
        }

        public string Name { get; set; }

        public int Progress { get; set; }

        public int Episodes { get; set; }

        public string ReleaseType { get; set; }

        public string StartDate { get; set; }

        public string Status { get; set; }

        public int Rating { get; set; }
    }
}
