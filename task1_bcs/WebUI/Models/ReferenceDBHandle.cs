using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace WebUI.Models
{
    public class ReferenceDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["ReferenceDB"].ToString();
            con = new SqlConnection(constring);
        }

        public bool AddPerson(PersonModel person)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddPerson", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@name", person.FullName);
            cmd.Parameters.AddWithValue("@phone", person.Phone);
            cmd.Parameters.AddWithValue("@email", person.Email);
            cmd.Parameters.AddWithValue("@cityid", ((object)person.CityId) ?? DBNull.Value);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
        
        public List<PersonModel> GetPersons()
        {
            connection();
            List<PersonModel> personList = new List<PersonModel>();

            SqlCommand cmd = new SqlCommand("GetPerson", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
                        
            con.Open();
            sd.Fill(dt);
            con.Close();                      

            foreach(DataRow dr in dt.Rows)
            {
                personList.Add(
                    new PersonModel
                    {
                        PersonId = Convert.ToInt32(dr["PersonId"]),
                        FullName = Convert.ToString(dr["FullName"]),
                        Phone = Convert.ToString(dr["Phone"]),
                        Email = Convert.ToString(dr["Email"]),
                        CityId = Convert.ToInt32(dr["CityId"] is DBNull ? 0 : dr["CityId"]),
                    });
            }
            return personList;
        }

        public List<CityModel> GetCities()
        {
            connection();
            List<CityModel> cityList = new List<CityModel>();

            SqlCommand cmd = new SqlCommand("GetCity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();
            
            foreach (DataRow dr in dt.Rows)
            {
                cityList.Add(
                    new CityModel
                    {
                        CityId = Convert.ToInt32(dr["CityId"]),
                        Name = Convert.ToString(dr["Name"]),
                        Country = Convert.ToString(dr["Country"]),
                    });
            }
            return cityList;
        }

        public List<PersonViewModel> GetPersonsWithCityNames()
        {
            List<PersonModel> persons = GetPersons();
            List<CityModel> cities = GetCities();

            var result = from person in persons
                         join city in cities 
                            on person.CityId equals city.CityId into temp
                         from t in temp.DefaultIfEmpty()
                         //where t.CityId == 0
                         select new PersonViewModel()
                         {
                             PersonId = person.PersonId,
                             FullName = person.FullName,
                             Phone = person.Phone,
                             Email = person.Email,
                             CityName = t?.Name
                         };

            return result.ToList();
        }
    }
}