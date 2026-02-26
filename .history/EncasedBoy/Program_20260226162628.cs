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
                // 1. Identificazione Lingua (Default: EN)
                // Prende il primo argomento che non inizia con '-' (es: EN, FR, ES, IT)
                string langCode = args.FirstOrDefault(a => !a.StartsWith("-"))?.ToUpper() ?? "EN";

                // 2. Configurazione Percorsi
                string localeFolder = "Locale";
                string inputFile = Path.Combine(localeFolder, $"{langCode}.locale");
                string jsonFile = $"{langCode}_Traduzione.json";
                string outputFile = Path.Combine(localeFolder, $"{langCode}.locale");

                // Controlliamo se l'utente ha passato il parametro di estrazione
                bool forceExtract = args.Contains("--extract_json");

                // Assicuriamoci che la cartella Locale esista
                if (!Directory.Exists(localeFolder))
                {
                    Directory.CreateDirectory(localeFolder);
                }

                if (forceExtract)
                {
                    Console.WriteLine($"--- MODALITÀ ESTRAZIONE FORZATA [{langCode}] ---");
                    
                    if (File.Exists(jsonFile))
                    {
                        Console.WriteLine($"ATTENZIONE: {jsonFile} esiste già!");
                        Console.WriteLine("Per sicurezza, rinominalo o eliminalo manualmente se vuoi davvero ri-estrarlo.");
                        return;
                    }

                    if (!File.Exists(inputFile))
                    {
                        Console.WriteLine($"ERRORE: Non trovo il file sorgente -> {inputFile}");
                        return;
                    }

                    var locale = FileService.FileToLocale(inputFile);
                    var json = JsonConvert.SerializeObject(locale, Formatting.Indented);
                    File.WriteAllText(jsonFile, json);
                    
                    Console.WriteLine($"SUCCESSO: Creato {jsonFile} partendo da {inputFile}.");
                }
                else
                {
                    // MODALITÀ DEFAULT: Crea .locale partendo dal JSON
                    Console.WriteLine($"--- MODALITÀ GENERAZIONE {langCode}.LOCALE ---");

                    if (!File.Exists(jsonFile))
                    {
                        Console.WriteLine($"ERRORE: Non trovo il file di traduzione -> {jsonFile}");
                        Console.WriteLine($"Uso: dotnet run --project EncasedBoy -- {langCode} --extract_json");
                        return;
                    }

                    Console.WriteLine($"Lettura di {jsonFile} in corso...");
                    string jsonContent = File.ReadAllText(jsonFile);
                    var locale = JsonConvert.DeserializeObject<EncasedLib.Models.Locale>(jsonContent);

                    // Scrive il file binario nella cartella Locale/
                    FileService.LocaleToFile(locale, outputFile);

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine($"COMPILAZIONE COMPLETATA: {outputFile} creato!");
                    Console.WriteLine("--------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRORE CRITICO: " + ex.Message);
            }
        }
    }
}