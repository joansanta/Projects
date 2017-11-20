using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Int32;

namespace Recover_Emails
{
    class Program
    {
        static void Main(string[] args)
        {
            var contacts = new List<contact>();


            using (SqlConnection conn = new SqlConnection("server=sql-wrt.coches.net;User ID=usr_motor;password=pasivo;database=COC_ESP;Connection Reset=FALSE;Application Name=Coches"))
            {
                conn.Open();


                SqlCommand command = new SqlCommand("SELECT * FROM [COC_ESP_AUX].[dbo].[COC_AD_Mails_Backup_09_14] a with(nolock)  inner join COC_ESP_LCTR..COC_AD lctr on lctr.pk_ad=a.fk_ad where lctr.fk_status=2 and fk_origin in (0) and Date between '2017-09-18 00:00:00' and '2017-09-18 13:45:59' and PublicationId = 52 and (send=0 or send is null) order by date desc", conn);


                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var cont = new contact
                        {
                            pk_mail = Parse(reader.GetValue(reader.GetOrdinal("pk_mail")).ToString()),
                            adid = Parse(reader.GetValue(reader.GetOrdinal("fk_ad")).ToString()),
                            contractid = Parse(reader.GetValue(reader.GetOrdinal("fk_contract")).ToString()),
                            Nombre_Interesado = reader.GetValue(reader.GetOrdinal("name")).ToString(),
                            Email_interesado = reader.GetValue(reader.GetOrdinal("email")).ToString(),
                            Telefono_interesado = reader.GetValue(reader.GetOrdinal("phone")).ToString(),
                            pregunta = reader.GetValue(reader.GetOrdinal("textmail")).ToString()

                        };

                        contacts.Add(cont);
                    }
                }

                var url = "https://www.coches.net/";

                //var url = "http://dev.coches.net/";

                HttpClientHandler httpClientHandler = new HttpClientHandler()
                {
                    Proxy = new WebProxy("proxy.cochesnet.es", 8080),
                    PreAuthenticate = false,
                    UseDefaultCredentials = false,
                };
                
                foreach (var cont in contacts)
                {
                    var json = "AdId=" + cont.adid.ToString() + "&ContractId=" + cont.contractid.ToString() + "&Nombre_Interesado=" + Uri.EscapeDataString(cont.Nombre_Interesado) + "&Email_interesado=" + Uri.EscapeDataString(cont.Email_interesado) + "&Telefono_interesado=" + cont.Telefono_interesado.ToString() + "&DetailHitType=3&OriginId=0&ZoneId=1&Preguntas_interesado=" + Uri.EscapeDataString(cont.pregunta);
                    // var json2 = "AdId="+ Uri.EscapeDataString("33332629")+ "&ContractId=1&Nombre_Interesado=" + Uri.EscapeDataString(cont.Nombre_Interesado) + "&Email_interesado="+ Uri.EscapeDataString("joan.verdaguer@scmspain.com")+"&Telefono_interesado=" + cont.Telefono_interesado.ToString() + "&DetailHitType=3&OriginId=0&ZoneId=1&Preguntas_interesado=" + Uri.EscapeDataString(cont.pregunta);

                    StringContent content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");

                    try
                    {
                        var client = new HttpClient(httpClientHandler);

                        //client.DefaultRequestHeaders.Add("Cookie", "usunico=21/09/2017:12-0017887:816; useragent=0; cfg=1; ASP.NET_SessionId=kimvq1dyuheze5fzr4cwwlxc;");
                        client.DefaultRequestHeaders.Add("Cookie", "usunico=21/09/2017:15-00146088:304; useragent=0; cfg=1; ASP.NET_SessionId=rni0j4mvd3ra53kxof5pvm3r;");
                        client.BaseAddress = new Uri(url);

                        var x = client.PostAsync("/ws/Stats.asmx/MailToAdvertiser2", content).Result;
                        if (x.StatusCode != HttpStatusCode.OK)
                        {
                            Console.WriteLine("Error " + cont.adid.ToString());
                            SqlCommand command2 = new SqlCommand("UPDATE [COC_ESP_AUX].[dbo].[COC_AD_Mails_Backup_09_14] set send = 0 where pk_mail=" + cont.pk_mail, conn);
                            command2.ExecuteNonQuery();
                        }
                        else
                        {
                            Console.Out.Write("OK " + cont.adid.ToString());
                            SqlCommand command2 = new SqlCommand("UPDATE [COC_ESP_AUX].[dbo].[COC_AD_Mails_Backup_09_14] set send = 1 where pk_mail=" + cont.pk_mail, conn);
                            command2.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        SqlCommand command2 = new SqlCommand("UPDATE [COC_ESP_AUX].[dbo].[COC_AD_Mails_Backup_09_14] set send = 0 where pk_mail=" + cont.pk_mail, conn);
                        command2.ExecuteNonQuery();
                        Console.WriteLine(" Exception " + e.Message);
                    }
                }
                conn.Close();
            }
        }

        class contact
        {
            public int adid { get; set; }
            public int contractid { get; set; }
            public string Nombre_Interesado { get; set; }
            public string Email_interesado { get; set; }
            public string Telefono_interesado { get; set; }
            public string pregunta { get; set; }
            public int pk_mail { get; set; }
        }
    }
}
