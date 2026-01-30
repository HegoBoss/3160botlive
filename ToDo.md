# 📝 TODO - 3160 Bot (Toernooi Editie)

## 🚨 Hoge Prioriteit (Huidige Focus)
	

- [ ] **Message Handler opzetten**
    - [v] Het `MessageReceived` event in Discord.Net implementeren.
    - [v] Zorgen dat de bot leest wat er geschreven wordt (filteren op berichten die niet van bots zijn).
    - [ ] Basic logica toevoegen: "Als bericht start met `!`, doe iets".
- [ ] **Activatie Command Maken**
	- [v] wakeup command die mensen move door aantal channels voor ze actief weer te maken 
    - [v] Een specifiek command maken (bijv. `!starttourney` of `!moveall`).
    - [v] Zorgen dat alleen admins/mods dit command kunnen gebruiken.
- [ ] **Voice Move Logica**
    - [v] De code schrijven om een `SocketGuildUser` te verplaatsen naar een ander Voice Channel.
    - [v] Check inbouwen: Zit de gebruiker wel in een voice channel? (Anders crasht de bot).

---

## 🏗️ Project Setup & Config
- [v] **Basis configuratie**
    - [v] `launchsettings.json` invullen met Token en Prefix.
    - [V] `.gitignore` checken (zorg dat tokens niet op GitHub komen).
- [v] **Discord Permissions Check**
    - [v] In de Discord Developer Portal: Zorg dat **"Move Members"** aan staat in de bot invite link.
    - [v] In de Discord Developer Portal: Zet **"Message Content Intent"** AAN (belangrijk om tekst te kunnen lezen!).

---

## 🏆 Toernooi Functionaliteit
- [ ] **Team Indeling**
    - [ ] Manier bedenken om te weten wie in welk team zit.
        - *Optie A:* Via Discord Rollen (bijv. rol "Team Rood" gaat naar channel "Rood").
        - *Optie B:* Hardcoded lijsten in de code (tijdelijk).
        - *Optie C:* Een command `!join [teamnaam]` maken.
- [ ] **De "Move" Loop**
    - [ ] Loop door alle gebruikers met een bepaalde rol.
    - [ ] Verplaats ze naar de bijbehorende Voice Channel ID.

---

## 🧹 Opschonen & Testen
- [ ] **Foutafhandeling (Error Handling)**
    - [ ] Wat gebeurt er als het doel-kanaal vol is?
    - [ ] Wat gebeurt er als de bot geen rechten heeft?
- [ ] **Logging**
    - [ ] Console logs toevoegen: "Gebruiker X verplaatst naar Kanaal Y".

---

## 🔮 Toekomst (Backlog)
- [ ] Slash Commands implementeren (beter dan tekst commands, bijv. `/start`).
- [ ] Scorebord bijhouden.
- [ ] Automatisch nieuwe channels aanmaken als er te veel teams zijn.