using System;
using System.Collections.Generic;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Services.TweetProcessing
{
    public interface ITweetParser
    {
        public IEnumerable<string> GetEmojis(Tweet tweet);

        public IEnumerable<string> GetHashtags(Tweet tweet);

        public IEnumerable<Uri> GetUris(Tweet tweet);

        bool ContainsPhotoUrls(IEnumerable<Uri> uris);

        public IEnumerable<string> GetCryptos(Tweet tweet);
    }
}
