# Account Overlay

WPF (Windows Presentation Foundation) overlay written in C# that allows for secure storage of accounts and easy login capabilities to Riot Games' applications, but can easily be extended. 

### Functionality:

Follows MVVM (Model View ViewModel) architecture to split the UI from the logic, which allows for easy addition of features. 

- Account form where you can add your account data

![Image](https://github.com/user-attachments/assets/760aff3b-b78e-49af-8652-5b28203f6cc2)

- Capability of adding multiple accounts

![Image](https://github.com/user-attachments/assets/ade25250-12b7-4fa4-a1c0-8ba975e3057b)
  
- Ability to remove previously stored accounts
- Local Encryption of the account's password (the application does not interact with any servers, it's fully local to enhance security)
- Only shows up when the specific game, in this case, Riot Client, is visible
- Types the account data and logs in automatically
- Multiple color themes

![image](https://github.com/user-attachments/assets/bce0812e-cd26-4451-a582-eed92ccea31e)

- Allows for re-calibration of the click positions (i.e where the username and password boxes are, and where the Login button is)

![image](https://github.com/user-attachments/assets/18b593f2-a2f8-4a0b-b15b-e42877a561a9)

![image](https://github.com/user-attachments/assets/83bc8844-81b9-4029-b68a-461367abe0b4)
  
- Relog capabilities with Computer Vision (checks for specific features of an application, for example, it looks for the Play button to check if you are in game already)
