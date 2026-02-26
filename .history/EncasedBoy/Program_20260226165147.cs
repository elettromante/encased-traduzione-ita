using System;
using System.IO;
using System.Linq;
using EncasedLib.Services;
using Newtonsoft.Json;

namespace EncasedBoy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try 
            {
                // 1. Identificazione Lingua (Default: En)
                string rawInput = args.FirstOrDefault(a => !a.StartsWith("-")) ?? "En";
                string langCode = char.ToUpper(rawInput[0]) + rawInput.Substring(1).ToLower();

                // 2. Configurazione Cartelle
                string inputFolder = "Locale";           // File originali
                string outputJsonFolder = "Extracted_Json"; // I tuoi JSON
                string packedFolder = "Packed";          // File generati per il gioco
                
                // Creazione cartelle se mancano
                if (!Directory.Exists(inputFolder)) Directory.CreateDirectory(inputFolder);
                if (!Directory.Exists(outputJsonFolder)) Directory.CreateDirectory(outputJsonFolder);
                if (!Directory.Exists(packedFolder)) Directory.CreateDirectory(packedFolder);

                // 3. Percorsi File
                string originalLocale = Path.Combine(inputFolder, $"{langCode}.locale");
                string jsonFile = Path.Combine(outputJsonFolder, $"{langCode}_Traduzione.json");
                string packedLocale = Path.Combine(packedFolder, $"{langCode}.locale");

                bool isExtracting = args.Contains("--extract_json");
                bool isPacking = args.Contains("--pack");

                if (isExtracting)
                {
                    Console.WriteLine($"--- ESTRAZIONE [{langCode}] ---");
                    
                    if (File.Exists(jsonFile))
                    {
                        Console.WriteLine($"STOP: {jsonFile} esiste già. Spostalo o nominalo diversamente per ri-estrarre.");
                        return;
                    }

                    if (!File.Exists(originalLocale))
                    {
                        Console.WriteLine($"ERRORE: Non trovo il file originale {originalLocale}");
                        return;
                    }

                    var locale = FileService.FileToLocale(originalLocale);
                    var json = JsonConvert.SerializeObject(locale, Formatting.Indented);
                    File.WriteAllText(jsonFile, json);
                    
                    Console.WriteLine($"SUCCESSO: Creato {jsonFile}");
                }
                else if (isPacking)
                {
                    Console.WriteLine($"--- PACKING [{langCode}] ---");

                    if (!File.Exists(jsonFile))
                    {
                        Console.WriteLine($"ERRORE: Non trovo {jsonFile} da compilare.");
                        return;
                    }

                    Console.WriteLine($"Leggendo {jsonFile}...");
                    string jsonContent = File.ReadAllText(jsonFile);
                    var locale = JsonConvert.DeserializeObject<EncasedLib.Models.Locale>(jsonContent);

                    // Scrive il file finale nella cartella Packed/
                    FileService.LocaleToFile(locale, packedLocale);

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine($"SUCCESSO: {packedLocale} creato correttamente!");
                    Console.WriteLine("--------------------------------------------------");
                }
                else
                {
                    Console.WriteLine("Comandi disponibili:");
                    Console.WriteLine($" - dotnet run -- {langCode} --extract_json  (Estrae da Locale/ a Extracted_Json/)");
                    Console.WriteLine($" - dotnet run -- {langCode} --pack          (Compila da Extracted_Json/ a Packed/)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRORE: " + ex.Message);
            }
        }
    }
}