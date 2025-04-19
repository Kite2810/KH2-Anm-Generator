using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
            Environment.Exit(0); // Kein Doppelklick erlaubt

        string inputFile = args[0];

        if (!File.Exists(inputFile))
        {
            Console.WriteLine("❌ Die angegebene Datei existiert nicht.");
            return;
        }

        string fileName = Path.GetFileNameWithoutExtension(inputFile); // z.B. p_ex100 für Sora, p_ex020 für Donald und p_ex030 für Goofy
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string listPath = Path.Combine(basePath, $"lists/{fileName}.yml");

        if (!File.Exists(listPath))
        {
            Console.WriteLine($"❌ Pfadliste nicht gefunden: {fileName}.yml im selben Ordner wie die EXE.");
            return;
        }

        byte[] sourceData = File.ReadAllBytes(inputFile);
        int copiedCount = 0;

        foreach (var rawLine in File.ReadLines(listPath))
        {
            string relativePath = rawLine.Trim().Trim('"');
            if (string.IsNullOrWhiteSpace(relativePath)) continue;

            string fullTargetPath = Path.Combine(basePath, relativePath);
            string targetDirectory = Path.GetDirectoryName(fullTargetPath)!;

            try
            {
                Directory.CreateDirectory(targetDirectory);
                File.WriteAllBytes(fullTargetPath, sourceData);
                Console.WriteLine($"✔ Kopiert nach: {fullTargetPath}");
                copiedCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fehler bei {relativePath}: {ex.Message}");
            }
        }

        Console.WriteLine($"\n✅ Fertig! {copiedCount} Dateien erstellt.");
    }
}
