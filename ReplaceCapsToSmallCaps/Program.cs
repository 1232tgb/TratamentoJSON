using System.IO;

namespace ReplaceCapsToSmallCaps
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Passa o caminho do arquivo que quer diminuir as strings:");
                string path = ReadLine();


                //Console.WriteLine("passa o trecho que vc quer substituir:");
                //String trecho = ReadLine();
                String trecho = "\"/api/";

                var logFile = File.ReadAllLines(path);
                var logList = new List<string>(logFile);

                for (int i = 0; i < logList.Count; i++)
                {
                                        
                    ///////
                    ///Esse trecho coloca as linhas das rotas dos endpoints em letras minusculas
                    //////
                    var line = logList[i];
                    var line2 = line.Trim(' ').Replace(@"\\", "");
                    //string line2 = line.Replace("\t", "").Replace("\"", "").Trim(' ');
                    if (line2.StartsWith(trecho))
                    {
                        var lineLowerCaps = line.ToLower();
                        logList[i] = lineLowerCaps.ToString();
                    }


                    ///////
                    ///Esse trecho concatena as propriedades de Description, Summary e OperationID nos endpoints
                    //////
                    string concatena = "";
                    string[] actionsArray = new string[4] { "\"get\": {", "\"post\": {", "\"put\": {", "\"delete\": {" };
                    bool equal = actionsArray.Any(a => line2.Equals(a));
                    if (equal)
                    {
                        var response = "\"responses\": {";
                        string trechoDescricao = "\"description\":\"\" ,";
                        string trechoResumo = "\"summary\":\"\" ,";
                        string trechoOperacaoID = "\"operationId\":\"\" ,";
                        string[] propriedades = new string[3] { trechoDescricao, trechoResumo, trechoOperacaoID };

                        for (int j = 0; j < propriedades.Length; j++)
                        {
                            bool deveConcatenar = false;
                            var propriedade = propriedades[j];//.Replace("\\", "").Replace("\"", "").Trim(' ');
                                                              //var propriedade = propriedades[j].Trim(' ');
                            int newIndex = i;
                            while (true)
                            {
                                var proximoIndice = newIndex;
                                if (proximoIndice >= logList.Count)
                                    break;

                                var proximaLinha = logList[proximoIndice];//.Replace("\\", "").Replace("\"", "");

                                //se achou a propriedade, quebra o loop
                                var propriedadeTratada = propriedade.Replace("\\", "").Replace("\"", "").Trim(' ');
                                var proximaLinhaTratada = proximaLinha.Replace("\\", "").Replace("\"", "").Trim(' ');
                                if (proximaLinhaTratada.Equals(propriedadeTratada))
                                    break;

                                //se não achou a propriedade, chegou até a proxima endpoint
                                if (proximaLinha.StartsWith(trecho) || proximaLinha.Trim(' ').Contains(response))
                                {
                                    deveConcatenar = true;
                                    concatena += propriedade;
                                    break;
                                }

                                newIndex++;
                            }
                            if (deveConcatenar)
                            {
                                logList[i] += concatena;
                                concatena = "";
                            }
                        }

                        //logList[i] += concatena;
                    }


                }

                File.WriteAllLines(path, logList);

                //WriteLine(path);
            }
            catch (Exception e)
            {
                WriteLine(e.Message.ToString());
            }
            
        }
    }
}