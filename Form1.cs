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

    
}

}
