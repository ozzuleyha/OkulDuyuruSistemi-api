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

        [HttpPut("update-topluluk")]
        public JsonResult UpdateTopluluk(Topluluk topluluk)
        {
            string query = @"
                             UPDATE dbo.Topluluk
                             SET akademisyen_id=@AkademisyenId, yonetici_kullanici_id=@YoneticiId, topluluk_adi=@ToplulukAdi
                             WHERE id=@id
                             ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", topluluk.Id);
                    myCommand.Parameters.AddWithValue("@AkademisyenId", topluluk.AkademisyenId);
                    myCommand.Parameters.AddWithValue("@YoneticiId", topluluk.YoneticiKullaniciId);
                    myCommand.Parameters.AddWithValue("@ToplulukAdi", topluluk.ToplulukAdi);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("delete-topluluk")]
        public JsonResult DeleteTopluluk(Topluluk topluluk)
        {
            string query = @"
                            delete from dbo.[Topluluk]
                            where id = @id
                            ";

            DataTable ToplulukTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", topluluk.Id);

                    myReader = myCommand.ExecuteReader();
                    ToplulukTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [HttpPost("add-topluluk")]
        public JsonResult addTopluluk(Topluluk topluluk)
        {
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;

            string UserQuery = @"
                            insert into dbo.[Topluluk]
                            (yonetici_kullanici_id, akademisyen_id, topluluk_adi)
                            values (@YoneticiId, @AkademisyenId, @ToplulukAdi)
                            ";

            DataTable ToplulukTable = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(UserQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@YoneticiId", topluluk.YoneticiKullaniciId);
                    myCommand.Parameters.AddWithValue("@AkademisyenId", topluluk.AkademisyenId);
                    myCommand.Parameters.AddWithValue("@ToplulukAdi", topluluk.ToplulukAdi);
                    myReader = myCommand.ExecuteReader();
                    ToplulukTable.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }
    }
}
