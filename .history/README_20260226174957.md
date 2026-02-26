# <img src="https://raw.githubusercontent.com/lipis/flag-icons/main/flags/4x3/it.svg" width="30"> Encased - Traduzione Italiana

Progetto per la traduzione completa e la revisione della localizzazione italiana di **Encased** tramite il tool **EncasedBoy**.

## Crediti e Ringraziamenti
Questo progetto è basato sui tool di estrazione e compilazione sviluppati originariamente da **[DocteurKain](https://github.com/DocteurKain/Encased)**. Un ringraziamento speciale per aver reso possibile l'accesso ai file di gioco.

## Installazione
1. Scaricare l'ultima versione del file `It.locale`.
2. Copiare il file nella cartella: 
   `{cartella_del_gioco}\Encased_Data\StreamingAssets\Localization`
3. **Nota Importante:** Se il gioco non mostra la lingua italiana, rinominare il file in `En.locale` o `Es.locale` (previo backup dell'originale) e selezionare la rispettiva lingua nelle impostazioni di gioco.

## Istruzioni per lo Sviluppo (EncasedBoy)
Il tool permette di gestire la traduzione in modo sicuro, applicando delle "patch" testuali ai file di localizzazione originali.

### Struttura Cartelle
* `Locale/`: Inserire qui i file `.locale` originali estratti dal gioco.
* `Extracted_Json/`: Destinazione dei file JSON generati.
* `Packed/`: Destinazione dei file binari pronti per l'uso.

### Comandi Disponibili

#### 1. Estrazione (`--extract_json`)
Converte il file binario originale in un JSON modificabile.
* **Comando:** `dotnet run -- {Lingua} --extract_json`
* **Esempio:** `dotnet run -- En --extract_json`
  *(Estrae Locale/En.locale in Extracted_Json/En_Traduzione.json)*

#### 2. Aggiornamento Patch (`--update`)
Inietta le traduzioni scritte nel file `patch.txt` dentro il JSON della lingua specificata.
* **Comando:** `dotnet run -- {Lingua} --update`
* **Esempio:** `dotnet run -- En --update`
* **Formato patch.txt:** `ID|Testo` (Esempio: `CD887E|Nuova Partita`)

#### 3. Compilazione (`--pack`)
Genera il file `.locale` finale dal JSON aggiornato.
* **Comando:** `dotnet run -- {Lingua} --pack`
* **Esempio:** `dotnet run -- En --pack`
  *(Genera Packed/En.locale)*

## Note Tecniche
* I sorgenti della libreria (`EncasedLib`) sono inclusi per permettere l'esportazione e l'importazione dei file tramite la classe `LocaleService`.
* Le modifiche tecniche dettagliate sono consultabili nel file **[CHANGELOG.md](./CHANGELOG.md)**.