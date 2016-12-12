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


}

}
