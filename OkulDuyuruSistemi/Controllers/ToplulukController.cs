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
    public class ToplulukController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public ToplulukController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("topluluk-listesi")]
        public JsonResult getToplulukList()
        {
            string query = @"
                            SELECT * FROM
                            dbo.Topluluk
                            ";

            DataTable ToplulukTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    ToplulukTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(ToplulukTable);
        }

        [HttpPost("add-topluluk")]
        public JsonResult addTopluluk(RequestParams requestParams)
        {
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;

            string UserQuery = @"
                            insert into dbo.[Topluluk]
                            (akademisyen_id, topluluk_adi)
                            values (@AkademisyenId, @ToplulukAdi)
                            ";

            DataTable ToplulukTable = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(UserQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@YoneticiId", requestParams.Kullanici.Id);
                    myCommand.Parameters.AddWithValue("@AkademisyenId", requestParams.Kullanici.Id);
                    myCommand.Parameters.AddWithValue("@ToplulukAdi", requestParams.Topluluk.ToplulukAdi);
                    myReader = myCommand.ExecuteReader();
                    ToplulukTable.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }
    }
}
