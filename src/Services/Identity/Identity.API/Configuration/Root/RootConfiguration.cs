namespace drDotnet.Services.Identity.API.Configuration.Root
{
    public class RootConfiguration : IRootConfiguration
    {
        public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
    }
}