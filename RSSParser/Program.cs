using Ninject;
using RSSParser.Repository;
using RSSParser.Servise;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RSSParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();

            kernel.Bind<IReader>().To<Reader>();
            kernel.Bind<Reader>().ToSelf();

            kernel.Bind<IRssRepository>().To<RssRepository>();
            kernel.Bind<RssRepository>().ToSelf();

            kernel.Bind<ISourceRepository>().To<SourceRepository>();
            kernel.Bind<SourceRepository>().ToSelf();

            List<RssSource> listSource = new List<RssSource>();
            using ( RSScontext context = new RSScontext())
            {
                RssSource source1 = new RssSource();
                RssSource source2 = new RssSource();

                if (context.RssSources.Count() == 0)
                {
                    source1 = new RssSource { Title = "Интерфакс", Url = "https://www.interfax.by/news/feed" };
                    source2 = new RssSource { Title = "Хабрахабр", Url = "https://habr.com/ru/rss/all/all/" };

                    context.RssSources.Add(source1);
                    context.RssSources.Add(source2);
                    context.SaveChanges();
                }
                listSource.AddRange(context.RssSources);
            }

            kernel.Get<IReader>().ReadAsync(listSource);

            Console.ReadKey();
        }
    }
}
