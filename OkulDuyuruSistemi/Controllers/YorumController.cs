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
    public class YorumController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public YorumController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpPost("add-yorum")]
        public JsonResult AddYorum(Yorum yorum)
        {
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;

            string UserQuery = @"
                            insert into dbo.[Yorum]
                            (kullanici_id, duyuru_id, yorum_aciklama)
                            values (@KullaniciId, @DuyuruId, @Yorum)
                            ";

            DataTable DuyuruTable = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(UserQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@KullaniciId", yorum.KullaniciId);
                    myCommand.Parameters.AddWithValue("@DuyuruId", yorum.DuyuruId);
                    myCommand.Parameters.AddWithValue("@Yorum", yorum.YorumAciklama);
                    myReader = myCommand.ExecuteReader();
                    DuyuruTable.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpGet("yorum-list")]
        public JsonResult getYorumList ()
        {
            string query = @"
                            SELECT * FROM
                            dbo.Yorum
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(table);
        }


    }
}
