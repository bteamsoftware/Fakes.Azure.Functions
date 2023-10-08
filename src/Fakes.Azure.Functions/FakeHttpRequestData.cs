using System.Security.Claims;


namespace Fakes.Azure.Functions;


public sealed class FakeHttpRequestData : HttpRequestData, IDisposable
{
    private bool _isDisposed;
    private readonly Stream _body;
    private readonly HttpHeadersCollection _headers;
    private static readonly FunctionContext _context = new FakeFunctionContext();
    private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);


    public FakeHttpRequestData(string uri) : base(_context)
    {
        Url = new(uri);
        _body = new MemoryStream();
        _headers = new HttpHeadersCollection();
    }


    public FakeHttpRequestData(string uri, string body) : base(_context)
    {
        Url = new(uri);
        _body = new MemoryStream();
        _headers = new HttpHeadersCollection();

        _body.Write(Encoding.UTF8.GetBytes(body));
        _body.Position = 0;
    }


    public override Stream Body => _body;


    public override IReadOnlyCollection<IHttpCookie> Cookies => throw new NotImplementedException();


    public static FakeHttpRequestData CreateInstance<TBody>(string uri, TBody body) =>
        new (uri, JsonSerializer.Serialize(body, options: _serializerOptions));


    public override HttpResponseData CreateResponse() => new FakeHttpResponseData(_context);


    public static HttpResponseData CreateResponse(HttpStatusCode statusCode) =>
        new FakeHttpResponseData(_context, statusCode);


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
                _body?.Dispose();
            }

            _isDisposed = true;
        }
    }


    public override HttpHeadersCollection Headers => _headers;


    public override IEnumerable<ClaimsIdentity> Identities => throw new NotImplementedException();


    public override string Method => throw new NotImplementedException();


    public override Uri Url { get; }
}
