﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PineLinks.Models;
using System.Diagnostics;
using System.Data.SqlClient;

namespace PineLinks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static IList<UserModel> Users = new List<UserModel>();

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        private IConfiguration Configuration;
        public HomeController(IConfiguration _configuration, ILogger<HomeController> logger)
        {
            Configuration = _configuration;
            _logger = logger;
        }

        void connectionString()
        {
            con.ConnectionString = this.Configuration.GetConnectionString("ConString");
        }

        public IActionResult Index()
        {
            Users.Clear();
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Users";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Users.Add(new UserModel
                {
                    Name = dr["UserName"].ToString()
                });

                while (dr.Read())
                {
                    Users.Add(new UserModel
                    {
                        Name = dr["UserName"].ToString()
                    });
                }

                return View(Users);
            }
            else
            {
                return View(Users);
            }
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}