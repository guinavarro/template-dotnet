using Supabase;

namespace Template.Domain.Services;

public class SupabaseService
{
    private readonly Supabase.Client _client;

    public SupabaseService()
    {

        _client = new Supabase.Client("",
        "",
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        });
    }

    public Supabase.Client GetClient() => _client;
}

