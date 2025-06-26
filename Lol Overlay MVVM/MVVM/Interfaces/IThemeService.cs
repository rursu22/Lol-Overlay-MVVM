namespace Lol_Overlay_MVVM.MVVM.Model
{
    public interface IThemeService
    {
        void LoadInitialTheme();
        void CycleTheme();

        string GetCurrentThemeName();
    }
}