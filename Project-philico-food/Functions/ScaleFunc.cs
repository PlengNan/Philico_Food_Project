using Project_philico_food.functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.UI;

namespace Project_philico_food.Functions
{
    internal class ScaleFunc
    {

        public string Err { get; set; }

        public bool Connect(SerialPort port)
        {
            try
            {
                AESEncryption aESEncryption = new AESEncryption();
                port.PortName = aESEncryption.Decrypt(ConfigurationManager.AppSettings["SCALE_PORT"]);
                port.BaudRate = int.Parse(aESEncryption.Decrypt(ConfigurationManager.AppSettings["SCALE_BAUDRATE"]));
                port.Open();
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return false;
            }
            return true;
        }


        public bool Disconnect(SerialPort port)
        {
            try
            {
                if (port.IsOpen)
                {
                    port.Close();
                }
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                return false;
            }
            return true;
        }


        public string DataReceive(string rawLine)
        {
            //string weight = "";
            try
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                    return "Error";

                string s = rawLine.Trim();

                s = s.Replace("ST", "")
                     .Replace("GS", "")
                     .Replace("NT", "")
                     .Replace("US", "")
                     .Trim();

                var match = Regex.Match(s, @"-?\d+(\.\d+)?");
                if(!match.Success)
                    return "Error";

                if(double.TryParse(match.Value, out double d))
                {
                    int value = (int)Math.Round(d, MidpointRounding.AwayFromZero);
                    return value.ToString();
                }
                return "Error";
            }
            catch 
            {
                return "ERROR";
            }
            
        }














    }
}
