using System;
using System.Collections.Generic;

public class PferderennenGUI
{
    private List<Pferd> pferdeListe;
    private Pferderennen rennen;
    private decimal konto = 1000m; // Startguthaben des Spielers
    private Random random = new Random();

    public PferderennenGUI()
    {
        // Setze die Farbe für die Pferde
        Console.ForegroundColor = ConsoleColor.Magenta;

        // Zeichnung eines Pferdes
        Console.WriteLine("           .'\"                     .'\"");
        Console.WriteLine("  ._.-.___.' (`\\          ._.-.___.' (`\\");
        Console.WriteLine(" //(        ( `'         //(        ( `' ");
        Console.WriteLine("'/ )\\ ).__. )           '/ )\\ ).__. )   ");
        Console.WriteLine("' <' `\\ ._/'\\           ' <' `\\ ._/'\\   ");
        Console.WriteLine("   `   \\     \\             `   \\     \\  ");

        // Zurück zur Standardfarbe
        Console.ResetColor();

        // Initialisiere Pferdeliste mit zufälligen Quoten
        pferdeListe = new List<Pferd>
        {
            new Pferd("Meteor", GeneriereZufallsQuote()),
            new Pferd("Cassini I", GeneriereZufallsQuote()),
            new Pferd("Bella Rose", GeneriereZufallsQuote()),
            new Pferd("Storm", GeneriereZufallsQuote()),
            new Pferd("Goku", GeneriereZufallsQuote()),
            new Pferd("Eren", GeneriereZufallsQuote())
        };

        // Sortiere Pferde basierend auf der Quote (niedrigere Quoten zuerst)
        pferdeListe.Sort((p1, p2) => p1.Quote.CompareTo(p2.Quote));

        rennen = new Pferderennen(pferdeListe);
    }

    private decimal GeneriereZufallsQuote()
    {
        // Generiere eine Quote zwischen 1.3 und 4.6
        return Math.Round((decimal)(random.NextDouble() * (4.6 - 1.3) + 1.3), 1);
    }

    public void StarteRennenMitWette()
    {
        Console.WriteLine("_____________________________________________");
        Console.WriteLine("Willkommen beim Pferderennen!");
        Console.WriteLine("_____________________________________________");
        bool weiterspielen = true;

        while (weiterspielen)
        {
            Console.WriteLine("\nIhr aktuelles Guthaben: {0:C}", konto);

            // Rennen zurücksetzen, bevor ein neues beginnt
            rennen.ResetRennen();

            // Pferdeliste anzeigen
            Console.WriteLine("\nAuf welches Pferd möchten Sie wetten? Bitte wählen Sie die Nummer des Pferdes:");
            for (int i = 0; i < pferdeListe.Count; i++)
            {
                // Farbe der Pferdenamen setzen
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{i}: {pferdeListe[i].Name} (Quote: {pferdeListe[i].Quote})");
                Console.ResetColor();
            }

            // Auswahl des Pferdes
            int pferdIndex;
            while (!int.TryParse(Console.ReadLine(), out pferdIndex) || pferdIndex < 0 || pferdIndex >= pferdeListe.Count)
            {
                Console.WriteLine("Ungültige Auswahl. Bitte geben Sie eine gültige Pferdenummer ein:");
            }

            // Einsatzbetrag eingeben
            Console.WriteLine("Wie viel möchten Sie setzen?");
            decimal einsatz;
            while (!decimal.TryParse(Console.ReadLine(), out einsatz) || einsatz <= 0 || einsatz > konto)
            {
                Console.WriteLine("Ungültiger Betrag. Bitte geben Sie einen gültigen Einsatz ein (maximal {0:C}):", konto);
            }

            // Wette platzieren
            rennen.WettePlatzieren(pferdIndex, einsatz);

            // Rennen starten und Ergebnis anzeigen
            rennen.RennenStarten();

            // Konto aktualisieren basierend auf dem Rennergebnis
            if (rennen.GewaehltGewonnen)
            {
                konto += rennen.GewinnBetrag - einsatz;
            }
            else
            {
                konto -= einsatz;
            }

            Console.WriteLine("\nIhr neues Guthaben: {0:C}", konto);

            // Spiel beenden, falls Guthaben aufgebraucht
            if (konto <= 0)
            {
                Console.WriteLine("Ihr Guthaben ist aufgebraucht. Vielen Dank fürs Spielen!");
                break;
            }

            // Fragen, ob der Spieler weiterspielen möchte
            weiterspielen = WillWeiterSpielen();
        }

        Console.WriteLine("\nVielen Dank fürs Spielen! Bis zum nächsten Mal.");
    }

    private bool WillWeiterSpielen()
    {
        while (true)
        {
            Console.WriteLine("Möchten Sie weiterspielen? (ja/nein)");
            string antwort = Console.ReadLine()?.Trim().ToLower();

            if (antwort == "ja")
            {
                return true;
            }
            else if (antwort == "nein")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe. Bitte antworten Sie mit 'ja' oder 'nein'.");
            }
        }
    }
}