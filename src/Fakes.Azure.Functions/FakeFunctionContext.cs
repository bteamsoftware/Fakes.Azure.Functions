using Azure.Core.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Fakes.Azure.Functions;


public sealed class FakeFunctionContext : FunctionContext, IDisposable
{
    private bool _isDisposed;
    private readonly IServiceProvider _services;
    private readonly CancellationTokenSource _cancellationTokenSource;


    public FakeFunctionContext()
    {
        _services = new ServiceCollection()
            .AddLogging()
            .AddSingleton(Options.Create(new WorkerOptions
            {
                Serializer = new JsonObjectSerializer(new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                })
            }))
            .BuildServiceProvider();

        _cancellationTokenSource = new CancellationTokenSource();
    }


    public override BindingContext BindingContext => throw new NotImplementedException();


    public override CancellationToken CancellationToken => _cancellationTokenSource.Token;


    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }


    private void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _cancellationTokenSource.Dispose();
            }

            _isDisposed = true;
        }
    }


    public override IInvocationFeatures Features => throw new NotImplementedException();


    public override FunctionDefinition FunctionDefinition => throw new NotImplementedException();


    public override string FunctionId => throw new NotImplementedException();


    public override IServiceProvider InstanceServices
    {
        get => _services;
        set => throw new NotImplementedException();
    }


    public override string InvocationId => throw new NotImplementedException();


    public override IDictionary<object, object> Items
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }


    public override RetryContext RetryContext => throw new NotImplementedException();


    public override TraceContext TraceContext => throw new NotImplementedException();
}
