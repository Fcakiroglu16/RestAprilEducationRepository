using System.Diagnostics.Metrics;

namespace RestAprilEducationRepository.API.Metrics;

/// <summary>
/// Uygulamaya ait OpenTelemetry/System.Diagnostics.Metrics instrument tanımları.
/// </summary>
public sealed class AppMetrics
{
    public const string MeterName = "RestAprilEducation.API";

    private readonly Meter _meter;

    /// <summary>
    /// Sürekli artan metric (Counter).
    /// Her çağrıda yalnızca artırılabilir, hiçbir zaman azalmaz.
    /// Örnek kullanım: toplam sipariş sayısı, toplam istek sayısı.
    /// </summary>
    public Counter<int> OrdersCreated { get; }

    /// <summary>
    /// Hem artabilen hem azalabilen metric (UpDownCounter).
    /// Örnek kullanım: anlık aktif bağlantı sayısı, kuyruktaki iş sayısı.
    /// </summary>
    public UpDownCounter<int> ActiveConnections { get; }

    public AppMetrics()
    {
        _meter = new Meter(MeterName);


        OrdersCreated = _meter.CreateCounter<int>(
            name: "orders.created",
            unit: "{order}",
            description: "Şimdiye kadar oluşturulan toplam sipariş sayısı (sürekli artar).");

        ActiveConnections = _meter.CreateUpDownCounter<int>(
            name: "active.connections",
            unit: "{connection}",
            description: "Anlık aktif bağlantı sayısı (artabilir veya azalabilir).");
    }
}
