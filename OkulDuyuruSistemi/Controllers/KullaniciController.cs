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

        [HttpGet("user-list")]
        public JsonResult getDuyuruList()
        {
            string query = @"
                            SELECT * FROM
                            dbo.Kullanici
                            ";

            DataTable KullaniciTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    KullaniciTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(KullaniciTable);
        }
        [HttpDelete("delete-kullanici")]
        public JsonResult DeleteKullanici(Kullanici kullanici)
        {
            string query = @"
                            delete from dbo.[Kullanici]
                            where id = @id
                            ";

            DataTable KullaniciTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", kullanici.Id);

                    myReader = myCommand.ExecuteReader();
                    KullaniciTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [HttpGet("akademisyen-list")]
        public JsonResult getAkademisyenList()
        {
            string query = @"
                            SELECT * FROM
                            dbo.Kullanici
                            WHERE (role_id = 2)
                            ";

            DataTable KullaniciTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    KullaniciTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(KullaniciTable);
        }

        [HttpPut("update-yonetici")]
        public JsonResult UpdateYonetici (Kullanici yonetici)
        {
            string query = @"
                             UPDATE dbo.Kullanici
                             SET role_id=@RoleId
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
                    myCommand.Parameters.AddWithValue("@id", yonetici.Id);
                    myCommand.Parameters.AddWithValue("@RoleId", yonetici.RoleId);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpGet("yonetici-list")]
        public JsonResult getYoneticiList()
        {
            string query = @"
                            SELECT * FROM
                            dbo.Kullanici
                            WHERE (role_id = 3)
                            ";

            DataTable KullaniciTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DuyuruAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    KullaniciTable.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(KullaniciTable);
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



        [HttpPost("login")]
        public JsonResult LoginControl(Kullanici kullanici)
        {
            string query = @"SELECT * FROM dbo.[Kullanici] WHERE mail='" + kullanici.Mail + "' AND parola='" + kullanici.Parola + "'";

            DataTable table = new DataTable();
            //int role = user.UserRoleId;
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
            if (table.Rows.Count == 0)
            {
                return new JsonResult("Yanlış bilgi girildi!");
            }
            else
            {
                return new JsonResult(table);
                
                    
                    
            }
        }
    }
}
