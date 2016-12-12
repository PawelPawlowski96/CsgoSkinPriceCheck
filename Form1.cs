using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Drawing;

using System.Linq;

using System.Windows.Forms;

using System.Net;

using System.IO;

using System.Threading;


namespace CsgoSkinPriceCheck

{
    
public partial class Form1 : Form
    
{
        
ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
        
private string[] bronie = { "AK-47", "AUG", "AWP", "Bayonet", "Bowie Knife", "Butterfly Knife", "CZ75-Auto", "Desert Eagle", "Dual Berettas", "Falchion Knife", "FAMAS", "Five-SeveN", "Flip Knife", "G3SG1",
            "Galil AR", "Glock-18", "Gut Knife", "Huntsman Knife", "Karambit", "M249", "M4A1-S", "M4A4", "M9 Bayonet", "MAC-10", "MAG-7", "MP7", "MP9", "Negev", "Nova", "P2000", "P250", "P90", "PP-Bizon", "Sawed-Off", "SCAR-20",
            "SG 553", "SSG 08", "Shadow Daggers", "Tec-9", "UMP-45", "USP-S", "XM1014" };
        
private List<PobraneDane> sortedListaZDanymi = new List<PobraneDane>();
        
private string[] linieStrony;
        
Thread pobieranie;
        
Thread usuwanie;
        
int igaben = 1;
        
string obecniePobieranaBron;

        
public Form1()
        
{
            
InitializeComponent();

 }

   private void Form1_Load(object sender, EventArgs e)
        
{
            
pobieranie = new Thread(pobieranieDanych);
            
usuwanie = new Thread(usuwanieDanych);
            
label8.Visible = false;
            
comboBox1.Text = "Wybierz broń";     
//Dodawanie elementów to pierwszego combo boxa.
            
for (int i = 0; i < bronie.Length; i++)
            
{
                
comboBox1.Items.Add(bronie[i]);
            
}

            
Directory.CreateDirectory(@"C:\Windows\Temp\CsgoSkinPriceCheck\images\");
            
pictureBox1.Image = new Bitmap(Properties.Resources.gaben1);
        
}

        
private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        
{
            
pictureBox1.Image = new Bitmap(Properties.Resources.gaben1);
            
checkBox1.Enabled = false;
            
comboBox2.Items.Clear();
            
comboBox2.Text = "";
            
label3.Text = "---";
            
label4.Text = "---";

            
if (!checkBox1.Checked)
                
sortedListaZDanymi.Clear();

 //Tutaj na dobrą sprawę program obiera jedną z dwóch możliwych dróg, w zależności od podjętej przez użytkownika decyzji. Może pobierać całą zawartość przed
            
//dopuszczeniem użytkownika do użytkowania (wariant comboBox1SelectedOffline), albo będzie pobierał wszelkie dane na bieżąco (comboBox1SelectedOnline).
            
if (checkBox1.Checked)
                
comboBox1SelectedOffline();
            
else
                
comboBox1SelectedOnline();
        
}

        
public void comboBox1SelectedOnline()
        
{
            
try
            
{  
//Program pobierze zawartość odpowiedniej strony (i zapisze ją do tablicy), zależnie od tego, którą broń wybrał użytkownik.
                
label8.Visible = true;
                
string stronaZeSkinami = new WebClient().DownloadString(ustalanieAdresuUrl(comboBox1.Text));
                
File.WriteAllText(@"C:\Windows\Temp\CsgoSkinPriceCheck\CsgoSkinPriceCheck_temp1.txt", stronaZeSkinami);
                
linieStrony = File.ReadAllLines(@"C:\Windows\Temp\CsgoSkinPriceCheck\CsgoSkinPriceCheck_temp1.txt");

                
List<PobraneDane> listaZDanymi = new List<PobraneDane>();
                
int nrLn;               
ustalanieNumeruLiniiZPierwszymSkinem(linieStrony, out nrLn);
//Tworzenie obiektów zawierających informacje o danym skinie: nazwa broni, nazwa skina, numer linni w której owa nazwa skina występuje.
                
//To ostatnie posłuży potem do odnalezienia cen i grafiki.
                
while (nrLn != 0)
                
{
                    
string skin = ustalanieNazwySkina(linieStrony, nrLn);
                    
PobraneDane skinn = new PobraneDane(comboBox1.Text, skin, nrLn);
                    
listaZDanymi.Add(skinn);
                    
ustalanieNastepnegoNumeruStrony(linieStrony, comboBox1.Text, ref nrLn);
                
}
//Skiny na stronie nie są podane w kolejności alfabetycznej, więc wypadałoby je posortować, a dopiero potem dodać do drugiego combo boxa.
                
sortedListaZDanymi = listaZDanymi.OrderBy(o => o.nazwaSkina).ToList();

                
for (int j = 0; j < sortedListaZDanymi.Count; j++)
                
{
                    
comboBox2.Items.Add(sortedListaZDanymi[j].nazwaSkina);
                
}
                
label8.Visible = false;
            
}
            
catch (WebException)
            
{
                
MessageBox.Show("Wprowadź prawidłową nazwę broni");
            
}
            
catch (Exception ex)
            
{
                
MessageBox.Show(ex.ToString());
            
}
        
}

        
public void comboBox1SelectedOffline()
       
 {
 //Program sprawdza czy pobieranie danych nadal trwa. Jeśli się zakończyło, przepuści dalej. Jeśli nie, wyświetli informację
            
//o tym, ile zostało już pobrane.
            
int postep = 0;
            
if (pobieranie.IsAlive)
            
{
                
if (obecniePobieranaBron != null)
                
{
                    
for (; postep < bronie.Length; postep++)
                    
{
                        
if (bronie[postep] == obecniePobieranaBron)
                            
break;
                    
}
                
}
                
string wynik = Convert.ToString((float)postep / (float)bronie.Length * 100);
                
if (wynik.Length >= 4)
                    
wynik = wynik.Substring(0, 4);
                
MessageBox.Show("Poczekaj na zakończenie pobierania. \nUkończono około " + wynik + "%");

  }
            
else
            
{
                
try
                
{
                    
for (int i = 0; i < sortedListaZDanymi.Count; i++)
                    
{
                        
if (sortedListaZDanymi[i].nazwaBroni == comboBox1.Text)
                           
 comboBox2.Items.Add(sortedListaZDanymi[i].nazwaSkina);
                    
}
                
}
                
catch (Exception ex)
                
{
                    
MessageBox.Show(ex.ToString());
                
}
            
}
        
}

        
private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        
{
//Po zaznaczeniu wartości w drugim combo boxie, program znowy wybiera którą ścieżką pójść. Znowu w zależności od statusu check boxa.
            
label7.Text = "";
            
if (checkBox1.Checked)
                
comboBox2SelectedOffline();
            
else
                           
comboBox2SelectedOnline();
        
}

        
public void comboBox2SelectedOnline()
        
{
            
try
            
{
                
label8.Visible = true;
                
string liniaZCena;
                
int k = 0;
//Ta pętla odczytuje który skin został wybrany.
                
for (; k < sortedListaZDanymi.Count; k++)
                
{
                    
if (comboBox2.Text == sortedListaZDanymi[k].nazwaSkina)
                        
break;
                
}

                
liniaZCena = linieStrony[sortedListaZDanymi[k].numerLiniiNazwySkina + 13];
                
string liniaTestowa = linieStrony[sortedListaZDanymi[k].numerLiniiNazwySkina + 16]; 
//Linia testowa, bo skin niekoniecznie posiada wersję StatTrak lub Souvenir.
                
if (linieStrony[sortedListaZDanymi[k].numerLiniiNazwySkina + 7].Contains("Available"))
                
{
                    
ustalanieCeny(ref liniaTestowa);

                    
if (linieStrony[sortedListaZDanymi[k].numerLiniiNazwySkina + 7].Contains("StatTrak"))
                        
label4.Text = liniaTestowa + " (StatTrak)";
                    
if (linieStrony[sortedListaZDanymi[k].numerLiniiNazwySkina + 7].Contains("Souvenir"))
                        
label4.Text = liniaTestowa + " (Souvenir)";
                
}

                
else
                    
label4.Text = "Brak";

                
ustalanieCeny(ref liniaZCena);
                
label3.Text = liniaZCena;

                
int numerLiniiZGrafika = sortedListaZDanymi[k].numerLiniiNazwySkina + 10;
               
 pictureBox1.Load(ustalanieLikuDoGrafiki(numerLiniiZGrafika, linieStrony));
                
label8.Visible = false;
            
}
catch (ArgumentOutOfRangeException)
            
{
                
MessageBox.Show("Wprowadź prawidłową nazwę skina.");
            
}
            
catch (Exception ex)
            
{
                
MessageBox.Show(ex.ToString());
            
}
        
}
public void comboBox2SelectedOffline()
       
 {
           
 try
            
{
                
int m = 0;
                
for (; m < sortedListaZDanymi.Count; m++)
                
{
                    
if (comboBox1.Text == sortedListaZDanymi[m].nazwaBroni && comboBox2.Text == sortedListaZDanymi[m].nazwaSkina)
                        
break;
                
}
label3.Text = sortedListaZDanymi[m].cenaSkina;
                
label4.Text = sortedListaZDanymi[m].cenaStatTrak;
                
pictureBox1.Load(@"C:\Windows\Temp\CsgoSkinPriceCheck\images\" + sortedListaZDanymi[m].sciezkaDoGrafiki);
            
}
            
catch (ArgumentOutOfRangeException)
            
{
                
MessageBox.Show("Wprowadź prawidłową nazwę skina.");
            
}
            
catch (Exception ex)
            
{
                
MessageBox.Show(ex.ToString());
            
}
        
}
private void checkBox1_CheckedChanged(object sender, EventArgs e)
        
{
            
if (checkBox1.Checked)
            
{
                
checkBox1.Enabled = false;
                
pobieranie.Start();
           
 }
       
 }
 public string ustalanieAdresuUrl(string wartoscPola)
        
{
            
//W adresach na tej stronie spacje są zamieniane na "+".
            
if (wartoscPola.Contains(" "))
                
wartoscPola = wartoscPola.Replace(" ", "+");

            
return "https://csgostash.com/weapon/" + wartoscPola;
        
}

public string ustalanieNazwySkina(string[] tablica, int linijka)
        
{
            
//Przykładowa linia wygląda tak: <h3><a href="https://csgostash.com/family/Scumbria">Scumbria</a></h3>
            
int index1 = tablica[linijka].IndexOf(">", 11);
            
int index2 = tablica[linijka].IndexOf("</a></h3>", 11);
            
string nazwaSkina = tablica[linijka].Substring(index1 + 1, index2 - index1 - 1);

            
if (nazwaSkina == "â… (Vanilla)")
                
return "Vanilla";

            
if (nazwaSkina == "Sunset Storm ĺŁ±")
                
return "Sunset Storm 1";

            
if (nazwaSkina == "Sunset Storm ĺĽ")
                
return "Sunset Storm 2";

            
if (nazwaSkina == "éľŤçŽ‹ (Dragon King)")
                
return "Dragon King";

            
return nazwaSkina;

        
}
public void ustalanieCeny(ref string linia)
        
{
            
//Przykładowa linia wygląda tak: 
            
//<p class="nomargin"><a href="https://csgostash.com/skin/649/XM1014-Scumbria" data-toggle="tooltip" data-placement="right" title="0  - 0 Keys">0,08&euro; - 0,16&euro;</a></p>
            
try
            
{
                
int odkad = linia.IndexOf(">", 50) + 1;
                
linia = linia.Substring(odkad, linia.Length - odkad - 8);
                
linia = linia = linia.Replace(",--", "").Replace("&euro;", "") + " €";
            
}
            
catch (ArgumentOutOfRangeException)
            
{
                
linia = "Brak Info";
            
}
        
}
public string ustalanieLikuDoGrafiki(int numerLinii, string[] linieStrony)
        
{
            
//Przykładowa linia wygląda tak:
            
//<a href="https://csgostash.com/skin/649/XM1014-Scumbria"><img class="img-responsive center-block" src="https://csgostash.com/img/skins/s649fn.png" alt="XM1014 Scumbria"></a>
            
int odkad = linieStrony[numerLinii].IndexOf("src", 0) + 5;
            
string linkDoGrafiki = linieStrony[numerLinii].Substring(odkad);
            
int ileUsunac = linkDoGrafiki.IndexOf("alt", 0) - 2;
            
linkDoGrafiki = linkDoGrafiki.Remove(ileUsunac);
            
return linkDoGrafiki;
        
}
public void ustalanieNastepnegoNumeruStrony(string[] tablica, string nazwa, ref int numer)
        
{
           
 if (tablica[numer + 37].Contains("text/javascript")) //<script type='text/javascript'>
                
numer += 27 + 35;
            
else
           
 {
                
if (tablica[numer + 35].Contains("family")) //<h3><a href="https://csgostash.com/family/Scumbria">Scumbria</a></h3>
                    
numer += 35;
                
else
               
 {
                    
if ((tablica[numer + 35].Contains("row") && tablica[numer + 35].Contains("<div class=")) || tablica[numer + 35].Contains("Default"))
                        
numer = 0;
                
}
            
}
        
}
public void pobieranieDanych()
        
{
            
List<PobraneDane> listaZDanymi = new List<PobraneDane>();

            
for (int z = 0; z < bronie.Length; z++)
            
{
                
obecniePobieranaBron = bronie[z];
                
string stronaZeSkinami = new WebClient().DownloadString(ustalanieAdresuUrl(obecniePobieranaBron));
                
File.WriteAllText(@"C:\Windows\Temp\CsgoSkinPriceCheck\CsgoSkinPriceCheck_temp1.txt", stronaZeSkinami);
                
linieStrony = File.ReadAllLines(@"C:\Windows\Temp\CsgoSkinPriceCheck\CsgoSkinPriceCheck_temp1.txt");

                
int nrLn;
                
ustalanieNumeruLiniiZPierwszymSkinem(linieStrony, out nrLn);

                
while (nrLn != 0)
                
{
                    
string _skin = ustalanieNazwySkina(linieStrony, nrLn);
                    
string liniaZCena = linieStrony[nrLn + 13];
                    
string liniaTestowa = linieStrony[nrLn + 16];

                    
if (linieStrony[nrLn + 7].Contains("Available"))
                    
{
                        
ustalanieCeny(ref liniaTestowa);

                        
if (linieStrony[nrLn + 7].Contains("StatTrak"))
                            
liniaTestowa = liniaTestowa + " (StatTrak)";
                        
if (linieStrony[nrLn + 7].Contains("Souvenir"))
                            
liniaTestowa = liniaTestowa + " (Souvenir)";
                    
}

                    ustalanieCeny(ref liniaZCena);

                    
int numerLiniiZGrafika = nrLn + 10;
                    
string nazwaGrafiki = obecniePobieranaBron + " " + _skin + ".png";
                    
new WebClient().DownloadFile(ustalanieLikuDoGrafiki(numerLiniiZGrafika, linieStrony), @"C:\Windows\Temp\CsgoSkinPriceCheck\images\" + nazwaGrafiki);
                    
PobraneDane dane = new PobraneDane(obecniePobieranaBron, _skin, liniaZCena, liniaTestowa, nazwaGrafiki);
                    
listaZDanymi.Add(dane);
                    
ustalanieNastepnegoNumeruStrony(linieStrony, dane.nazwaBroni, ref nrLn);
                
}
            
}

            
sortedListaZDanymi = listaZDanymi.OrderBy(o => o.nazwaSkina).ToList();

            
checkBox1.Enabled = false;
            
button1.Enabled = true;

            
MessageBox.Show("Pobrano zawartość.");
        
}

}

}
