using System.Diagnostics.Metrics;

namespace RestAprilEducationRepository.API.Endpoints.Metrics;

public static class MetricsEndpoints
{
    // Meter: uygulamaya ait metric grubu
    private static readonly Meter Meter = new("RestApi.Metrics");

    // Sürekli artan metric: toplam işlenen sipariş sayısı
    private static readonly Counter<long> OrderCounter =
        Meter.CreateCounter<long>("orders.processed.total", "adet", "Toplam işlenen sipariş sayısı");

    // Artan ve azalan metric: anlık aktif kullanıcı sayısı
    private static readonly UpDownCounter<long> ActiveUsersCounter =
        Meter.CreateUpDownCounter<long>("users.active.current", "kişi", "Anlık aktif kullanıcı sayısı");

    public static void AddMetricsEndpoints(this WebApplication app)
    {
        // Her çağrıda sipariş sayacını 1 artırır (sadece artar)
        app.MapPost("/api/metrics/order", () =>
        {
            OrderCounter.Add(1, new KeyValuePair<string, object?>("channel", "web"));
            return Results.Ok(new { message = "Sipariş kaydedildi, counter +1" });
        })
        .WithName("RecordOrder")
        .WithSummary("Sipariş sayacını artırır (Counter — sadece artar)");

        // Kullanıcı girişi: aktif kullanıcı sayısını artırır
        app.MapPost("/api/metrics/user/login", () =>
        {
            ActiveUsersCounter.Add(1);
            return Results.Ok(new { message = "Kullanıcı giriş yaptı, aktif kullanıcı +1" });
        })
        .WithName("UserLogin")
        .WithSummary("Aktif kullanıcı sayısını artırır (UpDownCounter — artar ve azalır)");

        // Kullanıcı çıkışı: aktif kullanıcı sayısını azaltır
        app.MapPost("/api/metrics/user/logout", () =>
        {
            ActiveUsersCounter.Add(-1);
            return Results.Ok(new { message = "Kullanıcı çıkış yaptı, aktif kullanıcı -1" });
        })
        .WithName("UserLogout")
        .WithSummary("Aktif kullanıcı sayısını azaltır (UpDownCounter — artar ve azalır)");
    }
}
