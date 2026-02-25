using System;
using System.IO;
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
                string inputFile = "En.locale";
                string outputFile = "En_Traduzione.json";

                Console.WriteLine("Avvio estrazione da: " + inputFile);

                if (!File.Exists(inputFile)) {
                    Console.WriteLine("ERRORE: Non trovo il file En.locale nella cartella principale!");
                    return;
                }

                // Carica il file binario e lo trasforma in un oggetto locale
                var locale = FileService.FileToLocale(inputFile);
                
                // Converte tutto in un formato JSON leggibile
                var json = JsonConvert.SerializeObject(locale, Formatting.Indented);
                
                // Salva il file solo se non esiste già
                if (!File.Exists(outputFile))
                {
                    File.WriteAllText(outputFile, json);
                    Console.WriteLine($"[INFO] File creato: {outputFile}");
                    Console.WriteLine("SUCCESSO! Creato file: " + outputFile);
                    Console.WriteLine("Ora puoi scaricare En_Traduzione.json dalla colonna di sinistra.");
                }
                else
                {
                    Console.WriteLine($"[SICUREZZA] Il file {outputFile} esiste già. Sovrascrittura annullata per proteggere il tuo lavoro!");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRORE DURANTE L'ESTRAZIONE: " + ex.Message);
            }
        }
    }
}