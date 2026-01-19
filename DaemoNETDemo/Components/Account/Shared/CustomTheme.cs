using MudBlazor;

namespace DaemoNETDemo.Components.Account.Shared
{
    
        public static class CustomTheme
        {
            public static MudTheme GetTheme() => new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    Primary = "#0FBE7C",          // Light Green
                    Secondary = "#FFCC80",        // Light Orange
                    Background = "#F9F9F9",       // Very Light Gray Background
                    Surface = "#FFFFFF",          // Clean White Surface
                    AppbarBackground = "#50992C", // Stronger green for AppBar
                    AppbarText = "#FFFFFF",       // White text on AppBar
                    DrawerBackground = "#FFFFFF",
                    DrawerText = "#37474F",       // Dark Gray
                    DrawerIcon = "#607D8B",
                    TextPrimary = "#212121",      // Standard dark text
                    TextSecondary = "#616161",    // Lighter text
                    ActionDefault = "#757575",    // Icon/Action colors
                    LinesDefault = "#E0E0E0",     // Border lines
                    TableLines = "#E0E0E0",
                    Divider = "#BDBDBD"
                },
                PaletteDark = new PaletteDark()
                {
                    Primary = "#FFB74D",          // Orange Accent
                    Secondary = "#FF5252",        // Red Accent
                    Background = "#121212",
                    Surface = "#1F1F1F",
                    AppbarBackground = "#1F1F1F",
                    AppbarText = "#FFFFFF",
                    TextPrimary = "#E0E0E0"
                }
            };
        }
    
}
