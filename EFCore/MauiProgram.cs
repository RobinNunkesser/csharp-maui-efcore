using Microsoft.Extensions.Logging;
using TodoEF.Core;
using TodoEF.Core.ViewModels;
using TodoEF.Infrastructure;

namespace EFCore;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "todos.db3");
		builder.Services.AddSingleton<ITodoRepository>(_ => new EfCoreTodoRepository(dbPath));
		builder.Services.AddSingleton<TodoViewModel>();
		builder.Services.AddSingleton<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
