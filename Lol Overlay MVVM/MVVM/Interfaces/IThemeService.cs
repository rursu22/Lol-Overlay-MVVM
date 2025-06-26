namespace Lol_Overlay_MVVM.MVVM.Model
{
    public interface IThemeService
    {
        Task<string> LoadInitialThemeAsync();
        void CycleTheme();

        string GetCurrentThemeName();
    }
}