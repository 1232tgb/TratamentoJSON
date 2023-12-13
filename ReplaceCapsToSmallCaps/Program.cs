using System.IO;
using System.Runtime.Serialization.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace ReplaceCapsToSmallCaps
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Passsa o caminho do arquivo que quer alterar:");

                string path = Console.ReadLine();
                //string path = @"D:\Projetos\VSCode\SwaggerFiles\JSON\file2.json";

                string trecho = "\"/api/";


                var logFile = File.ReadAllLines(path);
                var logList = new List<string>(logFile);

                for (int i = 0; i < logList.Count; i++)
                {
                    string line = logList[i];
                    //string line2 = line.Replace("\t", "").Replace("\"", "").Trim(' ');
                    string line2 = line.Trim(' ').Replace(@"\\", "");

                    if (line2.StartsWith(trecho))
                    {
                        logList[i] = line.ToLower();

                    }
                    string[] actionsArray = new string[4] { "\"get\": {", "\"post\": {", "\"put\": {", "\"delete\": {" };
                    bool contain = actionsArray.Any(a => line2.Equals(a));
                    if (contain)
                    {
                        string trechoAInserir = "\"description\": \"Description\",\r\n\"summary\": \"Summary\",\r\n\"operationId\": \"OperationID\",";
                        logList[i] += trechoAInserir;
                    }

                    Console.WriteLine(logList[i]);


                }
                File.WriteAllLines(path, logList);






            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }


        }
    }
}