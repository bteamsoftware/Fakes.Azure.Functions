namespace Fakes.Azure.Functions;


public class FakeHttpResponseData : HttpResponseData, IDisposable
{
    private bool _isDisposed;


    public FakeHttpResponseData(FunctionContext context) : base(context)
    {
    }


    public FakeHttpResponseData(FunctionContext context, HttpStatusCode statusCode) : base(context)
    {
        StatusCode = statusCode;
    }


    public override Stream Body { get; set; } = new MemoryStream();


    public override HttpCookies Cookies => throw new NotImplementedException();


    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                Body.Dispose();
            }

            _isDisposed = true;
        }
    }


    public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();


    public override HttpStatusCode StatusCode { get; set; }
}
