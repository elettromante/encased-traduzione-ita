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
                
                // Formatta l'input come "Ab" (es: fr -> Fr, EN -> En)
                string langCode = char.ToUpper(rawInput[0]) + rawInput.Substring(1).ToLower();

                // 2. Configurazione Percorsi
                string localeFolder = "Locale";
                string inputFile = Path.Combine(localeFolder, $"{langCode}.locale");
                
                // Qui avviene la magia: il JSON avrà il nome della lingua passata
                string jsonFile = $"{langCode}_Traduzione.json"; 
                
                string outputFile = Path.Combine(localeFolder, $"{langCode}.locale");

                bool forceExtract = args.Contains("--extract_json");

                if (!Directory.Exists(localeFolder))
                {
                    Directory.CreateDirectory(localeFolder);
                }

                if (forceExtract)
                {
                    Console.WriteLine($"--- MODALITÀ ESTRAZIONE FORZATA [{langCode}] ---");
                    
                    if (File.Exists(jsonFile))
                    {
                        Console.WriteLine($"ATTENZIONE: {jsonFile} esiste già! Rinominalo per non sovrascriverlo.");
                        return;
                    }

                    if (!File.Exists(inputFile))
                    {
                        Console.WriteLine($"ERRORE: Non trovo {inputFile} nella cartella {localeFolder}.");
                        return;
                    }

                    var locale = FileService.FileToLocale(inputFile);
                    var json = JsonConvert.SerializeObject(locale, Formatting.Indented);
                    File.WriteAllText(jsonFile, json);
                    
                    Console.WriteLine($"SUCCESSO: Creato {jsonFile} (estratto da {langCode}.locale)");
                }
                else
                {
                    // MODALITÀ DEFAULT: Compilazione
                    Console.WriteLine($"--- MODALITÀ GENERAZIONE {langCode}.LOCALE ---");

                    if (!File.Exists(jsonFile))
                    {
                        Console.WriteLine($"ERRORE: Non trovo il file {jsonFile} per la compilazione.");
                        return;
                    }

                    string jsonContent = File.ReadAllText(jsonFile);
                    var locale = JsonConvert.DeserializeObject<EncasedLib.Models.Locale>(jsonContent);

                    FileService.LocaleToFile(locale, outputFile);

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine($"COMPILAZIONE COMPLETATA: {outputFile} aggiornato!");
                    Console.WriteLine("--------------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRORE: " + ex.Message);
            }
        }
    }
}