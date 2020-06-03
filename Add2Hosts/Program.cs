using System;
using System.IO;


namespace Add2Hosts
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Программа прописывает в файл HOSTS параметры сервера BIFIT.");

            string filePath = Environment.GetEnvironmentVariable("WINDIR") + "\\System32\\drivers\\etc\\hosts";
            
            string[] text = { };

            string line;
            using (StreamReader sr = new StreamReader(filePath))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    Array.Resize(ref text, text.Length + 1);
                    text[text.Length - 1] = line;
                }
            }

            bool allOk = true;
            for (int i = 0; i < text.Length; i++)
            {
                line = text[i];
                if (line.Length > 0)
                {
                    line.Trim();
                    if (line[0] == '#')
                        continue;
                    if ((line.Substring(0, 9) == "127.0.0.1") || (line.Substring(0, 10) == "\t127.0.0.1"))
                    {
                        allOk = false;
                        break;
                    }
                }
            }

            if (allOk)
            {
                using (StreamWriter sw = new StreamWriter(filePath + "1", false, System.Text.Encoding.Default))
                {
                    allOk = false;
                    for (int i = 0; i < text.Length; i++)
                    {
                        line = text[i];
                        line.Trim();
                        if (line.Length > 0)
                            if (line[0] != '#')
                            {
                                sw.WriteLine("\t127.0.0.1\tbifit.signer.com");
                                allOk = true;
                            }
                                
                        sw.WriteLine(text[i]);
                    }
                    if (!allOk)
                    {
                        sw.WriteLine("\t127.0.0.1\tbifit.signer.com");
                    }
                }
            }
            else
                Console.WriteLine("Обновление не удалось! Адрес 127.0.0.1 уже используется");
        }
    }
}
