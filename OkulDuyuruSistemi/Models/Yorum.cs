using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkulDuyuruSistemi.Models
{
    public class Yorum
    {

        public int Id { get; set; }
        public string YorumAciklama { get; set; }
        public int DuyuruId { get; set; }
        public int KullaniciId { get; set; }
    }
}
