using BookReading.ViewModels;
using Stylet;
using StyletIoC;

namespace BookReading
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            builder.Bind<BookContext>().ToSelf().InSingletonScope();

        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
