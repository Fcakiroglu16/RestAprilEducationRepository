using RestAprilEducationRepository.API.Metrics;

namespace RestAprilEducationRepository.API.Endpoints.Metrics;

public static class MetricsEndpoints
{
    public static void AddMetricsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/metrics").WithTags("Metrics");

        // ---------------------------------------------------------------
        // Counter endpoint — yalnızca artırılabilir
        // ---------------------------------------------------------------

        // Belirtilen miktarda sipariş oluşturulmuş gibi counter'ı artırır.
        group.MapPost("orders/increment", (AppMetrics metrics, int count = 1) =>
        {
            if (count <= 0)
                return Results.BadRequest("count 0'dan büyük olmalıdır.");

            metrics.OrdersCreated.Add(count, new KeyValuePair<string, object?>("source", "manual-test"));

            return Results.Ok(new
            {
                Message = $"orders.created counter {count} artırıldı.",
                MetricName = "orders.created",
                AddedValue = count
            });
        })
        .WithSummary("Counter artır (sadece artar)")
        .WithDescription("orders.created counter'ını belirtilen miktar kadar artırır. " +
                         "Counter hiçbir zaman azalmaz — toplam oluşturulan sipariş sayısını temsil eder.");

        // ---------------------------------------------------------------
        // UpDownCounter endpoints — artabilir veya azalabilir
        // ---------------------------------------------------------------

        // Yeni bir bağlantı açıldığında çağrılır.
        group.MapPost("connections/connect", (AppMetrics metrics) =>
        {
            metrics.ActiveConnections.Add(1, new KeyValuePair<string, object?>("endpoint", "test"));

            return Results.Ok(new
            {
                Message = "active.connections +1 artırıldı (yeni bağlantı).",
                MetricName = "active.connections",
                Delta = +1
            });
        })
        .WithSummary("UpDownCounter artır (bağlantı aç)")
        .WithDescription("Bir bağlantı açıldığında active.connections counter'ını 1 artırır.");

        // Bir bağlantı kapandığında çağrılır.
        group.MapPost("connections/disconnect", (AppMetrics metrics) =>
        {
            metrics.ActiveConnections.Add(-1, new KeyValuePair<string, object?>("endpoint", "test"));

            return Results.Ok(new
            {
                Message = "active.connections -1 azaltıldı (bağlantı kapandı).",
                MetricName = "active.connections",
                Delta = -1
            });
        })
        .WithSummary("UpDownCounter azalt (bağlantı kapat)")
        .WithDescription("Bir bağlantı kapandığında active.connections counter'ını 1 azaltır.");
    }
}
