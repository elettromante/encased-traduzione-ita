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
                // Definiamo i percorsi dei file
                string gameFileBase = "En.locale";
                string jsonTranslation = "En_Traduzione.json";
                string finalItalianFile = "IT.locale";

                // Controlliamo se l'utente ha passato il parametro di estrazione
                bool forceExtract = args.Contains("--extract_json");

                if (forceExtract)
                {
                    Console.WriteLine("--- MODALITÀ ESTRAZIONE FORZATA ---");
                    
                    // Protezione: anche con il parametro, chiediamo se il file esiste già
                    if (File.Exists(jsonTranslation))
                    {
                        Console.WriteLine($"ATTENZIONE: {jsonTranslation} esiste già!");
                        Console.WriteLine("Per sicurezza, rinominalo o eliminalo manualmente se vuoi davvero ri-estrarlo.");
                        return;
                    }

                    if (!File.Exists(gameFileBase))
                    {
                        Console.WriteLine($"ERRORE: Non trovo {gameFileBase} da cui estrarre.");
                        return;
                    }

                    var locale = FileService.FileToLocale(gameFileBase);
                    var json = JsonConvert.SerializeObject(locale, Formatting.Indented);
                    File.WriteAllText(jsonTranslation, json);
                    
                    Console.WriteLine($"SUCCESSO: Creato {jsonTranslation}. Ora puoi iniziare a tradurre!");
                }
                else
                {
                    // MODALITÀ DEFAULT: Crea IT.locale partendo dal JSON
                    Console.WriteLine("--- MODALITÀ GENERAZIONE IT.LOCALE ---");

                    if (!File.Exists(jsonTranslation))
                    {
                        Console.WriteLine($"ERRORE: Non trovo {jsonTranslation}.");
                        Console.WriteLine("Se è la prima volta, usa: dotnet run --project EncasedBoy -- --extract_json");
                        return;
                    }

                    Console.WriteLine("Lettura traduzioni in corso...");
                    string jsonContent = File.ReadAllText(jsonTranslation);
                    var locale = JsonConvert.DeserializeObject<EncasedLib.Models.Locale>(jsonContent);

                    // Generiamo IT.locale invece di sovrascrivere l'originale
                    FileService.LocaleToFile(locale, finalItalianFile);

                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine($"COMPILAZIONE COMPLETATA: {finalItalianFile} creato!");
                    Console.WriteLine("Puoi usarlo nel gioco rinominandolo o inserendolo nella cartella locale.");
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