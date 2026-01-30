# Manager Organization - Hackerman Project

## Overview
La riorganizzazione dei manager ha l'obiettivo di separare le responsabilità e ridurre le sovrapposizioni tra i diversi sistemi.

## Manager Architecture

### 🎮 **GameManager** 
**Responsabilità**: Controllo dello stato generale del gioco
- Gestione stati del gioco (Playing, GameOver, Paused)
- Coordinamento tra i vari sistemi
- Controllo del time scale
- Restart del gioco

### 🔊 **AudioManager** 
**Responsabilità**: Gestione centralizzata di tutti gli audio
- Musica di background (soft, metal, game over)
- Effetti sonori del player (passi, imprecazioni)  
- Effetti sonori della mazza (colpi)
- Caricamento automatico delle risorse audio

### 🖥️ **UIManager**
**Responsabilità**: Gestione di tutta l'interfaccia utente
- Display del timer
- Visualizzazione del bonus tempo
- Counter dei computer riparati
- Aggiornamenti real-time dell'UI

### 🏆 **ScoreManager**
**Responsabilità**: Gestione del punteggio e statistiche
- Conteggio computer riparati
- Comunicazione con UIManager per aggiornamenti
- Reset del punteggio

### ⏰ **TimeManager**
**Responsabilità**: Gestione del timer di gioco
- Countdown del tempo rimanente
- Aggiunta di tempo bonus
- Rilevamento fine partita per timeout

### 🏠 **RoomManager** 
**Responsabilità**: Logica delle singole stanze
- Controllo dei computer nella stanza
- Apertura/chiusura porte
- Animazioni delle porte
- Trigger per cambio camera

### 🗂️ **TileManager**
**Responsabilità**: Gestione della generazione delle stanze
- Creazione e rimozione delle stanze
- Gestione della posizione delle stanze
- Ottimizzazione memoria (rimozione stanze vecchie)

### 📷 **CameraManager**
**Responsabilità**: Controllo della camera
- Movimento fluido della camera tra stanze
- Posizionamento iniziale
- Transizioni animate

## Flusso di Comunicazione

```
Computer.SetRepaired() 
    → GameManager.OnComputerRepaired() 
    → ScoreManager.OnComputerRepaired() 
    → UIManager.UpdateCounter()

RoomManager.CheckForDoor()
    → AudioManager.PlayMetalSong() (se prima stanza)
    → TimeManager.AddTimer()

Player/Mazza actions
    → AudioManager.PlaySound()

TimeManager.Update()
    → UIManager.UpdateTimerDisplay()
    → GameManager.OnTimerFinished() (se tempo scaduto)
```

## Vantaggi della Nuova Organizzazione

1. **Single Responsibility**: Ogni manager ha una responsabilità specifica
2. **Loose Coupling**: I manager comunicano attraverso interfacce chiare
3. **Manutenibilità**: Più facile debuggare e aggiornare singole funzionalità
4. **Scalabilità**: Facile aggiungere nuove funzionalità senza conflitti
5. **Testabilità**: Ogni manager può essere testato in isolamento

## File Rimossi
- `MazzaAudioManager.cs` → consolidato in `AudioManager.cs`
- `PlayerAudioManger.cs` → consolidato in `AudioManager.cs`

## File Creati
- `AudioManager.cs` - Audio centralizzato
- `UIManager.cs` - UI centralizzata
- `ScoreManager.cs` - Gestione punteggi
- `CameraManager.cs` - Controllo camera