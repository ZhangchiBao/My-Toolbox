using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.DAL
{
    public class DBContext : DbContext
    {
        public DBContext() : base(new SqlConnection()
        {
            ConnectionString = new SqlConnectionStringBuilder()
            {
                ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"{System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bdp.mdf")}\";Integrated Security=True;Connect Timeout=30"
            }.ConnectionString
        }, true)
        {
        }

        public DbSet<Finder> Finders { get; set; }
    }

    public class Finder
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public Guid Key { get; set; }
    }
}
