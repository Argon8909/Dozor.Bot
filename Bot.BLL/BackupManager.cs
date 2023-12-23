using Newtonsoft.Json;

namespace Bot.BLL;

public static class BackupManager
{
    public static void WriteObjectToFile<T>(T obj, string filePath, bool append = true)
    {
        try
        {
            using (var streamWriter = new StreamWriter(filePath, append))
            {
                var json = JsonConvert.SerializeObject(obj);
                streamWriter.WriteLine(json);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка записи в файл!" + e);
            throw;
        }
       
    }

    public static List<T> ReadObjectsFromFile<T>(string filePath)
    {
        var objects = new List<T>();

        // Проверяем, существует ли файл
        if (File.Exists(filePath))
        {
            // Читаем все строки из файла
            string[] lines = File.ReadAllLines(filePath);
            // Преобразуем строки в объекты и добавляем в список
            foreach (var line in lines)
            {
                var obj = JsonConvert.DeserializeObject<T>(line);
                objects.Add(obj);
            }
        }

        return objects;
    }
}