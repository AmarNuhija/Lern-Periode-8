using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class Pferderennen
{
    private List<Pferd> pferde;
    private Pferd gewaehltesPferd;
    private decimal einsatz;
    private const int Ziellinie = 48; // Länge der Rennstrecke (entspricht Anzahl der Zeichen)
    private Dictionary<Pferd, int> positionen;
    private List<Pferd> ergebnisListe;

    public Pferd Sieger { get; private set; }
    public bool GewaehltGewonnen => Sieger == gewaehltesPferd;
    public decimal GewinnBetrag => einsatz * Sieger.Quote;

    public Pferderennen(List<Pferd> pferde)
    {
        this.pferde = pferde;
        positionen = new Dictionary<Pferd, int>();
        ergebnisListe = new List<Pferd>();

        foreach (var pferd in pferde)
        {
            positionen[pferd] = 0; // Startposition ist 0
        }
    }

    // Methode, um eine Wette zu platzieren
    public void WettePlatzieren(int pferdIndex, decimal einsatz)
    {
        if (pferdIndex < 0 || pferdIndex >= pferde.Count)
        {
            Console.WriteLine("Ungültige Pferdwahl. Bitte wählen Sie ein gültiges Pferd.");
            return;
        }

        this.gewaehltesPferd = pferde[pferdIndex];
        this.einsatz = einsatz;
        Console.WriteLine($"Sie haben {einsatz:C} auf {gewaehltesPferd.Name} gesetzt.");
    }

    // Methode, um das Rennen zu simulieren und den "Live-Ticker" zu zeigen
    public void RennenStarten()
    {
        Console.WriteLine("Das Rennen beginnt!\n");

        bool rennenLaufend = true;
        Random random = new Random();

        while (rennenLaufend)
        {
            Console.Clear();
            Console.WriteLine("Aktueller Rennstand:");

            foreach (var pferd in pferde)
            {
                if (ergebnisListe.Contains(pferd)) continue; // Überspringen, falls das Pferd bereits das Ziel erreicht hat

                // Zufällige Distanz hinzufügen, um das Rennen voranzubringen
                int distanz = random.Next(1, 4); // Kleinere Schritte für langsameren Fortschritt
                positionen[pferd] += distanz;

                // Position des Pferdes auf der Strecke berechnen
                int pferdPosition = Math.Min(positionen[pferd], Ziellinie);
                string strecke = "|" + new string(' ', pferdPosition) + "-->" + new string(' ', Ziellinie - pferdPosition) + "|";

                // Die aktuelle Strecke des Pferdes anzeigen
                Console.WriteLine($"{pferd.Name.PadRight(10)} {strecke}");

                // Prüfen, ob ein Pferd die Ziellinie erreicht hat
                if (positionen[pferd] >= Ziellinie && !ergebnisListe.Contains(pferd))
                {
                    ergebnisListe.Add(pferd);
                    if (Sieger == null) Sieger = pferd; // Setze den ersten Zielläufer als Sieger
                }
            }

            // Sleep damit das Spiel nicht so schnell vorbei geht
            Thread.Sleep(500);
            // Wenn alle Pferde das Ziel erreicht haben wird das Spiel beendet
            if (ergebnisListe.Count == pferde.Count)
            {
                rennenLaufend = false;
            }
        }

        // Gesamtergebnis anzeigen
        Console.Clear();
        Console.WriteLine("Das Rennen ist beendet! Die Platzierungen sind:\n");

        for (int i = 0; i < ergebnisListe.Count; i++)
        {
            var pferd = ergebnisListe[i];
            string platzierung = (i == 0) ? "Gewinner" : $"{i + 1}. Platz";
            Console.WriteLine($"{platzierung}: {pferd.Name} (Quote: {pferd.Quote})");
        }

        // Ergebnis der Wette ausgeben
        if (GewaehltGewonnen)
        {
            Console.WriteLine($"\nHerzlichen Glückwunsch! Sie haben gewonnen: {GewinnBetrag:C}");
        }
        else
        {
            Console.WriteLine("\nLeider haben Sie verloren.");
        }
    }

    // Methode, um das Rennen zurückzusetzen
    public void ResetRennen()
    {
        // Positionen der Pferde zurücksetzen
        foreach (var pferd in pferde)
        {
            positionen[pferd] = 0;
        }

        // Ergebnisliste und Sieger zurücksetzen
        ergebnisListe.Clear();
        Sieger = null;
    }

}