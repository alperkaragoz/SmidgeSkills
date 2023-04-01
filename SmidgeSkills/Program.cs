using Smidge;
using Smidge.Cache;
using Smidge.Options;

var builder = WebApplication.CreateBuilder(args);

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSmidge(configuration.GetSection("smidge"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSmidge(bundle =>
{
    //Js root i�erisindeki b�t�n javascript dosyalar�na smidge uyguluyor.
    bundle.CreateJs("bundle-s", "~/js");

    //EnableCompositeProcessing => Birle�tirme i�lemini ger�ekle�tiriyor.
    //EnableFileWatcher         => Bundle i�erisindeki dosyalar� izler ve de�i�iklik alg�larsa cache dosyas�n�(App_Data)s�f�rdan olu�turuyor.
    //SetCacheBusterType        => App_Data cache dosyas�n� uygulama her aya�a kalkt���nda olu�turur.
    //CacheControlOptions       => Browser' a, olu�an bundle dosyas�n� hi� cachelememesini s�yl�yoruz.
    //enableTag                 => Browser, serverdan image veya dosya �ekti�i zaman response olarak sunucu bir e-tag g�nderir.Bu bir token de�eridir.Browser ikinci bir istek yapt���nda dosyan�n de�i�ip de�i�medi�ini tespit etmek i�in kullan�l�r.enableEtag de�erini e�er false yaparsak her seferinde debug modta s�f�rdan olu�turulur App_Data
    //cacheControlMaxAge        => Dosyan�n saniye cinsinden ne kadar tutulaca��n� belirliyor. De�er �ayet 0 ise memoryde tutulmayaca��n� g�sterir.
    //bundle.CreateJs("bundle-s", "~/js").WithEnvironmentOptions(BundleEnvironmentOptions.Create()
    //               .ForDebug(builder => builder.EnableCompositeProcessing().EnableFileWatcher().SetCacheBusterType<AppDomainLifetimeCacheBuster>()
    //               .CacheControlOptions(enableEtag: false, cacheControlMaxAge: 0)).Build());

    //Tek tek dosyalar� belirtiyoruz ve o dosyalar i�in smidge uyguluyor.
    //bundle.CreateJs("bundle-s", "~/js/site.js", "~/js/site2.js");
    bundle.CreateCss("css-s", "~/css/site.css", "~/lib/bootstrap/dist/css/bootstrap.css", "~/SmidgeSkills.styles.css");
});



app.Run();
