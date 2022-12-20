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
    public class DuyuruController: Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public DuyuruController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet("duyuru-list")]
        public JsonResult getDuyuruList()
        {
            string query = @"
                            SELECT * FROM
                            dbo.Duyuru
                            ";

            DataTable DuyuruTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    DuyuruTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(DuyuruTable);
        }

        [HttpDelete("delete-duyuru")]
        public JsonResult DeleteDuyuru(Duyuru duyuru)
        {
            string query = @"
                            delete from dbo.[Duyuru]
                            where id = @id
                            ";

            DataTable DuyuruTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", duyuru.Id);

                    myReader = myCommand.ExecuteReader();
                    DuyuruTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
        [HttpPut("update-duyuru")]
        public JsonResult UpdateDuyuru (Duyuru duyuru)
        {
            string query = @"
                             UPDATE dbo.Duyuru
                             SET duyuru_basligi=@DuyuruBasligi, duyuru_aciklama=@DuyuruAciklama, topluluk_id=@ToplulukId
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
                    myCommand.Parameters.AddWithValue("@id", duyuru.Id);
                    myCommand.Parameters.AddWithValue("@DuyuruBasligi", duyuru.DuyuruBasligi);
                    myCommand.Parameters.AddWithValue("@DuyuruAciklama", duyuru.DuyuruAciklama);
                    myCommand.Parameters.AddWithValue("@ToplulukId", duyuru.ToplulukId);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpPost("add-duyuru")]
        public JsonResult AddUser(Duyuru duyuru)
        {
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;

            string UserQuery = @"
                            insert into dbo.[Duyuru]
                            (topluluk_id, duyuru_basligi, duyuru_aciklama)
                            values (@ToplulukId, @DuyuruBasligi, @DuyuruAciklama)
                            ";

            DataTable DuyuruTable = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(UserQuery, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ToplulukId", duyuru.ToplulukId);
                    myCommand.Parameters.AddWithValue("@DuyuruBasligi", duyuru.DuyuruBasligi);
                    myCommand.Parameters.AddWithValue("@DuyuruAciklama", duyuru.DuyuruAciklama);
                    myReader = myCommand.ExecuteReader();
                    DuyuruTable.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }
    }
}
