Audi A6 Virtual Instrument Cluster Simulation

Descriere Generală
Această aplicație reprezintă un simulator de bord auto (Instrument Cluster) dezvoltat în mediul de programare C#, utilizând tehnologia Windows Forms. Proiectul simulează comportamentul dinamic al unui vehicul Audi A6, integrând atât elemente de randare grafică în timp real, cât și o logică complexă de calcul pentru parametrii fizici ai motorului și transmisiei.

Funcționalități Implementate
1. Vizualizarea Datelor Telemetrice
Turometru și Vitezometru: Redare grafică a instrumentelor analogice și digitale, cu scalare dinamică a acelor indicatoare.

Monitorizarea Temperaturii: Algoritm de simulare a încălzirii motorului în funcție de sarcină și regimul de turații.

Sistem de Gestiune a Combustibilului: Calculul consumului în timp real bazat pe viteza de deplasare și treapta de viteză selectată.

2. Transmisie și Cinematică
Cutie de Viteze Automată: Implementarea unei logici de transmisie cu 8 rapoarte (D1-D8), incluzând modul "Park" (P).

Schimbare Automată a Treptelor: Logica de trecere între rapoartele de viteză este determinată algoritmic de pragul optim de turații (RPM) și viteză.

3. Semnalizare și Avertizare
Sistem de semnalizare direcțională (stânga/dreapta) și avarii, sincronizat prin timere dedicate pentru intermitență vizuală.

Specificații Tehnice
Limbaj de Programare: C# (.NET Framework)

Randare Grafică: Utilizarea bibliotecii GDI+ (System.Drawing) pentru desenarea procedurală a componentelor.

Optimizare: Implementarea clasei BufferedPanel cu tehnica de Double Buffering pentru eliminarea efectului de flicker și asigurarea unei fluidități ridicate la nivelul interfeței utilizator.

Arhitectură: Abordare orientată pe obiecte, utilizând clase modulare pentru fiecare indicator în parte.

Interacțiunea cu Utilizatorul
Sistemul este operat prin intermediul tastaturii, după inițializarea prealabilă prin butonul principal de control (Start/Stop).

Tastă  Funcție
 W     Accelerare (Creșterea turației și a vitezei)
 S     Activare semnalizare stânga
 D     Activare semnalizare dreapta
 A     Activare lumini de avarie
 O     Dezactivare sisteme de semnalizare
