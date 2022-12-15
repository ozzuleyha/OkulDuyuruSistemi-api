using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkulDuyuruSistemi.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string Ad{ get; set; }
        public string Soyad { get; set; }
        public string Mail { get; set; }
        public string Parola { get; set; }
        public int RoleId { get; set; }


    }
}
