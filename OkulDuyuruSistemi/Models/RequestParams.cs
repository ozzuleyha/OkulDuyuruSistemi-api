using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkulDuyuruSistemi.Models
{
    public class RequestParams
    {
        public Duyuru Duyuru { get; set; }
        public Topluluk Topluluk { get; set; }
        public Kullanici Kullanici { get; set; }

    }
}
