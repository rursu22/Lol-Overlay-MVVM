# LoL Account Overlay

A WPF (Windows Presentation Foundation) overlay written in C# for securely storing multiple Riot Games account credentials and automating login—fully local, MVVM-based, and themeable.

---

## Features

### Account Management
- **Add / Remove Accounts**  
  Securely store multiple username + password pairs (AES-encrypted locally).  
  <img src="https://github.com/user-attachments/assets/760aff3b-b78e-49af-8652-5b28203f6cc2" alt="Add Account" width="400" />  
  <img src="https://github.com/user-attachments/assets/ade25250-12b7-4fa4-a1c0-8ba975e3057b" alt="List Accounts" width="400" />

### Automatic Login
- **Overlay only shows when Riot Client is visible**  
- **Types credentials and clicks “Login”** for you  
- **Re-login support** via computer vision—detects in-game state (e.g. Play button) and logs out/in as needed

### Calibration
- **Point-and-click calibration** of screen coordinates for:
  - Username box  
  - Password box  
  - Login button  
  <img src="https://github.com/user-attachments/assets/18b593f2-a2f8-4a0b-b15b-e42877a561a9" alt="Calibrate Username" width="400" />  
  <img src="https://github.com/user-attachments/assets/23ff7e7b-14ad-40ca-88c2-db931ae566f4" alt="Calibrate Password & Login" width="400" />

### Theming
- **Multiple color themes**, switchable at runtime  
- Persisted across restarts  
<img src="https://github.com/user-attachments/assets/bce0812e-cd26-4451-a582-eed92ccea31e" alt="Theme Preview" width="400" />

## Startup with Windows

Right-click the tray icon → **Start with Windows** to toggle auto-launch on login.

---

## Architecture

- **MVVM pattern**  
  - **Models** handle data and services (encryption, file I/O, computer vision)  
  - **ViewModels** expose `ICommands` & bindable properties  
  - **Views** (XAML + code-behind) purely for UI  

---

## Getting Started

1. Download the latest ZIP from our [Releases page](https://github.com/rursu22/LoL-Account-MVVM/releases)
2. Run "Lol Overlay MVVM.exe"

---

## Security & Privacy

- **All data is stored locally**, encrypted with AES  
- **No external servers**—the overlay never sends your credentials over the network  
- **Open-source**: inspect or modify as you wish  

---

## 🤝 Contributing

Feel free to open issues or submit PRs to add features (e.g. support for other games, advanced CV, custom installers).
