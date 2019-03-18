using RSSParser.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace RSSParser.Servise
{
    public class Reader : IReader
    {
        private readonly IRssRepository _repository;

        public Reader(IRssRepository repository)
        {
            _repository = repository;
        }

        public async Task ReadAsync(List<RssSource> listSource)
        {
            List<RSS> rssList = new List<RSS>();
            foreach (var source in listSource)
            {
                if (!String.IsNullOrEmpty(source.Url))
                {
                    HttpClient httpClient = new HttpClient();
                    var rssContent = await httpClient.GetStringAsync(source.Url);
                    rssContent = rssContent.Trim('\n');
                    using (XmlReader reader = XmlReader.Create(new StringReader(rssContent)))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        Console.WriteLine(feed.Links[0].Uri);
                        foreach (SyndicationItem item in feed.Items)
                        {
                            RSS rssItem = new RSS
                            {
                                Headline = item.Title.Text,
                                Date = item.PublishDate.UtcDateTime.ToLocalTime(),
                                Description = Regex.Replace(item.Summary.Text, "<.*?>", string.Empty).Trim('\n'),
                                Url = item.Links.FirstOrDefault().Uri.ToString(),
                                SourceId = source.Id
                            };
                            rssList.Add(rssItem);
                        }
                        var newList = AddRange(rssList);
                        Console.WriteLine("Прочитано новостей: " + rssList.Count());
                        Console.WriteLine("Сохранено новостей: " + newList.Count());
                        Console.WriteLine();
                    }
                }
            }
        }

        public IEnumerable<RSS> AddRange(IEnumerable<RSS> rssRange)
        {
            var rssBD = _repository.SelectAllAsync().Result;

            List<RSS> listRss = new List<RSS>();
            listRss.AddRange(rssRange);
            foreach (var v in rssBD)
            {
                if (rssBD.Where(r => r.Headline.Contains(v.Headline) && r.Date == v.Date).Count() != 0)
                    listRss.Remove(v);
            }
            if (listRss.Count() == 0)
                return listRss;

            return _repository.AddRangeAsync(listRss).Result;
        }
    }
}
