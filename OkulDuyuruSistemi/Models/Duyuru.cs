using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkulDuyuruSistemi.Models
{
    public class Duyuru
    {
        public int Id { get; set; }
        public string DuyuruBasligi{ get; set; }
        public string DuyuruAciklama { get; set; }
        public int ToplulukId { get; set; }
    }
}
