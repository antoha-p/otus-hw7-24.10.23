namespace AsyncFileRead.FileReader;

public class FileReader
{
    /// <summary>
    /// Метод считает количество пробелов в файлах.
    /// </summary>
    /// <param name="path">Директория с файлами.</param>
    /// <param name="filesLimit">Максимальное количество файлов для подсчета пробелов. По умолчанию без ограничений</param>
    /// <returns>Число пробелов</returns>
    public async Task<long> CountSpacesAsync(string path, int filesLimit = -1)
    {
        //Заводим список тасок, которые будут запускаться параллельно.
        var tasks = new List<Task<long>>();

        //Счетчик обрабатываемых файлов.
        var filesCount = 0;

        //Проходим по всем файлам в директории.
        foreach (var file in Directory.EnumerateFiles(path))
        {
            //Вызываем функцию подсчета символов и сохраняем таск в список.
            var task = CountSymbolsAsync(file, ' ');
            tasks.Add(task);
            
            //Проверяем ограничение количества файлов.
            if (++filesCount == filesLimit)
                break;
        }

        //Запускаем все таски параллельно и дожидаемся их выполнения.
        await Task.WhenAll(tasks);

        //Получаем результат.
        var result = tasks.Sum(x => x.Result);
        return result;
    }

    /// <summary>
    /// Метод считает количество символов в указанном файле.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    /// <param name="symbol">Символ, который нужно посчитать.</param>
    /// <returns></returns>
    private async Task<long> CountSymbolsAsync(string path, char symbol)
    {
        //Заводим буффер для чтения одного символа из файла.
        var buffer = new byte[1];

        try
        {
            //Открываем поток чтения файла.
            using var stream = File.OpenRead(path);

            long result = 0;

            //Асинхронно читаем из файла под одному символу до конца файла.
            while (true)
            {
                //Читаем символ в буффер.
                var symbolsRead = await stream.ReadAsync(buffer);

                //Если ничего не прочитали, значит файл закончился.
                if (symbolsRead == 0)
                {
                    Console.WriteLine($"File [{path}]: count = {result}");
                    return result;
                }

                //Если прочитанный байт равен искомому символу, то увеличиваем счетчик.
                if (buffer[0] == symbol)
                {
                    //Если раскомментировать эту строку, то можно увидеть одновременное чтение символов из файлов.
                    Console.WriteLine($"Symbol found in [{path}]");
                    result++;
                }
            }
        }
        catch (Exception ex) //Обрабатываем исключение, чтобы ошибка чтения одного файла не привела к остановке чтения других файлов.
        {
            Console.WriteLine($"Cannot read file [{path}]: {ex.Message}");
            return 0;
        }
    }
}
