using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoliSocialWebApi.ViewModels.News
{
    public class PublishNews
    {
        public string InstId { get; set; }

        public string Banner { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Corpo { get; set; }
        public List<ImageDesc> ImageList {get;set;}
        
        public class ImageDesc
        {
            public string Image { get; set; }
            public string Descricao { get; set; }
        }



    }

    public class PublishNewsGet
    {
        public long NewsId { get; set; }
    }
}
