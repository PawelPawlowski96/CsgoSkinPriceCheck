namespace CsgoSkinPriceCheck

{
 
   public class PobraneDane
   
 {
        
public string nazwaBroni 
{ get; set; 
}
        
public string nazwaSkina 
{
 get;
 set;
 }
        
public string cenaSkina 
{ 
get; 
set; 
}
        
public string cenaStatTrak 
{
 get;
 set; 
}
        
public string sciezkaDoGrafiki 
{ get; 
set; 
}
 
 public int numerLiniiNazwySkina 
{ get;
 set;
 }
 public PobraneDane(string nazwaBroni, string nazwaSkina, int numerLiniiNazwySkina)
        
{
            
this.nazwaBroni = nazwaBroni;
            
this.nazwaSkina = nazwaSkina;
           
this.numerLiniiNazwySkina = numerLiniiNazwySkina;
        
}

        
public PobraneDane(string nazwaBroni, string nazwaSkina, string cenaSkina, string cenaStatTrak, string sciezkaDoGrafiki)
        
{
            
this.nazwaBroni = nazwaBroni;
            
this.nazwaSkina = nazwaSkina;
            
this.cenaSkina = cenaSkina;
            
this.cenaStatTrak = cenaStatTrak;
            
this.sciezkaDoGrafiki = sciezkaDoGrafiki;
        
}

}

}
