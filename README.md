# Account Overlay

WPF (Windows Presentation Foundation) overlay written in C# that allows for secure storage of accounts and easy login capabilities to Riot Games' applications, but can easily be extended. 

### Functionality:

Follows MVVM (Model View ViewModel) architecture to split the UI from the logic, which allows for easy addition of features. 

- Account form where you can add your account data

![Image](https://github.com/user-attachments/assets/c805b528-ee33-42b4-b4d0-3b5484207f49)
  
- Local Encryption of the account's password (the application does not interact with any servers, it's fully local to enhance security)
- Ability to remove previously stored accounts
- Capability of adding multiple accounts
- Only shows up when the specific game, in this case, Riot Client, is visible
- Types the account data and logs in automatically
- Multiple color themes
- Allows for re-calibration of the click positions (i.e where the username and password boxes are, and where the Login button is)
- Relog capabilities with Computer Vision (checks for specific features of an application, for example, it looks for the Play button to check if you are in game already)
