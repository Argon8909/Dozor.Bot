using Newtonsoft.Json;

namespace Bot.BLL;

public class BackupManager
{
    public void WriteObjectToFile<T>(T obj, string filePath)
    {
        try
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                string json = JsonConvert.SerializeObject(obj);
                streamWriter.WriteLine(json);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка записи в файл!" + e);
            throw;
        }
       
    }

    public List<T> ReadObjectsFromFile<T>(string filePath)
    {
        List<T> objects = new List<T>();

        // Проверяем, существует ли файл
        if (File.Exists(filePath))
        {
            // Читаем все строки из файла
            string[] lines = File.ReadAllLines(filePath);
            // Преобразуем строки в объекты и добавляем в список
            foreach (string line in lines)
            {
                T obj = JsonConvert.DeserializeObject<T>(line);
                objects.Add(obj);
            }
        }

        return objects;
    }
}