#  Audi A6 Virtual Instrument Cluster Simulation

##  Descriere

**Audi A6 Virtual Instrument Cluster Simulation** este o aplicație desktop dezvoltată în **C# (Windows Forms)** care simulează un bord auto digital.

Proiectul reproduce comportamentul dinamic al unui vehicul real, combinând:

* randare grafică în timp real
* simulare de parametri fizici (viteză, turații, temperatură)
* logică de transmisie automată

---

##  Funcționalități principale

###  1. Vizualizarea datelor telemetrice

* **Turometru & Vitezometru**

  * afișare analogică și digitală
  * ac indicator animat în timp real

* **Temperatura motorului**

  * simulare progresivă în funcție de sarcină și turații

* **Consum combustibil**

  * calcul dinamic în funcție de:

    * viteză
    * treapta de viteză

---

###  2. Transmisie și cinematică

* **Cutie automată cu 8 trepte (D1–D8)**

* **Moduri disponibile:**

  * Drive (D)
  * Park (P)

* **Schimbare automată a treptelor**

  * bazată pe:

    * RPM (turații)
    * viteză
    * praguri optimizate

---

###  3. Sistem de semnalizare

* Semnalizare:

  * stânga
  * dreapta
  * avarii

* Implementare:

  * sistem de blink folosind **timere**
  * sincronizare vizuală realistă

---

##  Control utilizator

Aplicația este controlată din tastatură după apăsarea butonului **Start**.

| Tastă | Funcție             |
| ----- | ------------------- |
| **W** | Accelerare          |
| **S** | Semnalizare stânga  |
| **D** | Semnalizare dreapta |
| **A** | Avarii              |
| **O** | Oprire semnalizare  |

---

##  Exemple vizuale

### 🔹 Interfață principală

![Cluster View](https://github.com/user-attachments/assets/f5a93eac-e8c4-4db1-9f4a-c59b11a5faea)

---

### 🔹 Funcționare în timp real

![Live Simulation](https://github.com/user-attachments/assets/45c932d4-c1f9-46a5-bd70-218088638ef0)

---

##  Tehnologii utilizate

* **C# (.NET Framework)**
* **Windows Forms**
* **GDI+ (System.Drawing)**

---

##  Optimizări

* utilizare **Double Buffering**
* eliminare efect **flicker**
* randare fluidă în timp real

---

##  Arhitectură

* programare orientată pe obiecte (OOP)
* componente separate pentru:

  * indicatori
  * logică vehicul
  * UI

---

##  Scopul proiectului

Acest proiect a fost realizat pentru a demonstra:

* abilități de programare în C#
* lucru cu grafică 2D (GDI+)
* simularea sistemelor dinamice
* organizarea unui proiect modular

---

##  Posibile îmbunătățiri

* integrare sunete (motor, semnalizare)
* UI modern (WPF)
* simulare mai realistă a fizicii
* suport pentru input controller

---

##  Disclaimer

Acesta este un proiect educațional și nu reflectă complet comportamentul unui sistem auto real.
