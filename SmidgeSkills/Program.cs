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
    //Js root içerisindeki bütün javascript dosyalarýna smidge uyguluyor.
    bundle.CreateJs("bundle-s", "~/js");

    //EnableCompositeProcessing => Birleþtirme iþlemini gerçekleþtiriyor.
    //EnableFileWatcher         => Bundle içerisindeki dosyalarý izler ve deðiþiklik algýlarsa cache dosyasýný(App_Data)sýfýrdan oluþturuyor.
    //SetCacheBusterType        => App_Data cache dosyasýný uygulama her ayaða kalktýðýnda oluþturur.
    //CacheControlOptions       => Browser' a, oluþan bundle dosyasýný hiç cachelememesini söylüyoruz.
    //enableTag                 => Browser, serverdan image veya dosya çektiði zaman response olarak sunucu bir e-tag gönderir.Bu bir token deðeridir.Browser ikinci bir istek yaptýðýnda dosyanýn deðiþip deðiþmediðini tespit etmek için kullanýlýr.enableEtag deðerini eðer false yaparsak her seferinde debug modta sýfýrdan oluþturulur App_Data
    //cacheControlMaxAge        => Dosyanýn saniye cinsinden ne kadar tutulacaðýný belirliyor. Deðer þayet 0 ise memoryde tutulmayacaðýný gösterir.
    //bundle.CreateJs("bundle-s", "~/js").WithEnvironmentOptions(BundleEnvironmentOptions.Create()
    //               .ForDebug(builder => builder.EnableCompositeProcessing().EnableFileWatcher().SetCacheBusterType<AppDomainLifetimeCacheBuster>()
    //               .CacheControlOptions(enableEtag: false, cacheControlMaxAge: 0)).Build());

    //Tek tek dosyalarý belirtiyoruz ve o dosyalar için smidge uyguluyor.
    //bundle.CreateJs("bundle-s", "~/js/site.js", "~/js/site2.js");
    bundle.CreateCss("css-s", "~/css/site.css", "~/lib/bootstrap/dist/css/bootstrap.css", "~/SmidgeSkills.styles.css");
});



app.Run();
