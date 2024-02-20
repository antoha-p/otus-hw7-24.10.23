
using System.Diagnostics;

namespace AsyncFileRead;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Started");

        //Прочитать 3 файла параллельно и вычислить количество пробелов в них(через Task).
        //Написать функцию, принимающую в качестве аргумента путь к папке. Из этой папки параллельно прочитать все файлы и вычислить количество пробелов в них.
        //Замерьте время выполнения кода(класс Stopwatch).

        var stopWatch = new Stopwatch();

        stopWatch.Start();

        var fileReader = new FileReader.FileReader();
        var result = await fileReader.CountSpacesAsync("../../../Example/", 3);
        
        stopWatch.Stop();

        Console.WriteLine($"Spaces count = {result}, time = {(long)stopWatch.Elapsed.TotalMilliseconds} msec");
    }
}
