# P06_KAW_DataAccess
KAW Übung Nummer 6, Rainbowtable MD5 Hash Finder

## Projektbeschreibung des Rainbowtable Hash Finder
Im Zuge der Übung 6 wurden zwei Teil-Apps erstellt.

1. Rainbow Table Generator für MD5 Hashes
2. Lookup UI

Beide Programme setzen auf ein PostgreSQL Datenbanksystem auf. Sowohl die Hashes als auch die Statistiken der Abfrage werden darin gespeichert.

### Rainbowtable Generator im Detail
Der Rainbowtable Generator erstellt für beliebig viele Zahlen MD5 Summen und speichert sie in die Datenbank. In diesem Beispiel werden die Hashes für 1 bis 1.000.000 erzeugt. Die Performance konnte durch Threading stark optimiert werden.

### Lookup UI
In der Lookup UI gibt der Benutzer eine Zahl ein. Die Applikation hasht den Wert und sucht ihn in der erzeugten Rainbow Table. Wird ein entsprechender Wert gefunden, erzeugt das Programm in einer separaten Statistik-Tabelle einen Eintrag oder erneuert einen bereits bestehenden.

## Vergleich zu Scale-Out Bruteforce Approach
In einem vorherigen Projekt haben wir eine Applikation erstellt, die Bruteforce Angriffe auf MD5 Hashes ausführen und auf theoretisch beliebig viele Clients verteilen kann. Die Performance konnte durch Scale-Out verbessert werden.
Selbst bei höchster Rechenleistung und starker Skalierung könnte der Bruteforce-Approach die Rainbowtables bei Ganzzahlen im Bereich 1 bis 1 Mio. nicht schlagen. Der PostgreSQL Zugriff ist unglaublich schnell (300ms max.).
Der Bruteforce-Approach ist aber insofern weitsichtiger, da er nicht an eine vorgegebene Range an Werten gebunden ist. Sollte ein Wert nicht in der Lookuptable vorhanden sein, wird dieses Projekt nie ein Ergebnis liefern. "Irgendwann" wird der Bruteforce Angriff jedoch erfolgreich sein.
Die Generation der Rainbowtables dauert knapp eine Minute. Im Prinzip kann man hier Rechenleistung "ausborgen" und gegen Speicher tauschen. Wer gewillt ist, die Rainbowtables zu speichern, spart sich die Neuberechnung der Hashes zur Laufzeit.
