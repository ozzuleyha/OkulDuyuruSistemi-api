using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using OkulDuyuruSistemi.Models;

namespace OkulDuyuruSistemi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public KullaniciController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpPost("add-user")]
        public JsonResult AddUser (Kullanici kullanici)
        {
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;

            string UserQuery = @"
                            insert into dbo.[Kullanici]
                            (ad, soyad, mail, parola, role_id)
                            values (@Ad, @Soyad, @Mail, @Parola, @RoleId)
                            ";

            DataTable KullaniciTable = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(UserQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Ad", kullanici.Ad);
                    myCommand.Parameters.AddWithValue("@Soyad", kullanici.Soyad);
                    myCommand.Parameters.AddWithValue("@Mail", kullanici.Mail);
                    myCommand.Parameters.AddWithValue("@Parola", kullanici.Parola);
                    myCommand.Parameters.AddWithValue("@RoleId", kullanici.RoleId);
                    myReader = myCommand.ExecuteReader();
                    KullaniciTable.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }
    }
}
